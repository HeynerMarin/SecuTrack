using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SecuTrack.Application.DTOs;
using SecuTrack.Application.Services;
using SecuTrack.Core.Interfaces;

namespace SecuTrack.Web.Controllers
{
    public class AuditsController : Controller
    {
        private readonly IAuditService _auditService;
        private readonly IProviderService _providerService;
        private readonly IUnitOfWork _unitOfWork;

        public AuditsController(IAuditService auditService, IProviderService providerService, IUnitOfWork unitOfWork)
        {
            _auditService = auditService;
            _providerService = providerService;
            _unitOfWork = unitOfWork;
        }

        // GET: Audits
        public async Task<IActionResult> Index()
        {
            var audits = await _auditService.GetAllAuditsAsync();
            return View(audits);
        }

        // GET: Audits/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var audit = await _auditService.GetAuditDetailAsync(id);
            if (audit == null)
            {
                return NotFound();
            }
            return View(audit);
        }

        // GET: Audits/Create
        public async Task<IActionResult> Create()
        {
            var providers = await _providerService.GetAllProvidersAsync();
            ViewBag.Providers = new SelectList(providers, "Id", "Name");
            return View();
        }

        // POST: Audits/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAuditDto dto)
        {
            if (ModelState.IsValid)
            {
                var audit = await _auditService.CreateAuditAsync(dto);
                TempData["Success"] = "Auditoría creada exitosamente.";
                return RedirectToAction(nameof(Evaluate), new { id = audit.Id });
            }

            var providers = await _providerService.GetAllProvidersAsync();
            ViewBag.Providers = new SelectList(providers, "Id", "Name");
            return View(dto);
        }

        // GET: Audits/Evaluate/5
        public async Task<IActionResult> Evaluate(int id)
        {
            var audit = await _auditService.GetAuditDetailAsync(id);
            if (audit == null)
            {
                return NotFound();
            }

            if (audit.IsCompleted)
            {
                TempData["Warning"] = "Esta auditoría ya está completada.";
                return RedirectToAction(nameof(Details), new { id });
            }

            var controls = await _unitOfWork.Repository<SecuTrack.Core.Entities.ISOControl>().GetAllAsync();
            ViewBag.Controls = controls.OrderBy(c => c.ControlId).ToList();

            return View(audit);
        }

        // POST: Audits/SaveEvaluation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEvaluation(CreateEvaluationDto dto)
        {
            if (ModelState.IsValid)
            {
                // Verificar si ya existe una evaluación para este control en esta auditoría
                var existing = await _unitOfWork.Evaluations.FirstOrDefaultAsync(
                    e => e.AuditSessionId == dto.AuditSessionId && e.ISOControlId == dto.ISOControlId
                );

                if (existing != null)
                {
                    // Actualizar evaluación existente
                    existing.ComplianceLevel = dto.ComplianceLevel;
                    existing.Evidence = dto.Evidence;
                    existing.AuditorNotes = dto.AuditorNotes;
                    existing.Recommendations = dto.Recommendations;
                    existing.UpdatedAt = DateTime.UtcNow;

                    _unitOfWork.Evaluations.Update(existing);
                }
                else
                {
                    // Crear nueva evaluación
                    var evaluation = new SecuTrack.Core.Entities.Evaluation
                    {
                        AuditSessionId = dto.AuditSessionId,
                        ISOControlId = dto.ISOControlId,
                        ComplianceLevel = dto.ComplianceLevel,
                        Evidence = dto.Evidence,
                        AuditorNotes = dto.AuditorNotes,
                        Recommendations = dto.Recommendations
                    };

                    await _unitOfWork.Evaluations.AddAsync(evaluation);
                }

                await _unitOfWork.SaveChangesAsync();

                return Json(new { success = true, message = "Evaluación guardada correctamente." });
            }

            return Json(new { success = false, message = "Error al guardar la evaluación." });
        }

        // POST: Audits/Complete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete(int id)
        {
            var audit = await _auditService.GetAuditDetailAsync(id);
            if (audit == null)
            {
                return NotFound();
            }

            // Calcular compliance score
            await _auditService.CalculateComplianceScoreAsync(id);

            // Marcar como completada
            await _auditService.CompleteAuditAsync(id);

            TempData["Success"] = "Auditoría completada y score calculado exitosamente.";
            return RedirectToAction(nameof(Details), new { id });
        }

        // GET: Audits/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var audit = await _auditService.GetAuditDetailAsync(id);
            if (audit == null)
            {
                return NotFound();
            }
            return View(audit);
        }

        // POST: Audits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var audit = await _unitOfWork.Audits.GetByIdAsync(id);
            if (audit != null)
            {
                audit.IsActive = false;
                audit.UpdatedAt = DateTime.UtcNow;
                _unitOfWork.Audits.Update(audit);
                await _unitOfWork.SaveChangesAsync();

                TempData["Success"] = "Auditoría eliminada exitosamente.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
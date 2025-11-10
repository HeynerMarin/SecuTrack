using Microsoft.AspNetCore.Mvc;
using SecuTrack.Application.Services;
using SecuTrack.Core.Enums;
using SecuTrack.Core.Interfaces;

namespace SecuTrack.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IProviderService _providerService;
        private readonly IAuditService _auditService;
        private readonly IUnitOfWork _unitOfWork;

        public DashboardController(IProviderService providerService, IAuditService auditService, IUnitOfWork unitOfWork)
        {
            _providerService = providerService;
            _auditService = auditService;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var providers = await _providerService.GetAllProvidersAsync();
            ViewBag.Providers = providers;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetComparisonData()
        {
            var audits = await _unitOfWork.Audits.GetCompletedAuditsAsync();

            var data = audits
                .Where(a => a.ComplianceScore != null)
                .Select(a => new
                {
                    provider = a.Provider.Name,
                    globalScore = a.ComplianceScore!.GlobalScore,
                    organizationalScore = a.ComplianceScore.OrganizationalScore,
                    peopleScore = a.ComplianceScore.PeopleScore,
                    physicalScore = a.ComplianceScore.PhysicalScore,
                    technologicalScore = a.ComplianceScore.TechnologicalScore,
                    auditDate = a.AuditDate.ToString("yyyy-MM-dd")
                })
                .ToList();

            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetProviderHistory(int providerId)
        {
            var audits = await _unitOfWork.Audits.GetAuditsByProviderAsync(providerId);

            var data = audits
                .Where(a => a.ComplianceScore != null && a.IsCompleted)
                .OrderBy(a => a.AuditDate)
                .Select(a => new
                {
                    date = a.AuditDate.ToString("yyyy-MM-dd"),
                    score = a.ComplianceScore!.GlobalScore,
                    auditor = a.AuditorName
                })
                .ToList();

            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoryBreakdown(int providerId)
        {
            var latestAudit = (await _unitOfWork.Audits.GetAuditsByProviderAsync(providerId))
                .Where(a => a.ComplianceScore != null && a.IsCompleted)
                .OrderByDescending(a => a.AuditDate)
                .FirstOrDefault();

            if (latestAudit?.ComplianceScore == null)
            {
                return Json(new { error = "No hay auditorías completadas" });
            }

            var data = new
            {
                categories = new[] { "Organizacional", "Personas", "Físico", "Tecnológico" },
                scores = new[]
                {
                    latestAudit.ComplianceScore.OrganizationalScore,
                    latestAudit.ComplianceScore.PeopleScore,
                    latestAudit.ComplianceScore.PhysicalScore,
                    latestAudit.ComplianceScore.TechnologicalScore
                }
            };

            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetCriticalGaps()
        {
            var audits = await _unitOfWork.Audits.GetCompletedAuditsAsync();

            var data = audits
                .Where(a => a.ComplianceScore != null)
                .Select(a => new
                {
                    provider = a.Provider.Name,
                    criticalGaps = a.ComplianceScore!.CriticalGaps,
                    totalControls = a.ComplianceScore.TotalControls
                })
                .ToList();

            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetComplianceDistribution()
        {
            var allEvaluations = new List<SecuTrack.Core.Entities.Evaluation>();
            var completedAudits = await _unitOfWork.Audits.GetCompletedAuditsAsync();

            foreach (var audit in completedAudits)
            {
                var evals = await _unitOfWork.Evaluations.GetEvaluationsByAuditAsync(audit.Id);
                allEvaluations.AddRange(evals);
            }

            var distribution = allEvaluations
                .GroupBy(e => e.ComplianceLevel)
                .Select(g => new
                {
                    level = GetComplianceLevelName(g.Key),
                    count = g.Count()
                })
                .OrderBy(x => x.level)
                .ToList();

            return Json(distribution);
        }

        private string GetComplianceLevelName(ComplianceLevel level)
        {
            return level switch
            {
                ComplianceLevel.NotImplemented => "No Implementado",
                ComplianceLevel.PartiallyImplemented => "Parcial (25%)",
                ComplianceLevel.LargelyImplemented => "Mayormente (50%)",
                ComplianceLevel.FullyImplemented => "Completo (75%)",
                ComplianceLevel.ExceedsRequirements => "Excelente (100%)",
                _ => "Desconocido"
            };
        }
    }
}
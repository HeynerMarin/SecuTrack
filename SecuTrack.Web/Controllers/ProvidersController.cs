using Microsoft.AspNetCore.Mvc;
using SecuTrack.Application.DTOs;
using SecuTrack.Application.Services;

namespace SecuTrack.Web.Controllers
{
    public class ProvidersController : Controller
    {
        private readonly IProviderService _providerService;
        private readonly IAuditService _auditService;

        public ProvidersController(IProviderService providerService, IAuditService auditService)
        {
            _providerService = providerService;
            _auditService = auditService;
        }

        // GET: Providers
        public async Task<IActionResult> Index()
        {
            var providers = await _providerService.GetAllProvidersAsync();
            return View(providers);
        }

        // GET: Providers/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var provider = await _providerService.GetProviderByIdAsync(id);
            if (provider == null)
            {
                return NotFound();
            }

            var audits = await _auditService.GetAuditsByProviderAsync(id);
            ViewBag.Audits = audits;

            return View(provider);
        }

        // GET: Providers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Providers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProviderDto dto)
        {
            if (ModelState.IsValid)
            {
                await _providerService.CreateProviderAsync(dto);
                TempData["Success"] = "Proveedor creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(dto);
        }

        // GET: Providers/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var provider = await _providerService.GetProviderByIdAsync(id);
            if (provider == null)
            {
                return NotFound();
            }

            var dto = new CreateProviderDto
            {
                Name = provider.Name,
                Type = provider.Type,
                Region = provider.Region,
                Description = provider.Description,
                ContactEmail = provider.ContactEmail,
                WebsiteUrl = provider.WebsiteUrl
            };

            return View(dto);
        }

        // POST: Providers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateProviderDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _providerService.UpdateProviderAsync(id, dto);
                if (result)
                {
                    TempData["Success"] = "Proveedor actualizado exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                return NotFound();
            }
            return View(dto);
        }

        // GET: Providers/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var provider = await _providerService.GetProviderByIdAsync(id);
            if (provider == null)
            {
                return NotFound();
            }
            return View(provider);
        }

        // POST: Providers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _providerService.DeleteProviderAsync(id);
            if (result)
            {
                TempData["Success"] = "Proveedor eliminado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }
    }
}
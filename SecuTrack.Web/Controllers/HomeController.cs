using Microsoft.AspNetCore.Mvc;
using SecuTrack.Application.Services;

namespace SecuTrack.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProviderService _providerService;
        private readonly IAuditService _auditService;

        public HomeController(IProviderService providerService, IAuditService auditService)
        {
            _providerService = providerService;
            _auditService = auditService;
        }

        public async Task<IActionResult> Index()
        {
            var providers = await _providerService.GetAllProvidersAsync();
            var audits = await _auditService.GetAllAuditsAsync();

            ViewBag.TotalProviders = providers.Count();
            ViewBag.TotalAudits = audits.Count();
            ViewBag.CompletedAudits = audits.Count(a => a.IsCompleted);
            ViewBag.AverageScore = audits.Any() && audits.Any(a => a.GlobalScore.HasValue)
                ? Math.Round(audits.Where(a => a.GlobalScore.HasValue).Average(a => a.GlobalScore!.Value), 2)
                : 0;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
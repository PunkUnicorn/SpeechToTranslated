using Microsoft.AspNetCore.Mvc;

namespace ChurchReader.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TranslationController : ControllerBase
    {
        private readonly ILogger<TranslationController> _logger;

        public TranslationController(ILogger<TranslationController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "Index")]
        [Route(@"\")]
        public ActionResult Get(string languageCode)
        {
            return View();
        }

        [HttpPost(Name = "Translation")]
        [Route(@"[controller]\{languageCode}")]
        public int Post(string languageCode)
        {
            
            return 1;
        }
    }
}

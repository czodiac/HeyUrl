using System;
using System.Collections.Generic;
using System.Linq;
using hey_url_challenge_code_dotnet.Models;
using hey_url_challenge_code_dotnet.ViewModels;
using HeyUrlChallengeCodeDotnet.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shyjus.BrowserDetection;
using Web_Application.Models;

namespace HeyUrlChallengeCodeDotnet.Controllers {
    [Route("/")]
    public class UrlsController : Controller {
        private readonly ILogger<UrlsController> _logger;
        private static readonly Random getrandom = new Random();
        private readonly IBrowserDetector browserDetector;
        private readonly ApplicationContext _db;

        public UrlsController(ILogger<UrlsController> logger, IBrowserDetector browserDetector, ApplicationContext db) {
            this.browserDetector = browserDetector;
            _logger = logger;
            _db = db;
        }

        public IActionResult Index() {
            var model = new HomeViewModel();
            model.Urls = _db.UrlModel;
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken] // CSRF
        public IActionResult Index(HomeViewModel obj) {
            if (ModelState.IsValid) {
                UrlModel.CreateShortURL(_db, obj.NewUrl, false);
            }
            var model = new HomeViewModel();
            model.Urls = _db.UrlModel;
            return View(model);
        }

        [Route("/{url}")]
        public IActionResult Visit(string url) {
            string fullUrl = UrlModel.GetFullUrlFromShortUrl(_db, this.browserDetector.Browser, url);
            if (!string.IsNullOrEmpty(fullUrl)) {
                return Redirect(fullUrl);
            } else {
                return NotFound();
            }
        }

        [Route("urls/{url}")]
        public IActionResult Show(string url) {
            return View(BrowserModel.GetGraphData(_db, url));
        }
    }
}
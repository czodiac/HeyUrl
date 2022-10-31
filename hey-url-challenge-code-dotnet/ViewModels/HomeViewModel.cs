using System.Collections.Generic;
using hey_url_challenge_code_dotnet.Models;

namespace hey_url_challenge_code_dotnet.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<UrlModel> Urls { get; set; }
        public UrlModel NewUrl { get; set; }
    }
}

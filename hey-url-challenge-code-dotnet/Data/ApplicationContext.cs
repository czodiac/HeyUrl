using hey_url_challenge_code_dotnet.Models;
using Microsoft.EntityFrameworkCore;
using Web_Application.Models;

namespace HeyUrlChallengeCodeDotnet.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<UrlModel> UrlModel { get; set; }

        public DbSet<BrowserModel> BrowserModel { get; set; }
    }
}
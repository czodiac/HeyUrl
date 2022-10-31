using hey_url_challenge_code_dotnet.Models;
using hey_url_challenge_code_dotnet.ViewModels;
using HeyUrlChallengeCodeDotnet.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Web_Application.Models {
    public class BrowserModel {

        [Key]
        public int Id { get; set; }

        [Required]
        public string ShortUrl { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string OS { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;

        public static ShowViewModel GetGraphData(ApplicationContext _db, string url) {
            UrlModel um = _db.UrlModel.Where(x => x.ShortUrl.Equals(url)).FirstOrDefault();

            int Clicks_ie = 0;
            int Clicks_ff = 0;
            int Clicks_chrome = 0;
            int Clicks_safari = 0;

            int Clicks_win = 0;
            int Clicks_mac = 0;
            int Clicks_ubuntu = 0;
            int Clicks_other = 0;

            var records = _db.BrowserModel.Where(x => x.ShortUrl.Equals(url));
            foreach (BrowserModel bm in records) {
                switch (bm.Name) {
                    case "IE":
                        Clicks_ie++;
                        break;
                    case "Firefox":
                        Clicks_ff++;
                        break;
                    case "Chrome":
                        Clicks_chrome++;
                        break;
                    case "Safari":
                        Clicks_safari++;
                        break;
                }

                switch (bm.OS) {
                    case "Windows":
                        Clicks_win++;
                        break;
                    case "macOS":
                        Clicks_mac++;
                        break;
                    case "Ubuntu":
                        Clicks_ubuntu++;
                        break;
                    case "Other":
                        Clicks_other++;
                        break;
                }
            }

            // Click count for each day of this month
            Dictionary<string, int> clicks = new Dictionary<string, int>();
            int thisMonth = DateTime.Now.Month;
            var clickRecord = _db.BrowserModel.Where(x => x.ShortUrl.Equals(url) && x.Created.Month == thisMonth).OrderBy(x => x.Created); // Get this month data order by date
            foreach (BrowserModel bm in records) {
                string day = bm.Created.Day.ToString();
                if (clicks.ContainsKey(day)) {
                    // Increment by 1
                    clicks[day] = clicks[day] + 1;
                } else {
                    // Add new value pair in the dictionary.
                    clicks.Add(day, 1);
                }
            }

            return new ShowViewModel {
                Url = um,
                DailyClicks = clicks,
                BrowseClicks = new Dictionary<string, int>
                {
                    { "IE", Clicks_ie },
                    { "Firefox", Clicks_ff },
                    { "Chrome", Clicks_chrome },
                    { "Safari", Clicks_safari },
                },
                PlatformClicks = new Dictionary<string, int>
                {
                    { "Windows", Clicks_win },
                    { "macOS", Clicks_mac },
                    { "Ubuntu", Clicks_ubuntu },
                    { "Other", Clicks_other },
                }
            };
        }
    }
}

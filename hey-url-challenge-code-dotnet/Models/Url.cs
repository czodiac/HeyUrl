using HeyUrlChallengeCodeDotnet.Data;
using Shyjus.BrowserDetection;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Web_Application.Models;

namespace hey_url_challenge_code_dotnet.Models
{
    public class UrlModel
    {
        [Key]
        public int Id { get; set; }

        public string ShortUrl { get; set; }

        [Required]
        public string FullUrl { get; set; }
        
        public int Count { get; set; } // Click count

        public DateTime Created { get; set; } = DateTime.Now;

        public static bool CreateShortURL(ApplicationContext _db, UrlModel um, bool isUnitTest) {
            bool result = false;

            try {
                // Check if URL is valid
                bool isValidURL = Uri.IsWellFormedUriString(um.FullUrl, UriKind.Absolute);
                if (isValidURL) {
                    // Create short url until it is unique
                    bool isUnique = false;
                    string s = string.Empty;
                    while (!isUnique) {
                        // Create short url
                        Random r = new Random();
                        string alph = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                        const int MAX_URL_LENGTH = 5;
                        for (int i = 0; i < MAX_URL_LENGTH; i++) {
                            int idx = r.Next(alph.Length);
                            s += alph[idx].ToString();
                        }

                        // Check if it is unique
                        bool uniqueChecked = IsUniqueShortUrl(_db, s, isUnitTest);
                        if (uniqueChecked) {
                            isUnique = true;
                        } else {
                            s = string.Empty;
                        }
                    }

                    // Insert into DB
                    um.ShortUrl = s;
                    if (!isUnitTest) {
                        _db.UrlModel.Add(um);
                        _db.SaveChanges();
                    }
                    result = true;
                }
            } catch (Exception) {
                // Do something with error
            }
            return result;
        }

        private static bool IsUniqueShortUrl(ApplicationContext _db, string shortUrl, bool isUnitTest) {
            bool isUnique = true;
            if (!isUnitTest) {
                foreach (var m in _db.UrlModel) {
                    if (m.ShortUrl.Equals(shortUrl)) {
                        isUnique = false;
                        break;
                    }
                }
            }
            return isUnique;
        }

        public static string GetFullUrlFromShortUrl(ApplicationContext _db, IBrowser browser, string url) {
            string fullUrl = string.Empty;
            UrlModel um = _db.UrlModel.Where(x => x.ShortUrl.Equals(url)).FirstOrDefault();
            if (!string.IsNullOrEmpty(um.FullUrl)) {
                // Record browser info
                BrowserModel bm = new BrowserModel();
                bm.ShortUrl = um.ShortUrl;
                bm.Name = browser.Name;
                bm.OS = browser.OS;
                _db.BrowserModel.Add(bm);
                _db.SaveChanges();

                // Update click count (increment count)
                um.Count += 1;
                _db.UrlModel.Update(um);
                _db.SaveChanges();

                // Found full url
                fullUrl = um.FullUrl;
            }
            return fullUrl;
        }
    }
}

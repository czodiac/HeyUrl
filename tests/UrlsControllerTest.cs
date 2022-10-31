using hey_url_challenge_code_dotnet.Models;
using NUnit.Framework;


// Test business logic except DB execution part.

namespace HeyURL
{
    public class UrlsControllerTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("-1", ExpectedResult = false)]
        [TestCase("url", ExpectedResult = false)]
        [TestCase("http://www.gmail.com", ExpectedResult = true)]
        [TestCase("https://www.gmail.com", ExpectedResult = true)]
        public bool UrlModel_CreateShortURL_Test(string input) {
            UrlModel um = new UrlModel();
            um.FullUrl = input;
            //public static bool CreateShortURL(ApplicationContext _db, UrlModel um, IBrowser browser, bool isUnitTest) {
            bool result = UrlModel.CreateShortURL(null, um, true);
            return result;
        }
    }
}
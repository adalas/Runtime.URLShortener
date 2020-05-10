using Xunit;
using Runtime.URLShortener.ApplicationCore.Entities.ValueObject;

namespace Runtime.URLShortener.UnitsTests.ApplicationCore.Domain.URLTests
{
    public class ShortURLTest
    {
        public static readonly string _url = "http://abola.pt";
        public static readonly string _shortURL = "tWI+sxlFRV0=";

        // [Fact]
        // public void CreateShortURL()
        // {
        //     ShortURL su = new ShortURL(_url);
        // }

        [Fact]
        public void CreateShortURL()
        {
            var su = new ShortURL(_shortURL);
            Assert.Equal(_shortURL, su.SUrl);
            su = ShortURL.ComputeShortURLFromExtendedURL(_url);
            Assert.Equal(_shortURL,su.SUrl);
        }
    }
}
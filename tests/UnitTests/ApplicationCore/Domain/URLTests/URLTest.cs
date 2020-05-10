using Runtime.URLShortener.ApplicationCore.Entities;
using Runtime.URLShortener.ApplicationCore.Entities.ValueObject;
using Xunit;

namespace Runtime.URLShortener.UnitsTests.ApplicationCore.Domain.URLTests {
    public class URLTest {

        private readonly string _urlvalue = "http://record.pt";
        private readonly string _encodedURL = "adfasdfasdf";
                [Fact]
        public void CreateURL() {
            URL u = new URL(_urlvalue);
            Assert.Equal(_urlvalue,u.Value);

            ShortURL surl = new ShortURL(_encodedURL);
            u = new URL(_urlvalue,surl);
            Assert.Equal(surl,u.Id);
            Assert.Equal(_urlvalue,u.Value);

        }
    }
}
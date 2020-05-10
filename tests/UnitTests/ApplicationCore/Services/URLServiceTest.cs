using Runtime.URLShortener.ApplicationCore.Entities;
using Runtime.URLShortener.ApplicationCore.Entities.ValueObject;
using Xunit;
using Moq;
using Runtime.URLShortener.ApplicationCore.Interfaces;
using System.Threading.Tasks;

using Runtime.URLShortener.ApplicationCore.Interfaces.Config;
using Runtime.URLShortener.UnitsTests.ApplicationCore.Domain.URLTests;
using Runtime.UnitTests.Helper;
using System;
using Runtime.URLShortener.Web.Services;

namespace Runtime.URLShortener.UnitsTests.ApplicationCore.Services.URLTests
{
    public class URLServiceTest {
    
        private readonly Mock<IURLRepository> _mockUrlRepo;
        private readonly Mock<IConfigApplicationLimits> _mockConfig;
        private readonly ShortURL _surl;
        private readonly URL _url;
        private readonly string _goodSurl;
        private readonly string _badSurl;

        public URLServiceTest()
        {
            _mockUrlRepo = new Mock<IURLRepository>();

            _surl = ShortURL.ComputeShortURLFromExtendedURL(ShortURLTest._shortURL);
            _url = new URL(ShortURLTest._url, _surl);
            _mockUrlRepo.Setup(x => x.GetByIdAsync(It.IsAny<ShortURL>())).ReturnsAsync(_url);

            _mockConfig = new Mock<IConfigApplicationLimits>();
            _mockConfig.Setup(x =>x.MaxURLChars).Returns(100);
            _mockConfig.Setup(x => x.ShortUrlBase64Padding).Returns("=");
            _mockConfig.Setup(x => x.ShortUrlBase64PaddingLength).Returns(1);
            _mockConfig.Setup(x => x.ShortUrlLength).Returns(11);
            _goodSurl = StringGenerator.RandomString(11,false);
            _badSurl = "asda";
        }

        [Fact]
        public async Task GetURLByID_GoodAndBadInput_Test()
        {
            

            //  var uservice = new URLService(_mockUrlRepo.Object,null,_mockConfig.Object);
            //  var nurl = await uservice.GetURLAsync(_goodSurl);
            //  Assert.Equal(_url.Value,nurl);

            //  await Assert.ThrowsAnyAsync<Exception>( async () => await uservice.GetURLAsync(_badSurl));
        }

    
    }
}
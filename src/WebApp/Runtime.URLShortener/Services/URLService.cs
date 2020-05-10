using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Runtime.URLShortener.ApplicationCore.Entities;
using Runtime.URLShortener.ApplicationCore.Entities.ValueObject;
using Runtime.URLShortener.ApplicationCore.Exceptions;
using Runtime.URLShortener.ApplicationCore.Interfaces;
using Runtime.URLShortener.ApplicationCore.Interfaces.Config;
using Microsoft.Extensions.Options;
using Runtime.URLShortener.Infrastructure.Config;

namespace Runtime.URLShortener.Web.Services {
    public class URLService:IURLService
    {
        private readonly IURLRepository _urlRepository;
        private readonly IAppLogger<URLService> _logger;
        private readonly IConfigApplicationLimits _config;

        public URLService(IURLRepository urlRepostitory,
                          IAppLogger<URLService> logger,
                          IOptions<ConfigApplicationLimits> config)
        {
            _urlRepository = urlRepostitory;
            _logger = logger;
            _config = config.Value;            
        }

        private string RemoveBase64Strings(string base64str)
        {
            int base64paddingtoRemove = _config.ShortUrlBase64PaddingLength;
            return base64str.Remove(base64str.Length-base64paddingtoRemove,base64paddingtoRemove);
        }

        private string AddBase64Strings(string simplifiedBase64Str)
        {

            return $"{simplifiedBase64Str}{_config.ShortUrlBase64Padding}";
        }
        public async Task<string> CreateURLAsync(string extendedURL)
        {
            Guard.Against.NullOrEmpty(extendedURL,"URL value");
            Guard.Against.OutOfRange(extendedURL.Length,"URL value",1,_config.MaxURLChars);
            string res = null;
            try {
                ShortURL surl = ShortURL.ComputeShortURLFromExtendedURL(extendedURL);
                if (! await _urlRepository.ExistsAsync(surl)) 
                {
                    URL newURL = new URL(extendedURL, surl);
                    await _urlRepository.AddAsync(newURL);
                }                

                res = RemoveBase64Strings(surl.ToString());
            } catch (Exception excp) {
                _logger.LogError(excp, $"Error creating url with value: {extendedURL}.");
            }
            return res;
        }

        public Task DeleteURLAsync(string urlID)
        {
            throw new System.NotImplementedException();
        }
        public async Task<string> GetURLAsync(string shortUrl)
        {
            Guard.Against.NullOrEmpty(shortUrl, "Short URL");
            Guard.Against.OutOfRange(shortUrl.Length,"Short URL",_config.ShortUrlLength,_config.ShortUrlLength);
            URL res = null;
            try {
                ShortURL id = new ShortURL(AddBase64Strings(shortUrl));
                res = await _urlRepository.GetByIdAsync(id);
                if (res == null)
                    throw new URLNotFoundException(shortUrl);
                return res.Value;

            }catch(Exception excp)
            {
                _logger.LogError(excp,$"Error fetching url with shortURL: {shortUrl}.");
                res = null;
            }
            return res?.Value;

        }
    }
}
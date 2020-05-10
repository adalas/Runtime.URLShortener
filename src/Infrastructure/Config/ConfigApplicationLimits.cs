using System;
using Microsoft.Extensions.Configuration;
using Runtime.URLShortener.ApplicationCore.Interfaces.Config;

namespace Runtime.URLShortener.Infrastructure.Config
{
    public class ConfigApplicationLimits:IConfigApplicationLimits
    {
        public int MaxURLChars {get; set;}

        public string ShortUrlBase64Padding {get; set;}

        public int ShortUrlBase64PaddingLength {get{return ShortUrlBase64Padding.Length;}}

        public int ShortUrlLength {get; set;}
    }
}

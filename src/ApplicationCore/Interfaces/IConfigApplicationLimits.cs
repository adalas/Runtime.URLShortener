using System;

namespace Runtime.URLShortener.ApplicationCore.Interfaces.Config
{
    public interface IConfigApplicationLimits
    {
        int MaxURLChars {get;}
        string ShortUrlBase64Padding {get;}
        int ShortUrlBase64PaddingLength {get;}
        int ShortUrlLength {get;}
    }
}
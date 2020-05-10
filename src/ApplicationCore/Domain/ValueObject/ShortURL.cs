using System;
using System.Text;
using Extensions.Data;
namespace Runtime.URLShortener.ApplicationCore.Entities.ValueObject
{
    public class ShortURL {
        public string SUrl;

        public ShortURL(string shortUrl)
        {
            SUrl = shortUrl;
        } 


        public static string ComputeURLHash(string value)
        {
            byte[] vba = Encoding.UTF8.GetBytes(value);
            ulong hv = XXHash.XXH64(vba);

            
            return Convert.ToBase64String(BitConverter.GetBytes(hv));
        }


        public static ShortURL ComputeShortURLFromExtendedURL(string extendedURL)
        {
            string surl = ComputeURLHash(extendedURL);
            return new ShortURL(surl);
        }

        public override string ToString()
        {
            return SUrl.ToString();
        }


    }
}
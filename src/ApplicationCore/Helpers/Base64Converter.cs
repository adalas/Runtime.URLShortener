using System;

namespace Runtime.URLShortener.ApplicationCore.Helpers
{
    public class Base64Converter {
        public static string EncodeBase64(int id)
        {
            //it must be a multiple of 3. For 32bit it will remove on padding
            return Convert.ToBase64String(BitConverter.GetBytes(id)).TrimEnd('='); 
        } 

        public static int DecodeBase64(string shortURL)
        {
            return BitConverter.ToInt32(Convert.FromBase64String($"{shortURL}="));
        }
    }
}
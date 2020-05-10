using System;

namespace Runtime.URLShortener.ApplicationCore.Exceptions
{
    public class URLNotFoundException:Exception
    {
        public URLNotFoundException(string hexShortened):base($"No URL found with shortname {hexShortened}")
        {
            
        }
    }
}
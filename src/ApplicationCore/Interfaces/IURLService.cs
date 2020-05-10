using System.Collections.Generic;
using System.Threading.Tasks;
using Runtime.URLShortener.ApplicationCore.Entities;

namespace Runtime.URLShortener.ApplicationCore.Interfaces {

    public interface IURLService
    {
        Task<string> CreateURLAsync(string extendedURL);
        Task DeleteURLAsync(string urlID);
        Task<string> GetURLAsync(string shortUrl);

    } 
}

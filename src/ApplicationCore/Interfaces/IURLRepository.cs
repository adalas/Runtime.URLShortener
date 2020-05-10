using System.Collections.Generic;
using System.Threading.Tasks;
using Runtime.URLShortener.ApplicationCore.Entities;
using Runtime.URLShortener.ApplicationCore.Entities.ValueObject;

namespace Runtime.URLShortener.ApplicationCore.Interfaces {
    public interface IURLRepository {
        Task<URL> GetByIdAsync(ShortURL id);
        Task<ShortURL> AddAsync(URL entity);
        void DeleteFireAndForget(ShortURL entity);

        Task<bool> ExistsAsync(ShortURL id);
    }
}
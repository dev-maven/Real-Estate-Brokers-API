using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Urban.ng.Helpers;
using Urban.ng.Models;

namespace Urban.ng.Data
{
   public interface IUserRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();
        Task<ProfilePhoto> GetProfilePhoto(int id);
        Task<PagedList<User>> GetUsers(UserParams userParams);
        Task<User> GetUser(int id);

    }
}

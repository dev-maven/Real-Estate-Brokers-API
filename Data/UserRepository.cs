using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Urban.ng.Helpers;
using Urban.ng.Models;

namespace Urban.ng.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<ProfilePhoto> GetProfilePhoto(int id)
        {
            var photo = await _context.ProfilePhotos.FirstOrDefaultAsync(p => p.ProfilePhotoId == id);
            return photo;
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users
                .Include(p => p.ProfilePhoto)
                .Include(p => p.UserRoles)
                .FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var users = _context.Users
                .Include(x => x.UserRoles)
                .Include(x => x.Properties)
                .Include(x => x.ProfilePhoto)
                .AsQueryable();
            users = users.Where(u => u.Id != userParams.UserId);

            if (!string.IsNullOrEmpty(userParams.OrderBy))
            {
                switch (userParams.OrderBy)
                {
                    case "sort":
                        users = users.OrderByDescending(u => u.DateRegistered);
                        break;
                }
            }

            return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Urban.ng.Helpers;
using Urban.ng.Models;

namespace Urban.ng.Data
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly DataContext _context;

        public PropertyRepository(DataContext context)
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

        public async Task<Feature> GetFeature(int id)
        {
            return await _context.Features.FirstOrDefaultAsync(u => u.FeatureId == id);
        }

        public async Task<Photo> GetMainPhotoForProperty(int propertyId)
        {
            return await _context.Photos.Where(u => u.PropertyId == propertyId)
               .FirstOrDefaultAsync(p => p.IsMain);
        }

        public async Task<PagedList<Property>> GetMyProperties(int id, UserParams userParams)
        {
            var properties = _context.Properties
                 .Include(p => p.Photos)
                 .Include(p => p.Features)
                .OrderByDescending(p => p.DateAdded).AsQueryable()
                .Where(u => u.UserId == id);
            return await PagedList<Property>.CreateAsync(properties, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<Photo> GetPhoto(int id)
        {
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.PhotoId == id);
            return photo;
        }

        public async Task<Plan> GetPlan(int id)
        {
            var plan = await _context.Plans.FirstOrDefaultAsync(p => p.PlanId == id);
            return plan;
        }

        public async Task<PagedList<Property>> GetProperties(UserParams userParams)
        {
            var properties = _context.Properties
                .Include(p => p.Photos)
                .Include(p => p.Features)
               .OrderByDescending(p => p.DateAdded).AsQueryable();
            return await PagedList<Property>.CreateAsync(properties, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<Property> GetProperty(int id)
        {
            var propertyDetails = await _context.Properties
              .Include(x => x.Photos)
              .Include(x => x.Features)
              .Include(x => x.Plans)
              .Include(x => x.Videos)
              .Include(x => x.User)
              .FirstOrDefaultAsync(u => u.PropertyId == id);

            return propertyDetails;
        }

        public async Task<Video> GetVideo(int id)
        {
            var video = await _context.Videos.FirstOrDefaultAsync(p => p.VideoId == id);
            return video;
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

     
    }
}

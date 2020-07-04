using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Urban.ng.Helpers;
using Urban.ng.Models;

namespace Urban.ng.Data
{
    public interface IPropertyRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();
        Task<Photo> GetPhoto(int id);
        Task<Photo> GetMainPhotoForProperty(int id);
        Task<Video> GetVideo(int id);
        Task<Property> GetProperty(int id);
        Task<PagedList<Property>> GetMyProperties(int id, UserParams userParams);
        Task<PagedList<Property>> GetProperties(UserParams userParams);
        Task<Plan> GetPlan(int id);
        Task<Feature> GetFeature(int id);
    }
}

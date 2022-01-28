using RealEstates.Data;
using RealEstates.Models;
using RealEstates.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RealEstates.Services
{
    public class DistrictService : IDistrictService
    {
        private readonly RealEstatesDbContext db;

        public DistrictService(RealEstatesDbContext db)
        {
            this.db = db;
        }
        public IEnumerable<DistrictViewModel> GetTopDistrictsByAveragePrice(int count = 10)
        {
            return db.Districts
                .OrderByDescending(x => x.Properties.Average(x => x.Price))
                .Select(MapToDistrictViewModel())
                .Take(count)
                .ToList();
        }

        
        public IEnumerable<DistrictViewModel> GetTopDistrictsByNumberOfProperties(int count = 10)
        {
            return this.db.Districts
                  .OrderByDescending(x => x.Properties.Count())
                  .Select(MapToDistrictViewModel())
                  .Take(count)
                  .ToList();
        }
        private static Expression<Func<District, DistrictViewModel>> MapToDistrictViewModel()
        {
            return x => new DistrictViewModel
            {
                Name = x.Name,
                AveragePrice = (double)x.Properties.Average(x => x.Price),
                MaxPrice = (int)x.Properties.Max(x => x.Price),
                MinPrice = (int)x.Properties.Min(x => x.Price),
                PropertiesCount = x.Properties.Count(),
            };
        }

    }
}

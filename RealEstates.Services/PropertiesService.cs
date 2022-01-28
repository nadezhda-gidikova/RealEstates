using RealEstates.Data;
using RealEstates.Models;
using RealEstates.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstates.Services
{
    public class PropertiesService : IPropertiesService
    {
        private readonly RealEstatesDbContext db;

        public PropertiesService(RealEstatesDbContext db)
        {
            this.db = db;
        }
        public void Create(string district, int size,int?year, string propertyType, string buildingType, decimal price, int floor, int maxFloors)
        {
            if (district==null)
            {
                throw new ArgumentNullException(nameof(district));
            }
            var property = new RealEstateProperty
            {
                Size = size,
                Price = price,
                Year = year,
                Floor = floor,
                TotalNumberOfFloors=maxFloors,
            };

            if (property.Year<1800)
            {
                property.Year = null;
            }
            if (property.Floor<=0)
            {
                property.Floor = null;
            }
            if (property.TotalNumberOfFloors <= 0)
            {
                property.TotalNumberOfFloors = null;
            }

            var districtProperty = this.db.Districts.FirstOrDefault(x => x.Name.Trim() == district.Trim());
            if (districtProperty==null)
            {
                districtProperty = new District
                {
                    Name = district,
                };
            }
            property.District = districtProperty;
            //buildingType
            var buildingTypeEntity = this.db.BuildingTypes.FirstOrDefault(x => x.Name.Trim() == buildingType.Trim());
            if (buildingTypeEntity==null)
            {
                buildingTypeEntity = new BuildingType
                {
                    Name = buildingType,
                };
            }

            property.BuildingType = buildingTypeEntity;
            //propertyType
            var propertyTypeEntity = this.db.PropertyTypes.FirstOrDefault(x => x.Name.Trim() == propertyType.Trim());
            if (propertyTypeEntity == null)
            {
                propertyTypeEntity = new PropertyType
                {
                    Name = propertyType,
                };
            }
            property.PropertyType = propertyTypeEntity;

            this.db.Properties.Add(property);
            this.db.SaveChanges();

            this.UpdateTags(property.Id);
        }

        public IEnumerable<PropertyViewModel> Search(int minYear, int maxYear, int minSize, int maxSize)
        {
            return this.db.Properties.Where(x => x.Year >= minYear && x.Year <= maxYear
            && x.Size>=minSize && x.Size<=maxSize)
               .Select(x => new PropertyViewModel
               {
                   BuildingType = x.BuildingType.Name,
                   District = x.District.Name,
                   Floor = (x.Floor ?? 0).ToString() + "/" + (x.TotalNumberOfFloors ?? 0).ToString(),
                   Price = x.Price,
                   PropertyType = x.PropertyType.Name,
                   Size = x.Size,
                   Year = x.Year,
               })
               .OrderBy(x => x.Price)
               .ToList();
        }

        public IEnumerable<PropertyViewModel> SearchByPrice(int minPrice, int maxPrice)
        {
            return this.db.Properties.Where(x => x.Price >= minPrice && x.Price <= maxPrice)
                .Select(x=>new PropertyViewModel
                {
                    BuildingType=x.BuildingType.Name,
                    District=x.District.Name,
                    Floor=(x.Floor ?? 0)+"/"+ ( x.TotalNumberOfFloors ?? 0),
                    Price=x.Price,
                    PropertyType=x.PropertyType.Name,
                    Size=x.Size,
                    Year=x.Year,
                })
                .OrderBy(x=>x.Price)
                .ToList();

        }

        public void UpdateTags(int propertyId)
        {
            var property = this.db.Properties.FirstOrDefault(x => x.Id == propertyId);

                property.Tags.Clear();
            if (property.Year.HasValue && property.Year < 1990)
            {
                property.Tags.Add(new RealEstatePropertyTag { Tag = this.GetOrCreateTag("OldBuilding") });
            }
            if ( property.Size > 120)
            {
                property.Tags.Add(new RealEstatePropertyTag { Tag = this.GetOrCreateTag("HugeApartment") });
            }
            if (property.Year.HasValue && property.Year >2018 && property.TotalNumberOfFloors>5)
            {
                property.Tags.Add(new RealEstatePropertyTag { Tag = this.GetOrCreateTag("HasParking") });
            }
            if (property.Floor == property.TotalNumberOfFloors)
            {
                property.Tags.Add(new RealEstatePropertyTag { Tag = this.GetOrCreateTag("LastFloor") });
            }
            if (((double)property.Price/ property.Size) < 800)
            {
                property.Tags.Add(new RealEstatePropertyTag { Tag = this.GetOrCreateTag("CheapProperty") });
            }
            if (((double)property.Price / property.Size) > 2000)
            {
                property.Tags.Add(new RealEstatePropertyTag { Tag = this.GetOrCreateTag("ExpensiveProperty") });
            }
            this.db.SaveChanges();
        }

        private Tag GetOrCreateTag(string tag)
        {
            var tagEntity = this.db.Tags.FirstOrDefault(x => x.Name.Trim() == tag.Trim());
            if (tagEntity==null)
            {
                tagEntity = new Tag { Name = tag };
            }
            return tagEntity;
        }
    }
}

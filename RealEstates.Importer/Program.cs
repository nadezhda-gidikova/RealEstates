using RealEstates.Data;
using RealEstates.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace RealEstates.Importer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            var json = File.ReadAllText("imot.bg-raw-data-2022-01-27.json");

            var properties=JsonSerializer.Deserialize<IEnumerable<JsonProperty>>(json);
            var db = new RealEstatesDbContext();
            IPropertiesService propertyservice = new PropertiesService(db);
            foreach (var item in properties.Where(x=>x.Price >1000))
            {
                try
                {
                    propertyservice.Create(item.District,
                    item.Size,
                    item.Year,
                    item.Type,
                    item.BuildingType,
                    item.Price, item.Floor, item.TotalFloors);
                }
                catch 
                {
                }
            }

        }


    }
}

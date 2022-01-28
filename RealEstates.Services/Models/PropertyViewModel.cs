﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstates.Services.Models
{
    public class PropertyViewModel
    {
        public string District { get; set; }

        public string BuildingType { get; set; }

        public string PropertyType { get; set; }

        public decimal Price { get; set; }

        public int? Year { get; set; }

        public string Floor { get; set; }

        public int Size { get; set; }
    }
}

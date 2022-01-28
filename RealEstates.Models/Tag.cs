﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RealEstates.Models
{
    public class Tag
    {
        public Tag()
        {
            this.RealEstateProperties = new HashSet<RealEstatePropertyTag>();
        }
        public int Id { get; set; }
        [Required]
        public string  Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<RealEstatePropertyTag> RealEstateProperties { get; set; }
    }
}
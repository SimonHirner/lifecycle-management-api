using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace LifecycleManagementAPI.DataObjects
{
    public class Manufacturer
    {
        [Key]
        public int ManufacturerId { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public string Address { get; set; }

        [MaxLength(50)]
        public string TaxNumber  { get; set; }

        [MaxLength(50)]
        public string CompanyRegistrationNumber  { get; set; }

        public virtual List<Model> Models { get; set; }
    }
}
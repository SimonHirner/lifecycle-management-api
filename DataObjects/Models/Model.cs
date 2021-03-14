using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LifecycleManagementAPI.DataObjects
{
    public abstract class Model
    {
        [Key]
        public int ModelId { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string ModelName { get; set; }

        [MaxLength(50)]
        public string ModelNumber { get; set; }

        public DateTime LaunchYear { get; set; }

        public string Dimensions { get; set; }
        
        public virtual List<Device> Devices { get; set; }

        public int ManufacturerId { get; set; }

        [JsonIgnore]
        public virtual Manufacturer Manufacturer { get; set; }
    }
}
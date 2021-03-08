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
        [MinLength(1)]
        [MaxLength(100)]
        public string ModelName { get; set; }

        [MinLength(0)]
        [MaxLength(100)]
        public string ModelNumber { get; set; }
        
        public virtual List<Device> Devices { get; set; }

        public int ManufacturerId { get; set; }

        [JsonIgnore]
        public virtual Manufacturer Manufacturer { get; set; }
    }
}
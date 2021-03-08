using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace LifecycleManagementAPI.DataObjects
{
    public class Device
    {
        [Key]
        public int DeviceId { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string SerialNumber { get; set; }

        public DateTime WarrantyEnd { get; set; }

        [MinLength(0)]
        [MaxLength(500)]
        public string Notes { get; set; }
        
        public int ModelId { get; set; }

        [JsonIgnore]
        public virtual Model Model { get; set; }

        public int ActivityId { get; set; }

        [JsonIgnore]
        public virtual Activity Activity { get; set; }
    }
}
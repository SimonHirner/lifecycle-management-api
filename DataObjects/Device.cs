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
        [MinLength(5)]
        [MaxLength(50)]
        public string SerialNumber { get; set; }

        public DateTime WarrantyEnd { get; set; }

        [MaxLength(50)]
        public string CPU { get; set; }

        [MaxLength(50)]
        public string RAM { get; set; }

        [MaxLength(50)]
        public string Memory { get; set; }

        public string OperatingSystem { get; set; }
        
        public int ModelId { get; set; }

        [JsonIgnore]
        public virtual Model Model { get; set; }

        public int ActivityId { get; set; }

        [JsonIgnore]
        public virtual Activity Activity { get; set; }
    }
}
using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace LifecycleManagementAPI.DataObjects
{
    public abstract class Activity
    {
        [Key]
        public int ActivityId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public virtual List<Device> Devices { get; set; }

        public int EmployeeId { get; set; }

        [JsonIgnore]
        public virtual Employee Employee  { get; set; }
    }
}
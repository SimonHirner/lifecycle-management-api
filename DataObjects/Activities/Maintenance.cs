using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace LifecycleManagementAPI.DataObjects
{
    public class Maintenance : Activity
    {
        [Required]
        [MaxLength(50)]
        public string Issue { get; set; }

        public int Priority { get; set; }
    }
}
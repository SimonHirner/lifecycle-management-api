using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace LifecycleManagementAPI.DataObjects
{
    public class Maintenance : Activity
    {
        [Required]
        public string Issue { get; set; }
    }
}
using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace LifecycleManagementAPI.DataObjects
{
    public class Operation : Activity
    {
        [Required]
        [MaxLength(50)]
        public string Location { get; set; }

        [MaxLength(50)]
        public string Usage { get; set; }
    }
}
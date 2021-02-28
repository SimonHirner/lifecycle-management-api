using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace LifecycleManagementAPI.DataObjects
{
    public class Operation : Activity
    {
        [Required]
        public string Location { get; set; }

        [Required]
        public Employee User { get; set; }
    }
}
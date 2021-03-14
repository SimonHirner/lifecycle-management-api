using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace LifecycleManagementAPI.DataObjects
{
    public class Stock : Activity
    {
        [Required]
        [MaxLength(50)]
        public string Location { get; set; }
    }
}
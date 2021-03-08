using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace LifecycleManagementAPI.DataObjects
{
    public class Stock : Activity
    {
        [Required]
        public string Location { get; set; }
    }
}
using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace LifecycleManagementAPI.DataObjects
{
    public class Laptop : Model
    {
        public double DisplaySize { get; set; }

        public bool TouchScreen { get; set; }
    }
}
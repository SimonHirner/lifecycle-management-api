using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace LifecycleManagementAPI.DataObjects
{
    public class Phone : Model
    {
        public double DisplaySize { get; set; }

        public bool WaterResistant { get; set; }
    }
}
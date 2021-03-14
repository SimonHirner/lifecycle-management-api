using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace LifecycleManagementAPI.DataObjects
{
    public class Workstation : Model
    {
         public string MainboardFormFactor { get; set; }
    }
}
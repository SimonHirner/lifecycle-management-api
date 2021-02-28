using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace LifecycleManagementAPI.DataObjects
{
    public class Disposal : Activity
    {
        public bool ReadyForDisposal { get; set; }
    }
}
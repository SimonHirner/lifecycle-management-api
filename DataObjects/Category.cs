using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LifecycleManagementAPI.Controllers
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string Name { get; set; }
        public virtual List<Device> Devices { get; set; }
    }
}
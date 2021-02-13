using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace LifecycleManagementAPI.Controllers
{
    public class Device
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string Name { get; set; }
        public int CategoryId { get; set; }
        [JsonIgnore]
        public virtual Category Category { get; set; }
    }
}
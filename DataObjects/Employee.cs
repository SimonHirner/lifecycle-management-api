using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace LifecycleManagementAPI.DataObjects
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string Surname { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string Forname { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string Title  { get; set; }

        [Required]
        public DateTime BirthDate  { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string Department  { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string JobTitle  { get; set; }

        public virtual List<Activity> Activities { get; set; }

    }
}
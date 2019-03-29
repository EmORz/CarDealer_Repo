using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Castle.Components.DictionaryAdapter;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CarDealer.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public bool IsYoungDriver { get; set; }

        public ICollection<Sale> Sales { get; set; } = new List<Sale>();

        //is young driver (Young driver is a driver that has less than 2 years of experience.
        //Those customers get additional 5% off for the sale.)
    }
}
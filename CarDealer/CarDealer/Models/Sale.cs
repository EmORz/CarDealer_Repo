﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CarDealer.Models
{
    public class Sale
    {

        public Sale(Car car, Customer customer)
        {
            this.Car = car;
            this.Customer = customer;

        }
        public int Id { get; set; }

        public decimal Discount { get; set; }

        public int CarId { get; set; }
        public Car Car { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}

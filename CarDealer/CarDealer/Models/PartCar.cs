﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.Models
{
    public class PartCar
    {
        public PartCar(Part part, Car car)
        {
            this.Part = part;
            this.Car = car;
        }
        public int PartId { get; set; }
        public Part Part { get; set; }

        public int CarId { get; set; }
        public Car Car { get; set; }
    }
}

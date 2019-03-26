using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using AutoMapper;
using CarDealer.Data;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });

            var mapper = config.CreateMapper();

            var context = new CarDealerContext();

            //parts.json, cars.json, customers.json

            var path = $"E:\\CarDealer_Repo\\CarDealer\\CarDealer\\Datasets\\parts.json";
            var jsonString = File.ReadAllText(path);

            var desirealiedUsers = JsonConvert.DeserializeObject<Part[]>(jsonString);

            List<Part> parts = new List<Part>();


            foreach (var part in desirealiedUsers)
            {
                if (IsValid(part))
                {
                    parts.Add(part);
                }
            }

            context.Parts.AddRange(parts);
            context.SaveChanges();
        }


        public static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var result = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, result, true);
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using CarDealer.Data;
using CarDealer.Models;
using Castle.Core.Internal;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        private static string reportMessage = "Successfully imported {0}.";

        public static void Main(string[] args)
        {
            var context = new CarDealerContext();
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            var path = $"E:\\CarDealer_Repo\\01. Import Users_Car Dealer\\CarDealer\\Datasets\\sales.json";
            var userJson = File.ReadAllText(path);

            var result = GetSalesWithAppliedDiscount(context);

            Console.WriteLine(result); 
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            //Get first 10 sales with information about the car, customer and price of the sale with and without discount.
            var getSales =
                context
                    .Cars
                    
                    .Select(x => new
                    {
                        Make = x.Make,
                        Model = x.Model,
                        TravelledDistance = x.TravelledDistance,
                        customerName = x.Sales.Select(s => new
                        {
                            customerName = s.Customer.Name,
                            Discount = s.Discount,
                            Price = s.Car.PartCars.Select(d => new
                            {
                                price = d.Part.Quantity * d.Part.Price
                            })
                        })
                    })
                    .ToArray();
            ;

            var json = JsonConvert.SerializeObject(getSales, Formatting.Indented);
            return json;
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            //result list by total spent money descending then by total bought cars again in descending order. 
            var carParts =
                context
                    .Customers
                    .Where(x => x.Sales.Count>=1)
                    .Select(x => new
                    {
                        fullName = x.Name,
                        boughtCars = x.Sales.Count,
                        spentMoney = x.Sales.Select(a => a.Car.PartCars.Sum(z => z.Part.Price))

                    })
                    .OrderByDescending(x => x.boughtCars)
                    //.ThenByDescending(x => x.boughtCars)
                    .ToArray();
            ;

            var json = JsonConvert.SerializeObject(carParts, Formatting.Indented);
            return json;


        }


        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var carParts =
                context
                    .Cars
                    .Select(x => new
                    {
                        Make = x.Make,
                        Model = x.Model,
                        TravelledDistance = x.TravelledDistance,
                        parts = x.PartCars.Select(y => new
                        {
                            Name = y.Part.Name,
                            Price = y.Part.Price
                        })
                    })
                    //.OrderBy(x => x.Model)
                    //.ThenByDescending(x => x.TravelledDistance)
                    .ToArray();
            ;

            var json = JsonConvert.SerializeObject(carParts, Formatting.Indented);
            return json;

        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var supplers =
                context
                    .Suppliers
                    .Where(x => x.IsImporter == false)
                    .Select(x => new
                    {
                        Id = x.Id,
                        Name = x.Name,
                        PartsCount =  x.Parts.Count
                    })
                    //.OrderBy(x => x.Model)
                    //.ThenByDescending(x => x.TravelledDistance)
                    .ToArray();
            ;

            var json = JsonConvert.SerializeObject(supplers, Formatting.Indented);
            return json;
        }

        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var customers = 
                context
                .Cars
                .Where(x => x.Make.Equals("Toyota"))
                .Select(x => new
                {
                    Id = x.Id,
                    Make = x.Make,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance
                })
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TravelledDistance)
                .ToArray();
            ;

            var json = JsonConvert.SerializeObject(customers, Formatting.Indented);
            return json;

        }

        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context.Customers
                .Select(x => new
                {
                    Name = x.Name,
                    BirthDate = x.BirthDate.ToString("dd/MM/yyyy"),
                    IsYoungDriver = x.IsYoungDriver
                })
                .OrderBy(x => x.BirthDate)
                .ToArray();

            ;



            //    .Where(x => x.Price >= 500 && x.Price <= 1000)
            //    .Select(x => new ProductDto
            //    {
            //        Name = x.Name,
            //        Price = x.Price,
            //        Seller = $"{x.Seller.FirstName} {x.Seller.LastName}"
            //    })
            //    .OrderBy(x => x.Price)
            //    .ToList();
            var json = JsonConvert.SerializeObject(customers, Formatting.Indented);
            return json;

        }

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            var sales = JsonConvert.DeserializeObject<Sale[]>(inputJson);

            var validSales= new List<Sale>();

            ;


            foreach (var sale in sales)
            {
                validSales.Add(sale);
            }

            ;
            context.Sales.AddRange(validSales);
            var numRecords = context.SaveChanges();

            return string.Format(reportMessage, numRecords);
        }


        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var customers = JsonConvert.DeserializeObject<Customer[]>(inputJson);

            var validCustomers = new List<Customer>();

            foreach (var customer in customers)
            {
                validCustomers.Add(customer);
            }

            ;
            context.Customers.AddRange(validCustomers);
            var numRecords = context.SaveChanges();

            return string.Format(reportMessage, numRecords);
        }

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            //todo 50/100 refactor

            var cars = JsonConvert.DeserializeObject<Car[]>(inputJson);

            var validCars = new List<Car>();

            foreach (var car in cars)
            {
                validCars.Add(car);
            }

            ;
            context.Cars.AddRange(validCars);
            context.SaveChanges();

            return string.Format(reportMessage, validCars.Count);


        }

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            //todo 50/100 refactor

            var parts = JsonConvert.DeserializeObject<Part[]>(inputJson);

            var validParts = new List<Part>();

            foreach (var part in parts)
            {
                validParts.Add(part);
            }

            ;
            context.Parts.AddRange(validParts);
            context.SaveChanges();

            return string.Format(reportMessage, validParts.Count);
        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var suppliers = JsonConvert.DeserializeObject<Supplier[]>(inputJson)
                .Where(x => x.Name != null)
                .ToArray();

            var validSuppliers= new List<Supplier>();

            context.Suppliers.AddRange(suppliers);
            var records = context.SaveChanges();

            return string.Format(reportMessage, records);
        }
    }
}
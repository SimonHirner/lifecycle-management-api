using System;
using LifecycleManagementAPI.DataObjects;

namespace LifecycleManagementAPI
{
    public static class TestDataInitializer
    {

        public static void InitializeTestData(Context context)
        {

            //Initialize test data
            Manufacturer testManufacturer1 = new Manufacturer() 
            {
                Name = "Apple",
                Address = "California",
                CompanyRegistrationNumber = "12134",
                TaxNumber = "AC01249837450E"
            };

            Manufacturer testManufacturer2 = new Manufacturer() 
            {
                Name = "Hewlett-Packard",
                Address = "California",
                CompanyRegistrationNumber = "23287",
                TaxNumber = "DC213448582532"
            };

            Employee testEmployee1 = new Employee() 
            {
                Forname = "Max",
                Surname = "Musterman",
                BirthDate = new DateTime(1987, 3, 3),
                JobTitle = "1st Level Support",
                Department = "IT",
                Title = "Mr"
            };

            Employee testEmployee2 = new Employee() 
            {
                Forname = "Thomas",
                Surname = "Müller",
                BirthDate = new DateTime(1987, 3, 3),
                JobTitle = "Controller",
                Department = "Controlling",
                Title = "Mr"
            };

            Laptop testLaptop1 = new Laptop()
            {
                ModelName = "MacBook Pro",
                ModelNumber = "MBP2020/10",
                DisplaySize = 16.0,
                TouchScreen = false,
                ManufacturerId = 1
            };

            Server testServer = new Server()
            {
                ModelName = "HP ProLiant DL360",
                ModelNumber = "DL360e",
                FormFactor = "19\" Rack",
                ManufacturerId = 2
            };

            Laptop testLaptop2 = new Laptop()
            {
                ModelName = "MacBook Air",
                ModelNumber = "XCS2018/04",
                DisplaySize = 14.4,
                TouchScreen = false,
                ManufacturerId = 1
            };

            Maintenance testMaintenance = new Maintenance()
            {
                StartDate = new DateTime(),
                Issue = "Kaputter Bildschirm",
                EmployeeId = 1
            };

            Operation testOperation = new Operation()
            {
                StartDate = new DateTime(),
                Location = "Raum 01.234",
                Usage = "Mail-Server",
                EmployeeId = 1
            };

            Stock testStock = new Stock()
            {
                StartDate = new DateTime(),
                Location = "Lager 23",
                EmployeeId = 1
            };

            Device testDevice1 = new Device()
            {
                SerialNumber = "ZDX168918502",
                WarrantyEnd = new DateTime(2023, 5, 1),
                CPU = "Intel i7",
                OperatingSystem = "MacOS",
                ModelId = 1,
                ActivityId = 1
            };

            Device testDevice2 = new Device()
            {
                SerialNumber = "ZDX346963481",
                WarrantyEnd = new DateTime(2022, 6, 7),
                CPU = "Intel i5",
                OperatingSystem = "MacOS",
                ModelId = 1,
                ActivityId = 3
            };

            Device testDevice3 = new Device()
            {
                SerialNumber = "ALX81241200894",
                WarrantyEnd = new DateTime(2025, 1, 1),
                CPU = "Intel Xeon",
                OperatingSystem = "Linux",
                ModelId = 2,
                ActivityId = 2
            };

            context.Manufacturers.Add(testManufacturer1);

            context.Manufacturers.Add(testManufacturer2);

            context.Employees.Add(testEmployee1);

            context.Employees.Add(testEmployee2);

            context.Models.Add(testLaptop1);

            context.Models.Add(testLaptop2);

            context.Models.Add(testServer);

            context.Devices.Add(testDevice1);

            context.Devices.Add(testDevice2);

            context.Devices.Add(testDevice3);

            context.Activities.Add(testMaintenance);

            context.Activities.Add(testStock);

            context.Activities.Add(testOperation);

            //save these data into the in-memory database
            context.SaveChanges();

        }
    }
}
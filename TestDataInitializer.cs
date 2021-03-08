using System;
using LifecycleManagementAPI.DataObjects;

namespace LifecycleManagementAPI
{
    public static class TestDataInitializer
    {

        public static void InitializeTestData(Context context)
        {

            //Initialize test data
            Manufacturer testManufacturer = new Manufacturer() 
            {
                Name = "Apple",
                Address = "California",
                CompanyRegistrationNumber = "12134",
                TaxNumber = "AC01249837450E"
            };

            Employee testEmployee = new Employee() 
            {
                Forname = "Max",
                Surname = "Musterman",
                BirthDate = new DateTime(1987, 3, 3),
                JobTitle = "1st Level Support",
                Department = "IT",
                Title = "Mr"
            };

            Laptop testModel = new Laptop()
            {
                ModelName = "MacBook Pro",
                ModelNumber = "MBP2020/10",
                ManufacturerId = 1
            };

            Maintenance testActivity = new Maintenance()
            {
                ActivityDate = new DateTime(),
                Issue = "Broken screen",
                EmployeeId = 1
            };

            Device testDevice = new Device()
            {
                SerialNumber = "ZDX168918502",
                WarrantyEnd = new DateTime(2023, 5, 1),
                ModelId = 1,
                ActivityId = 1
            };

            context.Manufacturers.Add(testManufacturer);

            context.Employees.Add(testEmployee);

            context.Models.Add(testModel);

            context.Devices.Add(testDevice);

            context.Activities.Add(testActivity);

            context.Laptops.Add(testModel);

            context.Maintenances.Add(testActivity);

            //save these data into the in-memory database
            context.SaveChanges();

        }
    }
}
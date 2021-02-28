using System;
using LifecycleManagementAPI.DataObjects;

namespace LifecycleManagementAPI
{
    public static class TestDataInitializer
    {

        public static void InitializeTestData(Context context)
        {

            //Initialize test category
            Model testCategory = new Laptop()
            {
                ModelName = "MacBook Pro",
                Manufacturer = "Apple"
            };

            Device testDevice = new Device()
            {
                SerialNumber = "ZDX168918502",
                ModelId = 1
            };

            context.Categories.Add(testCategory);

            context.Devices.Add(testDevice);

            //save these data into the in-memory database
            context.SaveChanges();

        }
    }
}
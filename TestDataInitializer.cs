using System;
using LifecycleManagementAPI.Controllers;

namespace LifecycleManagementAPI
{
    public static class TestDataInitializer
    {

        public static void InitializeTestData(Context context)
        {

            //Initialize test category
            Category testCategory = new Category()
            {
                Name = "Notebook"
            };

            Device testDevice = new Device()
            {
                Name = "MacBook Pro",
                CategoryId = 1
            };

            context.Categories.Add(testCategory);

            context.Devices.Add(testDevice);

            //save these data into the in-memory database
            context.SaveChanges();

        }
    }
}
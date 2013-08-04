using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Grain.Tests.Models.TestModels
{
    public static class TestModelInstances
    {
        public static TestModel SimpleModel = new TestModel { Name = "Foo", Description = "Foo Bar", Id = 1, TimeCreated = DateTime.Parse("2012-12-04T14:50:06.4559307-05:00") };
        
        static TestModelWithCollections _collectionsModel;
        public static TestModelWithCollections ModelWithCollections 
        {
            get 
            {
                _collectionsModel = new TestModelWithCollections
                {
                    Name = "Hello",
                    Description = "Hello Bar",
                    Id = 2,
                    TimeCreated = DateTime.Parse("2012-12-04T14:50:06.4559307-05:00"),
                    StringCollection = new List<string> { "Foo", "Bar" },
                    Dictionary = new Dictionary<string, string> { },
                    Children = new List<TestModel> { 
                    new TestModel { Name = "Foo", Description = "Foo Bar", Id = 3, TimeCreated = DateTime.Parse("2012-12-04T14:50:06.4559307-05:00") }
                }
                };

                _collectionsModel.Dictionary.Add("Foo", "Bar");
                _collectionsModel.Dictionary.Add("Hello", "World");

                return _collectionsModel;
            }
        }
    }
}

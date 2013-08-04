using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grain.Tests.Models.TestModels
{
    public partial class TestModel
    {
        public TestModel() { }
        public TestModel(int id, string name, string description) 
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public int Id { get; set; }
        public DateTime TimeCreated { get; set; }
    }
}

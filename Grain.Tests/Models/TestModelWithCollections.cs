using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grain.Tests.Models.TestModels
{
    public class TestModelWithCollections : TestModel
    {
        public ICollection<string> StringCollection { get; set; }
        public IDictionary<string, string> Dictionary { get; set; }
        public ICollection<TestModel> Children { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace Grain.Tests.Configuration
{
    public partial class TestSection : ConfigurationSection
    {
        public TestSection()
        {
            TestValue = "helloWorld";
        }

        [ConfigurationProperty("testValue", DefaultValue = "helloWorld")]
        [StringValidator(MinLength = 1)]
        public string TestValue
        {
            get { return base["testValue"] as string; }
            set { base["testValue"] = value; }
        }
    }
}

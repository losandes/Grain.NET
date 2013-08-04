using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Grain.DataAccess.Sql;


namespace Grain.Tests.Models.TestModels
{
    public class DbTestModel
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }

    public class DbTestModelFoo
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Foo { get; set; }
        public DbTestModel TestModel { get; set; }
    }

    public static class TestModelExtensions
    {
        public static Func<IDataRecord, DbTestModel> TestModelFactory()
        {
            return r => new DbTestModel
            {
                Id = r.GetValueOrDefault<int>("Id"),
                Guid = r.GetValueOrDefault<Guid>("Guid"),
                Name = r.GetValueOrDefault<string>("Name"),
                Date = r.GetValueOrDefault<DateTime>("Date")
            };
        }

        public static Func<IDataRecord, DbTestModel> TestModelFactory_UsingOrdinals()
        {
            return r => new DbTestModel
            {
                Id = r.GetValueOrDefault<int>(0),
                Guid = r.GetValueOrDefault<Guid>(1),
                Name = r.GetValueOrDefault<string>(2),
                Date = r.GetValueOrDefault<DateTime>(3)
            };
        }

        public static Func<IDataRecord, DbTestModelFoo> TestModelFooFactory()
        {
            return r => new DbTestModelFoo
            {
                Id = r.GetValueOrDefault<int>("Id"),
                Guid = r.GetValueOrDefault<Guid>("Guid"),
                Foo = r.GetValueOrDefault<string>("Foo"),
                TestModel = new DbTestModel
                {
                    Id = r.GetValueOrDefault<int>("TestModelId"),
                    Guid = r.GetValueOrDefault<Guid>("TestModelGuid"),
                    Name = r.GetValueOrDefault<string>("TestModelName"),
                    Date = r.GetValueOrDefault<DateTime>("TestModelDate")
                }
            };
        }
    }
}

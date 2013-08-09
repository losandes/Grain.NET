using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Grain.Configuration;
using Grain.DataAccess.Sql;
using Grain.Tests.Models.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grain.Tests.DataAccess.Sql
{
    [TestClass]
    public class DbInstanceTests
    {
        #region Setup

        string _command1 = @"
declare @mockTestData xml = '
<TestModels>	
	<TestModel>	
		<Id>1</Id>
		<Guid>45576EAE-8206-46C6-97E1-80614349463B</Guid>
		<Name>Foo</Name>
		<Date>05/12/2013</Date>
	</TestModel>
	<TestModel>	
		<Id>2</Id>
		<Guid>81C795EA-D7AD-4B70-91B0-B9809E693A07</Guid>
		<Name>Bar</Name>
		<Date>05/13/2013</Date>
	</TestModel>
</TestModels>'
	select 
			Foos.Foo.value('Id[1]', 'int') as Id,
			Foos.Foo.value('Guid[1]', 'uniqueidentifier') as [Guid],
			Foos.Foo.value('Name[1]', 'nvarchar(max)') as Name,
			Foos.Foo.value('Date[1]', 'datetime') as [Date]
		from @mockTestData.nodes(N'//TestModel') Foos(Foo)";

        string _command2 = @"
declare @mockFooData xml = '
<TestModels>	
	<TestModelFoo>	
		<Id>1</Id>
		<Guid>1F48DD3B-E3A7-4CBD-AE7C-DA9A00632049</Guid>
		<Name>Hello</Name>
		<TestModel>	
			<Id>1</Id>
			<Guid>45576EAE-8206-46C6-97E1-80614349463B</Guid>
			<Name>Foo</Name>
			<Date>05/12/2013</Date>
		</TestModel>
	</TestModelFoo>
	<TestModelFoo>	
		<Id>2</Id>
		<Guid>38A0F824-F555-45B7-8DDC-B7FC9C1D3067</Guid>
		<Name>World</Name>
		<TestModel>	
			<Id>2</Id>
			<Guid>81C795EA-D7AD-4B70-91B0-B9809E693A07</Guid>
			<Name>Bar</Name>
			<Date>05/13/2013</Date>
		</TestModel>
	</TestModelFoo>
</TestModels>'
	select 
			Foos.Foo.value('Id[1]', 'int') as Id,
			Foos.Foo.value('Guid[1]', 'uniqueidentifier') as [Guid],
			Foos.Foo.value('Name[1]', 'nvarchar(max)') as Foo,
			Foos.Foo.value('TestModel[1]/Id[1]', 'int') as TestModelId,
			Foos.Foo.value('TestModel[1]/Guid[1]', 'uniqueidentifier') as [TestModelGuid],
			Foos.Foo.value('TestModel[1]/Name[1]', 'nvarchar(max)') as TestModelName,			
			Foos.Foo.value('TestModel[1]/Date[1]', 'datetime') as [TestModelDate]
		from @mockFooData.nodes(N'//TestModelFoo') Foos(Foo)";

        #endregion Setup

        #region Validation Methods

        private void ValidateCommand1(IEnumerable<DbTestModel> testModels)
        {
            var _first = testModels.First();
            Assert.AreEqual(_first.Id, 1);
            Assert.AreEqual(_first.Name, "Foo");
            Assert.AreEqual(_first.Guid, new Guid("45576EAE-8206-46C6-97E1-80614349463B"));
            Assert.AreEqual(_first.Date, new DateTime(2013, 05, 12));

            var _second = testModels.First(r => r.Id == 2);
            Assert.AreEqual(_second.Name, "Bar");
            Assert.AreEqual(_second.Guid, new Guid("81C795EA-D7AD-4B70-91B0-B9809E693A07"));
            Assert.AreEqual(_second.Date, new DateTime(2013, 05, 13));
        }

        private void ValidateCommand2(IEnumerable<DbTestModelFoo> testModels)
        {
            var _first = testModels.First();
            Assert.AreEqual(_first.Id, 1);
            Assert.AreEqual(_first.Guid, new Guid("1F48DD3B-E3A7-4CBD-AE7C-DA9A00632049"));
            Assert.AreEqual(_first.Foo, "Hello");
            Assert.AreEqual(_first.TestModel.Id, 1);
            Assert.AreEqual(_first.TestModel.Guid, new Guid("45576EAE-8206-46C6-97E1-80614349463B"));
            Assert.AreEqual(_first.TestModel.Name, "Foo");
            Assert.AreEqual(_first.TestModel.Date, new DateTime(2013, 05, 12));

            var _second = testModels.First(r => r.Id == 2);
            Assert.AreEqual(_second.Guid, new Guid("38A0F824-F555-45B7-8DDC-B7FC9C1D3067"));
            Assert.AreEqual(_second.Foo, "World");
            Assert.AreEqual(_second.TestModel.Id, 2);
            Assert.AreEqual(_second.TestModel.Guid, new Guid("81C795EA-D7AD-4B70-91B0-B9809E693A07"));
            Assert.AreEqual(_second.TestModel.Name, "Bar");
            Assert.AreEqual(_second.TestModel.Date, new DateTime(2013, 05, 13));
        }

        #endregion Validation Methods

        [TestMethod]
        [Owner("Andy")]
        [TestProperty("Data", "Execute")]
        [TestCategory("Touches Database")]
        [TestCategory("Grain.DataAccess")]
        public void ExecuteAsSingleTest()
        {
            using (DbInstance db = new DbInstance(ConfigManager.GetConnectionString("UnitTests")))
            {
                var _result = db.ExecuteAsSingle<DbTestModel>(new SqlCommand(_command1), TestModelExtensions.TestModelFactory());
                Assert.AreEqual(_result.Id, 1);
                Assert.AreEqual(_result.Name, "Foo");
                Assert.AreEqual(_result.Guid, new Guid("45576EAE-8206-46C6-97E1-80614349463B"));
                Assert.AreEqual(_result.Date, new DateTime(2013, 05, 12));
            }
        }

        [TestMethod]
        [Owner("Andy")]
        [TestProperty("Data", "Execute")]
        [TestCategory("Touches Database")]
        [TestCategory("Grain.DataAccess")]
        public void ExecuteAsTest()
        {
            using (DbInstance db = new DbInstance(ConfigManager.GetConnectionString("UnitTests")))
            {
                var _result = db.ExecuteAs<DbTestModel>(new SqlCommand(_command1), TestModelExtensions.TestModelFactory());
                ValidateCommand1(_result);
            }
        }

        [TestMethod]
        [Owner("Andy")]
        [TestProperty("Data", "Execute")]
        [TestCategory("Touches Database")]
        [TestCategory("Grain.DataAccess")]
        public void ExecuteAsTestUsingOrdinals()
        {
            using (DbInstance db = new DbInstance(ConfigManager.GetConnectionString("UnitTests")))
            {
                var _result = db.ExecuteAs<DbTestModel>(new SqlCommand(_command1), TestModelExtensions.TestModelFactory_UsingOrdinals());
                ValidateCommand1(_result);
            }
        }

        [TestMethod]
        [Owner("Andy")]
        [TestProperty("Data", "Execute")]
        [TestCategory("Touches Database")]
        [TestCategory("Grain.DataAccess")]
        public void ExecuteAsManyTest()
        {
            using (DbInstance db = new DbInstance(ConfigManager.GetConnectionString("UnitTests")))
            {
                List<DbTestModel> _models = new List<DbTestModel> { };
                List<DbTestModelFoo> _foos = new List<DbTestModelFoo> { };
                var _command = new SqlCommand(_command1 + _command2);

                // Test two result sets
                db.ExecuteAs<DbTestModel, DbTestModelFoo>(_command,
                    TestModelExtensions.TestModelFactory(), _models,
                    TestModelExtensions.TestModelFooFactory(), _foos);

                ValidateCommand1(_models);
                ValidateCommand2(_foos);

                // Test two result sets, using named parameters
                db.ExecuteAs<DbTestModel, DbTestModelFoo>(_command,
                    modelBinder1: TestModelExtensions.TestModelFactory(), output1: _models,
                    modelBinder2: TestModelExtensions.TestModelFooFactory(), output2: _foos);

                ValidateCommand1(_models);
                ValidateCommand2(_foos);

                // Test two result sets, using the six-result set overload
                db.ExecuteAs<DbTestModel, DbTestModelFoo, object, object, object, object>(_command,
                    modelBinder1: TestModelExtensions.TestModelFactory(), output1: _models,
                    modelBinder2: TestModelExtensions.TestModelFooFactory(), output2: _foos);
                
                ValidateCommand1(_models);
                ValidateCommand2(_foos);
            }
        }
    }
}

using Grain.DataAccess.Sql;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Grain.Tests.DataAccess.Sql
{
    [TestClass]
    public class SqlCommandFactoryTests
    {
        SqlCommandFactory factory;

        public SqlCommandFactoryTests()
        {
            factory = new SqlCommandFactory();
        }

        [TestMethod]
        public void SqlCommandFactory_MakeProcCommand_should_make_SqlCommand_with_proc_type() 
        {   
            // when
            var _actual = factory.MakeProcCommand("dbo.myStoredProc");

            // then
            Assert.IsInstanceOfType(_actual, typeof(SqlCommand));
            Assert.IsTrue(_actual.CommandType == System.Data.CommandType.StoredProcedure);
        }

        [TestMethod]
        public void SqlCommandFactory_MakeProcCommand_should_make_SqlCommand_with_SqlParameters() 
        { 
            // when
            var _actual = factory.MakeProcCommand("dbo.myStoredProc",
                new TupleList<string, SqlDbType, object> { 
                    { "Id", SqlDbType.Int, 1 },
                    { "Name", SqlDbType.NVarChar, "Foo" }
                });

            // then
            Assert.IsInstanceOfType(_actual, typeof(SqlCommand));
            Assert.IsTrue(_actual.CommandType == System.Data.CommandType.StoredProcedure);
            Assert.IsTrue(_actual.Parameters.Contains("Id"));
            Assert.IsTrue(_actual.Parameters.Contains("Name"));
        }
    }
}

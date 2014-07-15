using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Grain.DataAccess.Sql
{
    public class SqlCommandFactory : ISqlCommandFactory
    {
        public SqlCommand MakeProcCommand(string proc) 
        {
            var _command = new SqlCommand(proc);
            _command.CommandType = System.Data.CommandType.StoredProcedure;

            return _command;
        }

        public SqlCommand MakeProcCommand(string proc, TupleList<string, SqlDbType, object> parameters) 
        {
            var _command = MakeProcCommand(proc);

            foreach (var param in parameters) 
            {
                _command.Parameters.Add(MakeParam(param.Item1, param.Item2, param.Item3));
            }

            return _command;
        }

        /// <summary>
        /// Make a SqlParameter with the values that are provided; convert null values to DBNull.Value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private SqlParameter MakeParam(string name, SqlDbType type, object value) 
        {
            if (value != null)
                return new SqlParameter { ParameterName = name, SqlDbType = type, Value = value };
            else 
                return new SqlParameter { ParameterName = name, SqlDbType = type, Value = DBNull.Value };
        }
    }
}

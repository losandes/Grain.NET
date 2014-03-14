using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Grain.Attributes;

namespace Grain.DataAccess.Sql
{
    public class DbInstance : IDisposable
    {
        public DbInstance(string connectionString)
        {
            _dbConnectionString = connectionString;
        }

        public DbInstance(string connectionString, int defaultCommandTimeout)
        {
            _dbConnectionString = connectionString;
            _defaultCommandTimeout = defaultCommandTimeout;
        }

        string _dbConnectionString;
        int _defaultCommandTimeout;

        SqlConnection _sqlConnection;
        public SqlConnection SqlConnection
        {
            get
            {
                if (_sqlConnection == null)
                {
                    _sqlConnection = new SqlConnection(_dbConnectionString);
                    _sqlConnection.Open();
                }
                else if (_sqlConnection != null && _sqlConnection.State == ConnectionState.Closed) 
                {
                    _sqlConnection.Open();
                }
                return _sqlConnection;
            }
        }

        /// <summary>
        /// Executes a non-query Transact-SQL statement and returns the number of rows that were affected (assuming NOCOUNT is not on)
        /// </summary>
        /// <param name="command">SqlCommand: the command to be executed</param>
        /// <returns>int: if NOCOUNT is on, on the database, the number of rows that were affected</returns>
        public virtual int Execute(SqlCommand command)
        {
            using (command)
            {
                command.Connection = SqlConnection;
                command.CommandTimeout = _defaultCommandTimeout;
                return command.ExecuteNonQuery();
            } // end using command
        }

        /// <summary>
        /// Executes a SqlCommand and returns a single result via a factory pattern (selector param).  If more than one result 
        /// was present in the SQL execution, all but the first are omitted.
        /// </summary>
        /// <typeparam name="T">The type of output object</typeparam>
        /// <param name="command">SqlCommand: the command to execute</param>
        /// <param name="modelBinder">Func of type IDataRecord and T: the factory for binding the output to a model</param>
        /// <returns>IEnumerable of type T: the result set bound to a model, by way of the selector factory.</returns>
        public virtual T ExecuteAsSingle<T>(SqlCommand command, Func<IDataRecord, T> modelBinder, CommandBehavior commandBehavior = CommandBehavior.CloseConnection)
        {
            var _result = ExecuteAs<T>(command, modelBinder, commandBehavior: commandBehavior);

            if (_result != null)
            {
                // cast the IEnumerable to a list to avoid executing the query multiple times
                List<T> _list = _result.ToList();
                if (_list.Any())
                    return _list.First();
            }

            return default(T);
        }

        /// <summary>
        /// Executes a SqlCommand and returns the results via a factory pattern (selector param)
        /// </summary>
        /// <typeparam name="T">The type of output object</typeparam>
        /// <param name="command">SqlCommand: the command to execute</param>
        /// <param name="modelBinder">Func of type IDataRecord and T: the factory for binding the output to a model</param>
        /// <returns>IEnumerable of type T: the result set bound to a model, by way of the selector factory.</returns>
        public virtual IEnumerable<T> ExecuteAs<T>(SqlCommand command, Func<IDataRecord, T> modelBinder, CommandBehavior commandBehavior = CommandBehavior.CloseConnection)
        {
            using (command)
            {
                command.Connection = SqlConnection;
                command.CommandTimeout = _defaultCommandTimeout;

                using (SqlDataReader reader = command.ExecuteReader(commandBehavior))
                {
                    while (reader.Read())
                        yield return modelBinder(reader);
                } // end using reader
            } // end using command
        }
        // example:
        //SqlCommand _command = new SqlCommand("dbo.MyProc");
        //_command.CommandType = CommandType.StoredProcedure;
        //_command.Parameters.Add(new SqlParameter { ParameterName = "userId", SqlDbType = SqlDbType.Int, Value = 42 });
        //ExecuteAs<MyModel>(_command, m => new MyModel { 
        //     Id = m.GetValueOrDefault<int>("Id"),
        //     Name = m.GetValueOrDefault<string>("FullName")
        //     ...
        //});

        #region Multiple Result Sets

        /// <summary>
        /// Executes a SqlCommand that expects four result sets and binds the results to the given models
        /// </summary>
        /// <typeparam name="T1">Type: the type of object for the first result set</typeparam>
        /// <typeparam name="T2">Type: the type of object for the second result set</typeparam>
        /// <param name="command">SqlCommand: a stored procedure command</param>
        /// <param name="modelBinder1">Func of type IDataRecord and T: the factory for binding the output to a model</param>
        /// <param name="output1">A list to store the output from the first result set (treat as ref: must be initialized)</param>
        /// <param name="modelBinder2">Func of type IDataRecord and T: the factory for binding the output to a model</param>
        /// <param name="output2">A list to store the output from the second result set (treat as ref: must be initialized)</param>
        /// <param name="commandBehavior">CommandBehavior: a description of the results of the query and its effect on the database (our default is CloseConnection)</param>
        /// <returns>List of Type T: the results in a collection</returns>
        public virtual void ExecuteAs<T1, T2>(SqlCommand command,
            Func<IDataRecord, T1> modelBinder1, List<T1> output1,
            Func<IDataRecord, T2> modelBinder2, List<T2> output2,
            CommandBehavior commandBehavior = CommandBehavior.CloseConnection)
        {
            ExecuteAs<T1, T2, object, object, object, object>(command,
                modelBinder1: modelBinder1, output1: output1,
                modelBinder2: modelBinder2, output2: output2,
                commandBehavior: commandBehavior);
        }

        /// <summary>
        /// Executes a SqlCommand that expects four result sets and binds the results to the given models
        /// </summary>
        /// <typeparam name="T1">Type: the type of object for the first result set</typeparam>
        /// <typeparam name="T2">Type: the type of object for the second result set</typeparam>
        /// <typeparam name="T3">Type: the type of object for the third result set</typeparam>
        /// <param name="command">SqlCommand: a stored procedure command</param>
        /// <param name="modelBinder1">Func of type IDataRecord and T: the factory for binding the output to a model</param>
        /// <param name="output1">A list to store the output from the first result set (treat as ref: must be initialized)</param>
        /// <param name="modelBinder2">Func of type IDataRecord and T: the factory for binding the output to a model</param>
        /// <param name="output2">A list to store the output from the second result set (treat as ref: must be initialized)</param>
        /// <param name="modelBinder3">Func of type IDataRecord and T: the factory for binding the output to a model</param>
        /// <param name="output3">A list to store the output from the third result set (treat as ref: must be initialized)</param>
        /// <param name="commandBehavior">CommandBehavior: a description of the results of the query and its effect on the database (our default is CloseConnection)</param>
        /// <returns>List of Type T: the results in a collection</returns>
        public virtual void ExecuteAs<T1, T2, T3>(SqlCommand command,
            Func<IDataRecord, T1> modelBinder1, List<T1> output1,
            Func<IDataRecord, T2> modelBinder2, List<T2> output2,
            Func<IDataRecord, T3> modelBinder3, List<T3> output3,
            CommandBehavior commandBehavior = CommandBehavior.CloseConnection)
        {
            ExecuteAs<T1, T2, T3, object, object, object>(command,
                modelBinder1: modelBinder1, output1: output1,
                modelBinder2: modelBinder2, output2: output2,
                modelBinder3: modelBinder3, output3: output3,
                commandBehavior: commandBehavior);
        }

        /// <summary>
        /// Executes a SqlCommand that expects four result sets and binds the results to the given models
        /// </summary>
        /// <typeparam name="T1">Type: the type of object for the first result set</typeparam>
        /// <typeparam name="T2">Type: the type of object for the second result set</typeparam>
        /// <typeparam name="T3">Type: the type of object for the third result set</typeparam>
        /// <typeparam name="T4">Type: the type of object for the fourth result set</typeparam>
        /// <param name="command">SqlCommand: a stored procedure command</param>
        /// <param name="modelBinder1">Func of type IDataRecord and T: the factory for binding the output to a model</param>
        /// <param name="output1">A list to store the output from the first result set (treat as ref: must be initialized)</param>
        /// <param name="modelBinder2">Func of type IDataRecord and T: the factory for binding the output to a model</param>
        /// <param name="output2">A list to store the output from the second result set (treat as ref: must be initialized)</param>
        /// <param name="modelBinder3">Func of type IDataRecord and T: the factory for binding the output to a model</param>
        /// <param name="output3">A list to store the output from the third result set (treat as ref: must be initialized)</param>
        /// <param name="modelBinder4">Func of type IDataRecord and T: the factory for binding the output to a model</param>
        /// <param name="output4">A list to store the output from the fourth result set (treat as ref: must be initialized)</param>
        /// <param name="commandBehavior">CommandBehavior: a description of the results of the query and its effect on the database (our default is CloseConnection)</param>
        /// <returns>List of Type T: the results in a collection</returns>
        public virtual void ExecuteAs<T1, T2, T3, T4>(SqlCommand command,
            Func<IDataRecord, T1> modelBinder1, List<T1> output1,
            Func<IDataRecord, T2> modelBinder2, List<T2> output2,
            Func<IDataRecord, T3> modelBinder3, List<T3> output3,
            Func<IDataRecord, T4> modelBinder4, List<T4> output4,
            CommandBehavior commandBehavior = CommandBehavior.CloseConnection)
        {
            ExecuteAs<T1, T2, T3, T4, object, object>(command,
                modelBinder1: modelBinder1, output1: output1,
                modelBinder2: modelBinder2, output2: output2,
                modelBinder3: modelBinder3, output3: output3,
                modelBinder4: modelBinder4, output4: output4,
                commandBehavior: commandBehavior);
        }

        /// <summary>
        /// Executes a SqlCommand that expects four result sets and binds the results to the given models
        /// </summary>
        /// <typeparam name="T1">Type: the type of object for the first result set</typeparam>
        /// <typeparam name="T2">Type: the type of object for the second result set</typeparam>
        /// <typeparam name="T3">Type: the type of object for the third result set</typeparam>
        /// <typeparam name="T4">Type: the type of object for the fourth result set</typeparam>
        /// <typeparam name="T5">Type: the type of object for the fourth result set</typeparam>
        /// <param name="command">SqlCommand: a stored procedure command</param>
        /// <param name="modelBinder1">Func of type IDataRecord and T: the factory for binding the output to a model</param>
        /// <param name="output1">A list to store the output from the first result set (treat as ref: must be initialized)</param>
        /// <param name="modelBinder2">Func of type IDataRecord and T: the factory for binding the output to a model</param>
        /// <param name="output2">A list to store the output from the second result set (treat as ref: must be initialized)</param>
        /// <param name="modelBinder3">Func of type IDataRecord and T: the factory for binding the output to a model</param>
        /// <param name="output3">A list to store the output from the third result set (treat as ref: must be initialized)</param>
        /// <param name="modelBinder4">Func of type IDataRecord and T: the factory for binding the output to a model</param>
        /// <param name="output4">A list to store the output from the fourth result set (treat as ref: must be initialized)</param>
        /// <param name="modelBinder5">Func of type IDataRecord and T: the factory for binding the output to a model</param>
        /// <param name="output5">A list to store the output from the fourth result set (treat as ref: must be initialized)</param>
        /// <param name="commandBehavior">CommandBehavior: a description of the results of the query and its effect on the database (our default is CloseConnection)</param>
        /// <returns>List of Type T: the results in a collection</returns>
        public virtual void ExecuteAs<T1, T2, T3, T4, T5>(SqlCommand command,
            Func<IDataRecord, T1> modelBinder1, List<T1> output1,
            Func<IDataRecord, T2> modelBinder2, List<T2> output2,
            Func<IDataRecord, T3> modelBinder3, List<T3> output3,
            Func<IDataRecord, T4> modelBinder4, List<T4> output4,
            Func<IDataRecord, T5> modelBinder5, List<T5> output5,
            CommandBehavior commandBehavior = CommandBehavior.CloseConnection)
        {
            ExecuteAs<T1, T2, T3, T4, T5, object>(command,
                modelBinder1: modelBinder1, output1: output1,
                modelBinder2: modelBinder2, output2: output2,
                modelBinder3: modelBinder3, output3: output3,
                modelBinder4: modelBinder4, output4: output4,
                modelBinder5: modelBinder5, output5: output5,
                commandBehavior: commandBehavior);
        }

        /// <summary>
        /// Executes a SqlCommand that expects four result sets and binds the results to the given models
        /// </summary>
        /// <typeparam name="T1">Type: the type of object for the first result set</typeparam>
        /// <typeparam name="T2">Type: the type of object for the second result set</typeparam>
        /// <typeparam name="T3">Type: the type of object for the third result set</typeparam>
        /// <typeparam name="T4">Type: the type of object for the fourth result set</typeparam>
        /// <typeparam name="T5">Type: the type of object for the fourth result set</typeparam>
        /// <typeparam name="T6">Type: the type of object for the fourth result set</typeparam>
        /// <param name="command">SqlCommand: a stored procedure command</param>
        /// <param name="modelBinder1">Func of type IDataRecord and T: the factory for binding the output to a model</param>
        /// <param name="output1">A list to store the output from the first result set (treat as ref: must be initialized)</param>
        /// <param name="modelBinder2">Func of type IDataRecord and T: the factory for binding the output to a model</param>
        /// <param name="output2">A list to store the output from the second result set (treat as ref: must be initialized)</param>
        /// <param name="modelBinder3">Func of type IDataRecord and T: the factory for binding the output to a model</param>
        /// <param name="output3">A list to store the output from the third result set (treat as ref: must be initialized)</param>
        /// <param name="modelBinder4">Func of type IDataRecord and T: the factory for binding the output to a model</param>
        /// <param name="output4">A list to store the output from the fourth result set (treat as ref: must be initialized)</param>
        /// <param name="modelBinder5">Func of type IDataRecord and T: the factory for binding the output to a model</param>
        /// <param name="output5">A list to store the output from the fourth result set (treat as ref: must be initialized)</param>
        /// <param name="modelBinder6">Func of type IDataRecord and T: the factory for binding the output to a model</param>
        /// <param name="output6">A list to store the output from the fourth result set (treat as ref: must be initialized)</param>
        /// <param name="commandBehavior">CommandBehavior: a description of the results of the query and its effect on the database (our default is CloseConnection)</param>
        /// <returns>List of Type T: the results in a collection</returns>
        public virtual void ExecuteAs<T1, T2, T3, T4, T5, T6>(SqlCommand command,
            Func<IDataRecord, T1> modelBinder1 = null, List<T1> output1 = null,
            Func<IDataRecord, T2> modelBinder2 = null, List<T2> output2 = null,
            Func<IDataRecord, T3> modelBinder3 = null, List<T3> output3 = null,
            Func<IDataRecord, T4> modelBinder4 = null, List<T4> output4 = null,
            Func<IDataRecord, T5> modelBinder5 = null, List<T5> output5 = null,
            Func<IDataRecord, T6> modelBinder6 = null, List<T6> output6 = null,
            CommandBehavior commandBehavior = CommandBehavior.CloseConnection)
        {
            if (command == null)
                throw new ArgumentException("The command cannot be null when trying to execute a SqlCommand", "command");

            using (command)
            {
                command.Connection = SqlConnection;
                command.CommandTimeout = _defaultCommandTimeout;

                using (SqlDataReader reader = command.ExecuteReader(commandBehavior))
                {
                    reader.ReadRecordsTo<T1, T2, T3, T4, T5, T6>(
                        factory1: modelBinder1, out1: output1,
                        factory2: modelBinder2, out2: output2,
                        factory3: modelBinder3, out3: output3,
                        factory4: modelBinder4, out4: output4,
                        factory5: modelBinder5, out5: output5,
                        factory6: modelBinder6, out6: output6);
                } // end using reader
            } // end using command
        }

        #endregion Multiple Result Sets

        public void Dispose()
        {
            if (_sqlConnection != null) 
            {
                _sqlConnection.Close();
                _sqlConnection.Dispose();
            }
        }
    }


    public static class DbInstanceExtensions
    {
        /// <summary>
        /// Get the value or default(T) from a given IDataRecord (row) with the given column name
        /// </summary>
        /// <typeparam name="T">The type of output</typeparam>
        /// <param name="row">IDataRecord: the row that is being parsed</param>
        /// <param name="columnName">string: the column name for which to parse data from</param>
        /// <returns>Type of T: the value for the resulting field, as T</returns>
        /// <remarks>
        /// </remarks>
        [Cite(Author = "Sky Sanders", Link = "http://skysanders.net/subtext/archive/2010/03/02/generic-nullsafe-idatarecord-field-getter.aspx", Type = CiteType.Adaptation)]
        public static T GetValueOrDefault<T>(this IDataRecord row, string columnName)
        {
            return row.GetValueOrDefault<T>(row.GetOrdinal(columnName));
        }

        /// <summary>
        /// Get the value or default(T) from a given IDataRecord (row) by column ordinal
        /// </summary>
        /// <typeparam name="T">The type of output</typeparam>
        /// <param name="row">IDataRecord: the row that is being parsed</param>
        /// <param name="ordinal">int: the ordinal of the column</param>
        /// <returns>Type of T: the value for the resulting field, as T</returns>
        [Cite(Author = "Sky Sanders", Link = "http://skysanders.net/subtext/archive/2010/03/02/generic-nullsafe-idatarecord-field-getter.aspx", Type = CiteType.Copy)]
        public static T GetValueOrDefault<T>(this IDataRecord row, int ordinal)
        {
            return (T)(row.IsDBNull(ordinal) ? default(T) : row.GetValue(ordinal));
        }

        /// <summary>
        /// read the records that are returned for a given command, and bind them to the output ref
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <param name="modelBinder"></param>
        /// <param name="output"></param>
        public static void ReadRecordsTo<T>(this IDataReader reader, Func<IDataRecord, T> modelBinder, ref List<T> output)
        {
            while (reader.Read())
                output.Add(modelBinder(reader));
        }

        /// <typeparam name="T1">Type: the type of object for the first result set</typeparam>
        /// <typeparam name="T2">Type: the type of object for the second result set</typeparam>
        /// <typeparam name="T3">Type: the type of object for the third result set</typeparam>
        /// <typeparam name="T4">Type: the type of object for the fourth result set</typeparam>
        /// <typeparam name="T5">Type: the type of object for the fourth result set</typeparam>
        /// <typeparam name="T6">Type: the type of object for the fourth result set</typeparam>
        /// <param name="reader">IDataReader: the open DataReader</param>
        /// <param name="factory1">Func of type IDataRecord and T: the factory for binding the output to a model</param>
        /// <param name="out1">A list to store the output from the first result set (treat as ref: must be initialized)</param>
        /// <param name="factory2">Func of type IDataRecord and T: the factory for binding the output to a model</param>
        /// <param name="out2">A list to store the output from the second result set (treat as ref: must be initialized)</param>
        /// <param name="factory3">Func of type IDataRecord and T: the factory for binding the output to a model</param>
        /// <param name="out3">A list to store the output from the third result set (treat as ref: must be initialized)</param>
        /// <param name="factory4">Func of type IDataRecord and T: the factory for binding the output to a model</param>
        /// <param name="out4">A list to store the output from the fourth result set (treat as ref: must be initialized)</param>
        /// <param name="factory5">Func of type IDataRecord and T: the factory for binding the output to a model</param>
        /// <param name="out5">A list to store the output from the fourth result set (treat as ref: must be initialized)</param>
        /// <param name="factory6">Func of type IDataRecord and T: the factory for binding the output to a model</param>
        /// <param name="out6">A list to store the output from the fourth result set (treat as ref: must be initialized)</param>
        public static void ReadRecordsTo<T1, T2, T3, T4, T5, T6>(this IDataReader reader,
            Func<IDataRecord, T1> factory1 = null, List<T1> out1 = null,
            Func<IDataRecord, T2> factory2 = null, List<T2> out2 = null,
            Func<IDataRecord, T3> factory3 = null, List<T3> out3 = null,
            Func<IDataRecord, T4> factory4 = null, List<T4> out4 = null,
            Func<IDataRecord, T5> factory5 = null, List<T5> out5 = null,
            Func<IDataRecord, T6> factory6 = null, List<T6> out6 = null)
        {
            if (factory1 != null)
            {
                out1 = out1 != null ? out1 : new List<T1> { };
                reader.ReadRecordsTo<T1>(factory1, ref out1);
            }

            if (factory2 != null)
            {
                out2 = out2 != null ? out2 : new List<T2> { };
                reader.NextResult();
                reader.ReadRecordsTo<T2>(factory2, ref out2);
            }
            else return;

            if (factory3 != null)
            {
                out3 = out3 != null ? out3 : new List<T3> { };
                reader.NextResult();
                reader.ReadRecordsTo<T3>(factory3, ref out3);
            }
            else return;

            if (factory4 != null)
            {
                out4 = out4 != null ? out4 : new List<T4> { };
                reader.NextResult();
                reader.ReadRecordsTo<T4>(factory4, ref out4);
            }
            else return;

            if (factory5 != null)
            {
                out5 = out5 != null ? out5 : new List<T5> { };
                reader.NextResult();
                reader.ReadRecordsTo<T5>(factory5, ref out5);
            }
            else return;

            if (factory6 != null)
            {
                out6 = out6 != null ? out6 : new List<T6> { };
                reader.NextResult();
                reader.ReadRecordsTo<T6>(factory6, ref out6);
            }

            return;
        }
    }

    public class ModelBinderParameter<T>
    {
        IEnumerable<T> Output { get; set; }
        Func<IDataRecord, T> ModelBinder { get; set; }
    }
}

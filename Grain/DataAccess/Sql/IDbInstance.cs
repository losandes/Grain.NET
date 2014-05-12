using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Grain.DataAccess.Sql
{
    public interface ISqlDbInstance
    {
        SqlConnection SqlConnection { get; }

        /// <summary>
        /// Executes a non-query Transact-SQL statement and returns the number of rows that were affected (assuming NOCOUNT is not on)
        /// </summary>
        /// <param name="command">SqlCommand: the command to be executed</param>
        /// <returns>int: if NOCOUNT is on, on the database, the number of rows that were affected</returns>
        int Execute(SqlCommand command);

        /// <summary>
        /// Executes a SqlCommand and returns a single result via a factory pattern (selector param).  If more than one result 
        /// was present in the SQL execution, all but the first are omitted.
        /// </summary>
        /// <typeparam name="T">The type of output object</typeparam>
        /// <param name="command">SqlCommand: the command to execute</param>
        /// <param name="modelBinder">Func of type IDataRecord and T: the factory for binding the output to a model</param>
        /// <returns>IEnumerable of type T: the result set bound to a model, by way of the selector factory.</returns>
        T ExecuteAsSingle<T>(SqlCommand command, Func<IDataRecord, T> modelBinder, CommandBehavior commandBehavior = CommandBehavior.CloseConnection);

        /// <summary>
        /// Executes a SqlCommand and returns the results via a factory pattern (selector param)
        /// </summary>
        /// <typeparam name="T">The type of output object</typeparam>
        /// <param name="command">SqlCommand: the command to execute</param>
        /// <param name="modelBinder">Func of type IDataRecord and T: the factory for binding the output to a model</param>
        /// <returns>IEnumerable of type T: the result set bound to a model, by way of the selector factory.</returns>
        IEnumerable<T> ExecuteAs<T>(SqlCommand command, Func<IDataRecord, T> modelBinder, CommandBehavior commandBehavior = CommandBehavior.CloseConnection);
        
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
        void ExecuteAs<T1, T2>(SqlCommand command,
            Func<IDataRecord, T1> modelBinder1, List<T1> output1,
            Func<IDataRecord, T2> modelBinder2, List<T2> output2,
            CommandBehavior commandBehavior = CommandBehavior.CloseConnection);

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
        void ExecuteAs<T1, T2, T3>(SqlCommand command,
            Func<IDataRecord, T1> modelBinder1, List<T1> output1,
            Func<IDataRecord, T2> modelBinder2, List<T2> output2,
            Func<IDataRecord, T3> modelBinder3, List<T3> output3,
            CommandBehavior commandBehavior = CommandBehavior.CloseConnection);

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
        void ExecuteAs<T1, T2, T3, T4>(SqlCommand command,
            Func<IDataRecord, T1> modelBinder1, List<T1> output1,
            Func<IDataRecord, T2> modelBinder2, List<T2> output2,
            Func<IDataRecord, T3> modelBinder3, List<T3> output3,
            Func<IDataRecord, T4> modelBinder4, List<T4> output4,
            CommandBehavior commandBehavior = CommandBehavior.CloseConnection);

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
        void ExecuteAs<T1, T2, T3, T4, T5>(SqlCommand command,
            Func<IDataRecord, T1> modelBinder1, List<T1> output1,
            Func<IDataRecord, T2> modelBinder2, List<T2> output2,
            Func<IDataRecord, T3> modelBinder3, List<T3> output3,
            Func<IDataRecord, T4> modelBinder4, List<T4> output4,
            Func<IDataRecord, T5> modelBinder5, List<T5> output5,
            CommandBehavior commandBehavior = CommandBehavior.CloseConnection);

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
        void ExecuteAs<T1, T2, T3, T4, T5, T6>(SqlCommand command,
            Func<IDataRecord, T1> modelBinder1 = null, List<T1> output1 = null,
            Func<IDataRecord, T2> modelBinder2 = null, List<T2> output2 = null,
            Func<IDataRecord, T3> modelBinder3 = null, List<T3> output3 = null,
            Func<IDataRecord, T4> modelBinder4 = null, List<T4> output4 = null,
            Func<IDataRecord, T5> modelBinder5 = null, List<T5> output5 = null,
            Func<IDataRecord, T6> modelBinder6 = null, List<T6> output6 = null,
            CommandBehavior commandBehavior = CommandBehavior.CloseConnection);

        #endregion Multiple Result Sets

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or
        //  resetting unmanaged resources.
        /// </summary>
        void Dispose();
    }
}

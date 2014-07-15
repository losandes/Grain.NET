using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Grain.DataAccess.Sql
{
    public interface ISqlCommandFactory
    {
        /// <summary>
        /// Make a SqlCommand of type StoredProc, that executes a command by the given proc
        /// </summary>
        /// <param name="proc">the name of the stored procedure (i.e. dbo.myProc)</param>
        /// <returns>The SqlCommand</returns>
        SqlCommand MakeProcCommand(string proc);

        SqlCommand MakeProcCommand(string proc, TupleList<string, SqlDbType, object> parameters);
    }
}

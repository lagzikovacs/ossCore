using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Models
{
    public partial class ossContext
    {
        public async Task ExecuteLockFunction(string lockName, string kodName, int kod, DateTime modositva)
        {
            var cmd = Database.GetDbConnection().CreateCommand();

            cmd.Transaction = Database.CurrentTransaction.GetDbTransaction();
            cmd.CommandText = lockName;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@" + kodName, SqlDbType.Int) { Value = kod });
            cmd.Parameters.Add(new SqlParameter("@utoljaramodositva", SqlDbType.DateTime) { Value = modositva });

            cmd.Parameters.Add(new SqlParameter("@result", SqlDbType.Int) { Direction = ParameterDirection.Output });

            await cmd.ExecuteNonQueryAsync();

            if ((int)cmd.Parameters["@result"].Value != 1)
                throw new Exception("Ezt a tételt egy másik felhasználó már módosította!");
        }
    }
}

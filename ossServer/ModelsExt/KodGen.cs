using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ossServer.Enums;
using System.Data;

namespace ossServer.Models
{
    public partial class ossContext
    {
        public int KodGen(KodNev KodNev)
        {
            return KodGen(KodNev.ToString());
        }

        public int KodGen(string KodNev)
        {
            var cmd = Database.GetDbConnection().CreateCommand();

            cmd.Transaction = Database.CurrentTransaction.GetDbTransaction();
            cmd.CommandText = "KODGEN";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@ParticioKod", SqlDbType.Int) { Value = CurrentSession.Particiokod });
            cmd.Parameters.Add(new SqlParameter("@KodNev", SqlDbType.VarChar) { Value = KodNev });

            cmd.Parameters.Add(new SqlParameter("@result", SqlDbType.Int) { Direction = ParameterDirection.Output });

            cmd.ExecuteNonQuery();

            return (int)cmd.Parameters["@result"].Value;
        }
    }
}

using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KFA.SubSystem;
using KFA.SubSystem.Globals;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace KFA.SubSystem.Globals.DataLayer;

public static class SQLServerDbService
{
  public static SqlParameter[]? CreateParameters(Dictionary<string, object>? parameters) => parameters?.Select(n => new SqlParameter(n.Key, n.Value))?.ToArray();
  public static async Task SQLServerExecuteQuery(string sql, params SqlParameter[] parameters)
  {
    using var con = SQLServerDbConnection;
    await con!.OpenAsync();
    using var trans = con.BeginTransaction();
    using var cmd = new SqlCommand(sql, con);
    cmd.Transaction = trans;
    cmd.Parameters.AddRange(parameters);
    await cmd.ExecuteNonQueryAsync();
    trans.Commit();
  }

  public static async Task<object?> SQLServerGetScalar(string sql, params SqlParameter[] parameters)
  {
    using var con = SQLServerDbConnection;
    await con.OpenAsync();
    using var cmd = new SqlCommand(sql, con);
    cmd.Parameters.AddRange(parameters);
    return await cmd.ExecuteScalarAsync();
  }

  public static async Task<DataSet> SQLServerGetDataset(string sql, params SqlParameter[] parameters)
  {
    using var con = SQLServerDbConnection;
    await con.OpenAsync();
    using var cmd = new SqlCommand(sql, con);

    cmd.CommandText = sql;
    using var adapter = new SqlDataAdapter(cmd);
    var table = new DataSet();
    adapter.Fill(table);
    return table;
  }
  public static SqlConnection SQLServerDbConnection
  {
    get
    {
      var config = Functions.ResolveObject<IConfiguration>();
      var conString = config!.GetConnectionString("SQLServerConnection");
      return new SqlConnection(conString);
    }
  }
}

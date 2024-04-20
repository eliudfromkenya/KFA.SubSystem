using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KFA.SubSystem;
using KFA.SubSystem.Globals;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace KFA.SubSystem.Globals.DataLayer;

public static class MySQLDbService
{
  public static MySqlParameter[]? CreateParameters(Dictionary<string, object>? parameters) 
    => parameters?.Select(n => new MySqlParameter(n.Key, n.Value))?.ToArray();
  public static async Task MySQLExecuteQuery(string sql, params MySqlParameter[] parameters)
  {
    using var con = MySQLDbConnection;
    await con!.OpenAsync();
    using var trans = con.BeginTransaction();
    using var cmd = new MySqlCommand(sql, con);
    cmd.Transaction = trans;
    if (parameters?.Length > 0)
      cmd.Parameters.AddRange(parameters);
    await cmd.ExecuteNonQueryAsync();
    trans.Commit();
  }

  public static async Task<object?> MySQLGetScalar(string sql, params MySqlParameter[] parameters)
  {
    using var con = MySQLDbConnection;
    await con.OpenAsync();
    using var cmd = new MySqlCommand(sql, con);
    if (parameters?.Length > 0)
      cmd.Parameters.AddRange(parameters);

    return await cmd.ExecuteScalarAsync();
  }

  public static async Task<DataSet> MySQLGetDataset(string sql, params MySqlParameter[] parameters)
  {
    using var con = MySQLDbConnection;
    await con.OpenAsync();
    using var cmd = new MySqlCommand(sql, con);
    cmd.CommandText = sql;
    if (parameters?.Length > 0)
      cmd.Parameters.AddRange(parameters);

    using var adapter = new MySqlDataAdapter(cmd);
    var table = new DataSet();
    adapter.Fill(table);
    return table;
  }
  public static MySqlConnection MySQLDbConnection
  {
    get
    {
      var conString = Functions.GetEnviromentVariable("KFASubSystemMySQLDatabase");
      return new MySqlConnection(conString);
    }
  }
}

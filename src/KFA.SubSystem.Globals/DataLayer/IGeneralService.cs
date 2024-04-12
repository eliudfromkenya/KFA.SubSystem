namespace KFA.SubSystem.Globals.DataLayer;

public interface IGeneralService
{
  Task<TableMetaData[]> RefreshKeys(string prefix);
}

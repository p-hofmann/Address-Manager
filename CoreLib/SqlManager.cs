using System;
using System.IO;
using System.Data.SQLite;

namespace CoreLib
{
  /// <summary>
  /// Container of all SQLite related methods
  /// </summary>
  public class SqlManager
  {
    const string _sqlCreateFileLocation = "./CoreLib/sql/dbCreate.sql";

    /// <summary>
    /// Debug mode.
    /// 
    /// Create temporary file at unique default path and use it as default path, so UnitTests can be run parallel without conflict.
    /// </summary>
    private Boolean _debug;

    /// <summary>
    /// Default location of database.
    /// </summary>
    private string _dbLocation;

    /// <summary>
    /// Default location of database.
    /// </summary>
    private string _dbSource;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="defaultDbLocation"></param>
    /// <param name="debug">Debug mode</param>
    public SqlManager(string defaultDbLocation = "./sqlDatabase", Boolean debug = false)
    {
      _debug = debug;
      _dbLocation = defaultDbLocation;
      _dbSource = @"URI=file:" + _dbLocation;
    }

    /// <summary>
    /// Initialize a new, empty DB
    /// </summary>
    public void DbInitialize()
    {
      if (_debug)
        _dbLocation = Path.GetTempFileName();

      var con = new SQLiteConnection(_dbSource);
      con.Open();

      string stm;
      if (_debug)
      {
        stm = "SELECT SQLITE_VERSION()";
        var command = new SQLiteCommand(stm, con);
        string version = command.ExecuteScalar().ToString();
        Console.WriteLine($"SQLite version: {version}");      // TODO
      }

      stm = File.ReadAllText(_sqlCreateFileLocation);
      var cmd = new SQLiteCommand(stm, con);
      cmd.ExecuteNonQuery();
      con.Close();
    }

    /// <summary>
    /// Delete DB
    /// </summary>
    public void DbDelete()
    {
      // TODO
    }

    /// <summary>
    /// Clear DB of all entries
    /// </summary>
    public void DbClear()
    {
      // TODO
    }

    /// <summary>
    /// Get all entries in db
    /// </summary>
    public void EntryGetAll()
    {
      // TODO
    }

    public void EntryAdd()
    {
      // TODO
    }

    public void EntryRemove()
    {
      // TODO
    }

    public void EntryModify()
    {
      // TODO
    }
  }
}

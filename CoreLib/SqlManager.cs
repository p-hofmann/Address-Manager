using System;
using System.IO;

namespace CoreLib
{
  /// <summary>
  /// Container of all SQLite related methods
  /// </summary>
  public class SqlManager
  {
    /// <summary>
    /// Debug mode.
    /// 
    /// Create temporary file at unique default path and use it as default path, so UnitTests can be run parallel without conflict.
    /// </summary>
    private Boolean _debug;

    /// <summary>
    /// Default location of database.
    /// </summary>
    private string _defaultDbLocation;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="defaultDbLocation"></param>
    /// <param name="debug">Debug mode</param>
    public SqlManager(string defaultDbLocation = "./sqlDatabase", Boolean debug = false)
    {
      _debug = debug;
      _defaultDbLocation = defaultDbLocation;

      if (debug)
        _defaultDbLocation = Path.GetTempFileName();
    }

    /// <summary>
    /// Initialize a new, empty DB
    /// </summary>
    public void DbInitialize()
    {
      // TODO
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
    /// Clear DB of all entries
    /// </summary>
    public void DbConnect()
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

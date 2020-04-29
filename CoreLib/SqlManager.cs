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
    const string _sqlCreateFileLocation = "./dbCreate.sql";

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
        Console.WriteLine($"SQLite version: {version}");
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
      if (File.Exists(_dbLocation))
        File.Delete(_dbLocation);
    }

    /// <summary>
    /// Clear DB of all entries
    /// </summary>
    public void DbClear()
    {
      // TODO: DELETE FROM person;
    }

    /// <summary>
    /// Get all entries in db
    /// </summary>
    public void EntryGetAll()
    {
      // TODO:
      /*
      SELECT 
      person.name_first, person.name_last, city.city_name, street.name, address.house_number 
      FROM person 
      JOIN address ON person.id_adress = address.id 
      JOIN city ON address.id_city = city.id 
      JOIN street ON address.id_street = city.id 
      ORDER BY person.name_last;
      */
    }

    public void EntryGetPhone(int id_person)
    {
      // TODO
      /*
      SELECT
phone_category.name, phone_number.pnumber
FROM list_phone 
JOIN phone_number ON list_phone.id_phone_number = phone_number.id 
JOIN phone_category ON phone_number.id_phone_category = phone_category.id 
WHERE list_phone.id_person = {0}
ORDER BY phone_category.name;
*/
    }

    public void EntryGetPhoneList(int id_person)
    {
      // TODO
      /*
      SELECT 
      phone_number.id, phone_category.name, phone_number.pnumber 
      FROM list_phone 
      JOIN phone_number ON list_phone.id_phone_number = phone_number.id 
      JOIN phone_category ON phone_number.id_phone_category = phone_category.id 
      WHERE list_phone.id_person = {0}
      ORDER BY phone_category.name;
      */
    }

    public void EntryGetPictureList(int id_person)
    {
      // TODO
      /*
      SELECT 
      picture.id, picture.name 
      FROM list_picture 
      JOIN picture ON list_picture.id_picture = picture.id 
      WHERE list_picture.id_person = {0}
      ORDER BY picture.name;
      */
    }

    public void EntryAddPicture(int id_person, string fileLocation)
    {
      // TODO
    }

    public void EntryAddPhone(int id_person, string phoneNumber, string phoneCategory)
    {
      // TODO
    }

    public void EntryRemove(int id_person)
    {
      // TODO: DELETE FROM person WHERE id = {0};
    }

    public void EntryModify()
    {
      // TODO
    }
  }
}

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
    /// Turn file into byte array
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static byte[] FileToByteArray(string filePath)
    {
      if (string.IsNullOrEmpty(filePath))
        throw new Exception(string.Format("Bad file path: '{0}'", filePath));
      if (!File.Exists(filePath))
        throw new Exception(string.Format("Bad file path: '{0}'", filePath));

      FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
      BinaryReader br = new BinaryReader(fs);

      byte[] byteArray = br.ReadBytes((int)fs.Length);

      br.Close();
      fs.Close();

      return byteArray;
    }
    
    /// <summary>
    /// Initialize a new, empty DB
    /// </summary>
    public void DbInitialize()
    {
      if (_debug)
      {
        _dbLocation = Path.GetTempFileName();
        _dbSource = @"URI=file:" + _dbLocation;
      }

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
      var x = cmd.ExecuteNonQuery();
      con.Close();
    }

    /// <summary>
    /// Delete DB
    /// </summary>
    public void DbDelete()
    {
      if (File.Exists(_dbLocation))
      {
        // wait for db reference to be removed
        GC.Collect();
        GC.WaitForPendingFinalizers();

        // delete file
        File.Delete(_dbLocation);
      }
    }

    /// <summary>
    /// Clear DB of all entries
    /// </summary>
    public void DbClear()
    {
      var con = new SQLiteConnection(_dbSource);
      con.Open();

      string stm;

      stm = "DELETE FROM person;";
      var cmd = new SQLiteCommand(stm, con);
      cmd.ExecuteNonQuery();
      con.Close();
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

    private int EntryAddCountry(SQLiteConnection con, int countryCode, string countryName)
    {
      string stm;

      stm = string.Format(
        "REPLACE INTO country " +
        "(country_code, country_name)" +
        " VALUES " +
        "({0}, {1})",
        countryCode, countryName);
      var cmd = new SQLiteCommand(stm, con);
      var result = cmd.ExecuteScalar();
      if (result == null)
        return -1;
      return (int)result;
    }

    private int EntryAddCity(SQLiteConnection con, string postalCode, string cityName, int idCountry)
    {
      string stm;

      stm = string.Format(
        "REPLACE INTO city " +
        "(postal_code, city_name, id_country)" +
        " VALUES " +
        "({0}, {1}, {2})",
        postalCode, cityName, idCountry);

      var cmd = new SQLiteCommand(stm, con);

      var result = cmd.ExecuteScalar();
      if (result == null)
        return -1;
      return (int)result;
    }

    private int EntryAddStreet(SQLiteConnection con, string streetName, int idCity)
    {
      string stm;

      stm = string.Format(
        "REPLACE INTO street " +
        "(name, id_city)" +
        " VALUES " +
        "({0}, {1})",
        streetName, idCity);

      var cmd = new SQLiteCommand(stm, con);

      var result = cmd.ExecuteScalar();
      if (result == null)
        return -1;
      return (int)result;
    }

    private int EntryAddAddress(SQLiteConnection con, int streetId, int idCity, string houseNumber)
    {
      string stm;

      stm = string.Format(
        "REPLACE INTO address " +
        "(id_street, id_city, house_number)" +
        " VALUES " +
        "({0}, {1}, {2})",
        streetId, idCity, houseNumber);

      var cmd = new SQLiteCommand(stm, con);

      var result = cmd.ExecuteScalar();
      if (result == null)
        return -1;
      return (int)result;
    }

    private int EntryAddPerson(SQLiteConnection con, string nameFirst, string nameLast, int idAddress)
    {
      string stm;

      stm = string.Format(
        "INSERT INTO person " +
        "(name_first, name_last, id_address)" +
        " VALUES " +
        "({0}, {1}, {2})",
        nameFirst, nameLast, idAddress);
      var cmd = new SQLiteCommand(stm, con);
      var result = cmd.ExecuteScalar();
      if (result == null)
        return -1;
      return (int)result;
    }

    private int EntryAddPicture(SQLiteConnection con, string fileLocation)
    {
      string stm;

      byte[] fileData = FileToByteArray(fileLocation);

      stm = string.Format(
        "INSERT INTO picture " +
        "(name, data)" +
        " VALUES " +
        "({0}, @data)",
        fileLocation);

      var cmd = new SQLiteCommand(stm, con);
      cmd.Parameters.Add("@data", System.Data.DbType.Byte, fileData.Length).Value = fileData;

      var result = cmd.ExecuteScalar();
      if (result == null)
        return -1;
      return (int)result;
    }

    private int EntryAddCategory(SQLiteConnection con, string phoneCategory)
    {
      string stm = string.Format(
        "REPLACE INTO phone_category " +
        "(name)" +
        " VALUES " +
        "({0})",
        phoneCategory);

      var cmd = new SQLiteCommand(stm, con);

      var result = cmd.ExecuteScalar();
      if (result == null)
        return -1;
      return (int)result;
    }

    private int EntryAddPhone(SQLiteConnection con, int id_person, string phoneNumber, int idPhoneCategory)
    {
      string stm = string.Format(
        "INSERT INTO phone_number " +
        "(pnumber, id_phone_category)" +
        " VALUES " +
        "({0}, {1})",
        id_person, idPhoneCategory);

      var cmd = new SQLiteCommand(stm, con);

      var result = cmd.ExecuteScalar();
      if (result == null)
        return -1;
      return (int)result;
    }

    public Boolean AddPhone(int id_person, string phoneNumber, string phoneCategory)
    {
      var con = new SQLiteConnection(_dbSource);
      con.Open();

      int resultId = EntryAddCategory(con, phoneCategory);
      if (resultId < 0)
        throw new Exception("Error adding phone category!");

      resultId = EntryAddPhone(con, id_person, phoneNumber, resultId);
      if (resultId < 0)
        throw new Exception("Error adding phone!");

      string stm = string.Format(
        "INSERT INTO list_phone " +
        "(id_phone_number, id_person)" +
        " VALUES " +
        "({0}, {1})",
        resultId, id_person);

      var cmd = new SQLiteCommand(stm, con);

      var result = cmd.ExecuteNonQuery();
      con.Close();

      return result == 1;
    }

    public Boolean AddPicture(int id_person, string fileLocation)
    {
      var con = new SQLiteConnection(_dbSource);
      con.Open();

      int resultId = EntryAddPicture(con, fileLocation);
      if (resultId < 0)
        throw new Exception("Error adding phone category!");

      string stm = string.Format(
        "INSERT INTO list_phone " +
        "(id_picture, id_person)" +
        " VALUES " +
        "({0}, {1})",
        resultId, id_person);

      var cmd = new SQLiteCommand(stm, con);

      var result = cmd.ExecuteNonQuery();
      con.Close();

      return result == 1;
    }

    public Boolean AddPerson(
      string nameFirst, string nameLast,
      string streetName, string houseNumber, string postalCode, string cityName,
      int countryCode = 49, string countryName = "Germany")
    {
      nameFirst = nameFirst.Trim();
      nameLast = nameLast.Trim();
      streetName = streetName.Trim();
      houseNumber = houseNumber.Trim();
      postalCode = postalCode.Trim();
      cityName = cityName.Trim();
      countryName = countryName.Trim();

      if (string.IsNullOrEmpty(nameFirst))
        throw new Exception("Invalid first name!");
      if (string.IsNullOrEmpty(nameLast))
        throw new Exception("Invalid last name!");

      var con = new SQLiteConnection(_dbSource);
      con.Open();

      int idCountry = 0;
      if (!string.IsNullOrEmpty(countryName))
      {
        idCountry = EntryAddCountry(con, countryCode, countryName);
        if (idCountry < 0)
          throw new Exception("Error adding country!");
      }

      int idCity = 0;
      if (!string.IsNullOrEmpty(cityName) && !string.IsNullOrEmpty(postalCode))
      {
        if (string.IsNullOrEmpty(postalCode))
          postalCode = "NULL";
        if (string.IsNullOrEmpty(cityName))
          cityName = "NULL";
        idCity = EntryAddCity(con, postalCode, cityName, idCountry);
        if (idCity < 0)
          throw new Exception("Error adding city!");
      }

      int idStreet = 0;
      if (!string.IsNullOrEmpty(streetName))
      {
        idStreet = EntryAddStreet(con, streetName, idCity);
        if (idStreet < 0)
          throw new Exception("Error adding street!");
      }

      if (string.IsNullOrEmpty(houseNumber))
        houseNumber = "NULL";

      int idAddress = EntryAddAddress(con, idStreet, idCity, houseNumber);
      if (idAddress < 0)
        throw new Exception("Error adding address!");

      int idPerson = EntryAddPerson(con, nameFirst, nameLast, idAddress);

      con.Close();

      return idPerson > 0;
    }

    public Boolean RemovePerson(int id_person)
    {
      string stm = string.Format("DELETE FROM person WHERE id = {0}", id_person);

      var con = new SQLiteConnection(_dbSource);
      con.Open();

      var cmd = new SQLiteCommand(stm, con);
      var result = cmd.ExecuteNonQuery();

      con.Close();

      return result == 1;
    }

    public void EntryModify()
    {
      // TODO
    }
  }
}

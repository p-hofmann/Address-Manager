using System;
using System.IO;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CoreLib
{
  /// <summary>
  /// Container of all SQLite related methods
  /// </summary>
  public class SqlHandler
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
    public SqlHandler(string defaultDbLocation = "./sqlDatabase", Boolean debug = false)
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

    private long EntryAddCountry(SQLiteConnection con, int countryCode, string countryName)
    {
      long idEntry = EntryGetCountryId(con, countryCode, countryName);
      if (idEntry != 0)
        return idEntry;

      string stm;
      stm = string.Format(
        "REPLACE INTO country " +
        "(country_code, country_name)" +
        " VALUES " +
        "({0}, {1})",
        countryCode, countryName);
      var cmd = new SQLiteCommand(stm, con);
      var result = cmd.ExecuteNonQuery();
      if (result == 0)
        return 0;
      return con.LastInsertRowId;
    }

    private long EntryAddCity(SQLiteConnection con, string postalCode, string cityName, long idCountry=49)
    {
      long idEntry = EntryGetCityId(con, postalCode, cityName, idCountry);
      if (idEntry != 0)
        return idEntry;

      string stm;
      stm = string.Format(
        "REPLACE INTO city " +
        "(postal_code, city_name, id_country)" +
        " VALUES " +
        "({0}, {1}, {2})",
        postalCode, cityName, idCountry);

      var cmd = new SQLiteCommand(stm, con);

      var result = cmd.ExecuteNonQuery();
      if (result == 0)
        return 0;
      return con.LastInsertRowId;
    }

    private long EntryAddStreet(SQLiteConnection con, string streetName, long idCity)
    {
      long idEntry = EntryGetStreetId(con, idCity, streetName);
      if (idEntry != 0)
        return idEntry;

      string stm;
      stm = string.Format(
        "REPLACE INTO street " +
        "(name, id_city)" +
        " VALUES " +
        "({0}, {1})",
        streetName, idCity);

      var cmd = new SQLiteCommand(stm, con);

      var result = cmd.ExecuteNonQuery();
      if (result == 0)
        return 0;
      return con.LastInsertRowId;
    }

    private long EntryAddAddress(SQLiteConnection con, long streetId, long idCity, string houseNumber)
    {
      string stm;

      stm = string.Format(
        "REPLACE INTO address " +
        "(id_street, id_city, house_number)" +
        " VALUES " +
        "({0}, {1}, {2})",
        streetId, idCity, houseNumber);

      var cmd = new SQLiteCommand(stm, con);

      var result = cmd.ExecuteNonQuery();
      if (result == 0)
        return 0;
      return con.LastInsertRowId;
    }

    private long EntryAddPerson(SQLiteConnection con, string nameFirst, string nameLast, long idAddress)
    {
      string stm;

      stm = string.Format(
        "INSERT INTO person " +
        "(name_first, name_last, id_address)" +
        " VALUES " +
        "({0}, {1}, {2})",
        nameFirst, nameLast, idAddress);
      var cmd = new SQLiteCommand(stm, con);

      var result = cmd.ExecuteNonQuery();
      if (result == 0)
        return 0;
      return con.LastInsertRowId;
    }

    private long EntryAddPicture(SQLiteConnection con, string fileLocation)
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

      var result = cmd.ExecuteNonQuery();
      if (result == 0)
        return 0;
      return con.LastInsertRowId;
    }

    private long EntryAddPhoneCategory(SQLiteConnection con, string phoneCategory)
    {
      long idCategory = EntryGetPhoneCategoryId(con, phoneCategory);
      if (idCategory != 0)
        return idCategory;

      string stm = string.Format(
        "REPLACE INTO phone_category " +
        "(name)" +
        " VALUES " +
        "({0})",
        phoneCategory);

      var cmd = new SQLiteCommand(stm, con);

      var result = cmd.ExecuteNonQuery();
      if (result == 0)
        return 0;
      return con.LastInsertRowId;
    }

    private long EntryAddPhoneNumber(SQLiteConnection con, long id_person, string phoneNumber, long idPhoneCategory)
    {
      string stm = string.Format(
        "INSERT INTO phone_number " +
        "(pnumber, id_phone_category)" +
        " VALUES " +
        "({0}, {1})",
        id_person, idPhoneCategory);

      var cmd = new SQLiteCommand(stm, con);

      var result = cmd.ExecuteNonQuery();
      if (result == 0)
        return 0;
      return con.LastInsertRowId;
    }

    private Boolean EntryAddPhoneEntry(SQLiteConnection con, long id_person, long idPhoneNumber)
    {
      string stm = string.Format(
        "INSERT INTO list_phone " +
        "(id_phone_number, id_person)" +
        " VALUES " +
        "({0}, {1})",
        idPhoneNumber, id_person);

      var cmd = new SQLiteCommand(stm, con);

      var result = cmd.ExecuteNonQuery();
      return result == 1;
    }

    public Boolean AddPhone(long id_person, string phoneNumber, string phoneCategory)
    {
      var con = new SQLiteConnection(_dbSource);
      con.Open();

      long idPhoneCategory = EntryAddPhoneCategory(con, phoneCategory);
      if (idPhoneCategory == 0)
        throw new Exception("Error adding phone category!");

      long idPhoneNumber = EntryAddPhoneNumber(con, id_person, phoneNumber, idPhoneCategory);
      if (idPhoneNumber == 0)
        throw new Exception("Error adding phone!");

      string stm = string.Format(
        "INSERT INTO list_phone " +
        "(id_phone_number, id_person)" +
        " VALUES " +
        "({0}, {1})",
        idPhoneNumber, id_person);

      var cmd = new SQLiteCommand(stm, con);

      var result = cmd.ExecuteNonQuery();
      con.Close();

      return result == 1;
    }

    public Boolean AddPicture(int id_person, string fileLocation)
    {
      var con = new SQLiteConnection(_dbSource);
      con.Open();

      long resultId = EntryAddPicture(con, fileLocation);
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

    public Boolean AddCountry(int countryCode, string countryName)
    {
      var con = new SQLiteConnection(_dbSource);
      con.Open();

      long resultId = EntryAddCountry(con, countryCode, countryName);

      return resultId != 0;
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
        nameFirst = "";
      if (string.IsNullOrEmpty(nameLast))
        nameLast = "";

      var con = new SQLiteConnection(_dbSource);
      con.Open();

      long idCountry = 0;
      if (!string.IsNullOrEmpty(countryName))
      {
        idCountry = EntryAddCountry(con, countryCode, countryName);
        if (idCountry < 0)
          throw new Exception("Error adding country!");
      }

      long idCity = 0;
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

      long idStreet = 0;
      if (!string.IsNullOrEmpty(streetName))
      {
        idStreet = EntryAddStreet(con, streetName, idCity);
        if (idStreet < 0)
          throw new Exception("Error adding street!");
      }

      if (string.IsNullOrEmpty(houseNumber))
        houseNumber = "NULL";

      long idAddress = EntryAddAddress(con, idStreet, idCity, houseNumber);
      if (idAddress < 0)
        throw new Exception("Error adding address!");

      long idPerson = EntryAddPerson(con, nameFirst, nameLast, idAddress);

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

    public Boolean EntryUpdatePerson(SQLiteConnection con, int id_person, string nameFirst, string nameLast)
    {
      if (nameFirst == null && nameLast == null)
        return false;

      string updateColumn = "";
      if (nameFirst != null)
        updateColumn += string.Format("name_first = {0}", nameFirst);

      if (nameLast != null)
      {
        if (updateColumn.Length > 0)
          updateColumn += ", ";
        updateColumn += string.Format("name_last = {0}", nameLast);
      }

      string stm;

      stm = string.Format(
        "UPDATE person " +
        "SET " + updateColumn +
        " WHERE " +
        "person.id = {0}",
        id_person);
      var cmd = new SQLiteCommand(stm, con);
      var result = cmd.ExecuteNonQuery();
      return result == 1;
    }

    public Boolean EntryUpdateCity(
      SQLiteConnection con, long idPerson, long idCity, string postalCode, string cityName)
    {
      long idAdress = EntryGetAddressId(con, idPerson);

      if (postalCode == "" && cityName == "")
      {
        // Todo: remove city
      }

      if (cityName != null)
      {
        // Todo: if city only assigned to one person, update city
        if (idCity == 0)
          idCity = EntryAddCity(con, postalCode, cityName);
      }

      if (idCity == 0)
        throw new Exception("Could not update city!");

      string stm;
      stm = string.Format(
        "UPDATE address " +
        "SET id_city = {0}" +
        "WHERE " +
        "address.id = {1}",
        idCity, idAdress);
      var cmd = new SQLiteCommand(stm, con);
      var result = cmd.ExecuteNonQuery();
      return result == 1;
    }

    public Boolean EntryUpdateStreet(
      SQLiteConnection con, long idPerson, long idStreet, string streetName, string houseNumber)
    {
      long idAdress = EntryGetAddressId(con, idPerson);
      long idCity = EntryGetCityId(con, idAdress);

      if (streetName == "" && houseNumber == "")
      {
        // Todo: remove city
      }

      if (streetName != null)
      {
        // Todo: if street only assigned to one person, update street
        if (idStreet == 0)
          idStreet = EntryAddStreet(con, streetName, idCity);
      }

      string stm;
      stm = string.Format(
        "UPDATE address " +
        "SET id_street = {0}, house_number = {1}" +
        "WHERE " +
        "address.id = {2}",
        idStreet, houseNumber, idAdress);
      var cmd = new SQLiteCommand(stm, con);
      var result = cmd.ExecuteNonQuery();
      return result == 1;
    }

    public Boolean EntryUpdatePhoneCategory(
      SQLiteConnection con, long idPerson, long idPhoneEntry, long idPhoneCategory, string phoneCategory, string phoneNumber)
    {

      if (phoneCategory == "")
      {
        // Todo: remove
      }

      if (phoneCategory != null)
      {
        // Todo: if street only assigned to one person, update street
        if (idPhoneCategory == 0)
          idPhoneCategory = EntryAddPhoneCategory(con, phoneCategory);
      }

      if (idPhoneCategory == 0)
        throw new Exception("Could not create phone category!");

      if (idPhoneEntry == 0)
        idPhoneEntry = EntryAddPhoneNumber(con, idPerson, phoneNumber, idPhoneCategory);

      if (idPhoneEntry == 0)
        return AddPhone(idPerson, "", phoneCategory);

      string stm;
      stm = string.Format(
        "UPDATE phone_number " +
        "SET id_phone_category = {0}" +
        "WHERE " +
        "id = {1}",
        idPhoneCategory, idPhoneEntry);
      var cmd = new SQLiteCommand(stm, con);
      var result = cmd.ExecuteNonQuery();
      return result == 1;
    }

    public Boolean EntryUpdatePhoneNumber(
      SQLiteConnection con, long idPerson, long idPhoneEntry, long idPhoneCategory, string phoneNumber, string phoneCategory="")
    {

      if (idPhoneCategory == 0)
        idPhoneCategory = EntryGetPhoneCategoryId(con, phoneCategory);

      if (idPhoneCategory == 0)
        throw new Exception("Could not create phone category!");

      if (idPhoneEntry == 0)
        idPhoneEntry = EntryGetPhoneEntryId(con, idPerson, idPhoneCategory);

      string stm;
      stm = string.Format(
        "UPDATE phone_number " +
        "SET pnumber = {0}" +
        "WHERE " +
        "id = {1}",
        phoneNumber, idPhoneEntry);
      var cmd = new SQLiteCommand(stm, con);
      var result = cmd.ExecuteNonQuery();
      return result == 1;
    }

    private long EntryGetPhoneCategoryId(SQLiteConnection con, string phoneCategory)
    {
      string stm = string.Format(
        "SELECT id " +
        "FROM phone_category " +
        "WHERE " +
        "name = {0}",
        phoneCategory);

      var cmd = new SQLiteCommand(stm, con);

      SQLiteDataReader reader = cmd.ExecuteReader();
      if (!reader.HasRows)
        return 0;
      reader.Read();
      return reader.GetInt64(0);
    }

    private long EntryGetPhoneEntryId(SQLiteConnection con, long idPerson, long idCategory)
    {
      // Todo: this needs categoryId!!!
      string stm = string.Format(
        "SELECT id " +
        "FROM list_phone " +
        "JOIN phone_number ON list_phone.id_phone_number = phone_number.id" +
        "WHERE " +
        "list_phone.id_person = {0} and phone_number.id_phone_category = {1}",
        idPerson, idCategory);

      var cmd = new SQLiteCommand(stm, con);

      SQLiteDataReader reader = cmd.ExecuteReader();
      if (!reader.HasRows)
        return 0;
      reader.Read();
      return reader.GetInt64(0);
    }

    public long EntryGetCityId(SQLiteConnection con, string postalCode, string cityName, long idCountry = 49)
    {
      string stm = string.Format(
        "SELECT id " +
        "FROM city " +
        "WHERE " +
        "city_name = {0} and postal_code = {1} and id_country = {2}",
        cityName, postalCode, idCountry);

      var cmd = new SQLiteCommand(stm, con);

      SQLiteDataReader reader = cmd.ExecuteReader();
      if (!reader.HasRows)
        return 0;
      reader.Read();
      return reader.GetInt64(0);
    }

    public long EntryGetCountryId(SQLiteConnection con, int countryCode, string countryName)
    {
      string stm = string.Format(
        "SELECT id " +
        "FROM country " +
        "WHERE " +
        "country_code = {0} and country_name = {1}",
        countryCode, countryName);

      var cmd = new SQLiteCommand(stm, con);

      SQLiteDataReader reader = cmd.ExecuteReader();
      if (!reader.HasRows)
        return 0;
      reader.Read();
      return reader.GetInt64(0);
    }

    public long EntryGetCityId(SQLiteConnection con, long idAdress)
    {
      string stm = string.Format(
        "SELECT id_city " +
        "FROM adress " +
        "WHERE " +
        "id = {0}",
        idAdress);

      var cmd = new SQLiteCommand(stm, con);

      SQLiteDataReader reader = cmd.ExecuteReader();
      if (!reader.HasRows)
        return 0;
      reader.Read();
      return reader.GetInt64(0);
    }

    public long EntryGetAddressId(SQLiteConnection con, long idPerson)
    {
      string stm = string.Format(
        "SELECT id_address " +
        "FROM person " +
        "WHERE " +
        "id = {0}",
        idPerson);

      var cmd = new SQLiteCommand(stm, con);

      SQLiteDataReader reader = cmd.ExecuteReader();
      if (!reader.HasRows)
        return 0;
      reader.Read();
      return reader.GetInt64(0);
    }

    public long EntryGetStreetId(SQLiteConnection con, long idCity, string streetName)
    {
      string stm = string.Format(
        "select id " +
        "FROM street " +
        "WHERE " +
        "id_city = {0} and name = {1}",
        idCity, streetName);

      var cmd = new SQLiteCommand(stm, con);

      SQLiteDataReader reader = cmd.ExecuteReader();
      if (!reader.HasRows)
        return 0;
      reader.Read();
      return reader.GetInt64(0);
    }


    public ObservableCollection<PersonHandler> GetListPerson()
    {
      var con = new SQLiteConnection(_dbSource);
      con.Open();

      string stm =
        "SELECT person.id, person.name_first, person.name_last, address.id_city, address.id_street, address.house_number " +
        "FROM person " +
        "JOIN address ON address.id = person.id_address " +
        "JOIN city ON city.id = address.id_city " +
        "JOIN street ON street.id = address.id_street " +
        "ORDER BY person.name_first;";

      var cmd = new SQLiteCommand(stm, con);
      SQLiteDataReader reader = cmd.ExecuteReader();
      con.Close();

      var list = new ObservableCollection<PersonHandler>();
      while (reader.HasRows)
      {
        reader.Read();
        list.Add(new PersonHandler() { 
          PId = reader.GetInt64(0), 
          Name = reader.GetString(1), 
          Family = reader.GetString(2),
          City = reader.GetString(3),
          Street = reader.GetString(4),
          StreetNumber = reader.GetString(5)
        });
      }
      return list;
    }

    public void EntryGetPhone(SQLiteConnection con, int id_person)
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

    public void EntryGetPhoneList(SQLiteConnection con, int id_person)
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

    public void EntryGetPictureList(SQLiteConnection con, int id_person)
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
  }
}

/*
      SQLiteConnection con, int id_person,
      string nameFirst, string nameLast,
      string streetName, string houseNumber, string postalCode, string cityName,
 */

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoreLib;

namespace UnitTestCore
{
  /// <summary>
  /// UnitTests for SqlManager
  /// </summary>
  [TestClass]
  public class UnitTestSqlManager
  {
    [TestMethod]
    public void TestMethodSqliteDbCreate()
    {
      try
      {
        SqlHandler db = new SqlHandler(debug: true);
        db.DbInitialize();
      }
      catch (Exception ex)
      {
        Assert.Fail("Exception: " + ex.Message);
      }
    }

    [TestMethod]
    public void TestMethodSqliteDbDelete()
    {
      try
      {
        SqlHandler db = new SqlHandler(debug: true);
        db.DbInitialize();
        db.DbDelete();
      }
      catch (Exception ex)
      {
        Assert.Fail("Exception: " + ex.Message);
      }
    }

    [TestMethod]
    public void TestMethodClear()
    {
      try
      {
        SqlHandler db = new SqlHandler(debug: true);
        db.DbInitialize();
        //TODO: add some entries
        db.DbClear();
        //TODO: test how many entries are there
        db.DbDelete();
      }
      catch (Exception ex)
      {
        Assert.Fail("Exception: " + ex.Message);
      }
    }

    [TestMethod]
    public void TestMethodEntryAddPhone()
    {
      SqlHandler db = new SqlHandler(debug: true);
      db.DbInitialize();
      db.AddPhone(0, "0", ""/*TODO*/);
      Assert.IsTrue(false);
    }

    [TestMethod]
    public void TestMethodEntryAddPicture()
    {
      SqlHandler db = new SqlHandler(debug: true);
      db.DbInitialize();
      db.AddPicture(0, ""/*TODO*/);
      Assert.IsTrue(false);
    }

    [TestMethod]
    public void TestMethodEntryRemove()
    {
      SqlHandler db = new SqlHandler(debug: true);
      db.DbInitialize();
      db.RemovePerson(0/*TODO*/);
      Assert.IsTrue(false);
    }

    [TestMethod]
    public void TestMethodEntryGetAll()
    {
      SqlHandler db = new SqlHandler(debug: true);
      db.DbInitialize();
      db.EntryGetAll();
      Assert.IsTrue(false);
    }

    [TestMethod]
    public void TestMethodEntryModify()
    {
      SqlHandler db = new SqlHandler(debug: true);
      db.DbInitialize();
      db.EntryModify();
      Assert.IsTrue(false);
    }
  }
}

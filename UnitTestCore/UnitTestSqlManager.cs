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
        SqlManager db = new SqlManager(debug: true);
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
        SqlManager db = new SqlManager(debug: true);
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
        SqlManager db = new SqlManager(debug: true);
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
      SqlManager db = new SqlManager(debug: true);
      db.DbInitialize();
      db.AddPhone(0, "0", ""/*TODO*/);
      Assert.IsTrue(false);
    }

    [TestMethod]
    public void TestMethodEntryAddPicture()
    {
      SqlManager db = new SqlManager(debug: true);
      db.DbInitialize();
      db.AddPicture(0, ""/*TODO*/);
      Assert.IsTrue(false);
    }

    [TestMethod]
    public void TestMethodEntryRemove()
    {
      SqlManager db = new SqlManager(debug: true);
      db.DbInitialize();
      db.RemovePerson(0/*TODO*/);
      Assert.IsTrue(false);
    }

    [TestMethod]
    public void TestMethodEntryGetAll()
    {
      SqlManager db = new SqlManager(debug: true);
      db.DbInitialize();
      db.EntryGetAll();
      Assert.IsTrue(false);
    }

    [TestMethod]
    public void TestMethodEntryModify()
    {
      SqlManager db = new SqlManager(debug: true);
      db.DbInitialize();
      db.EntryModify();
      Assert.IsTrue(false);
    }
  }
}

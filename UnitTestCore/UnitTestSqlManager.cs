﻿using System;
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
      SqlManager db = new SqlManager(debug: true);
      db.DbInitialize();
      Assert.IsTrue(false);
    }

    [TestMethod]
    public void TestMethodSqliteDbDelete()
    {
      SqlManager db = new SqlManager(debug: true);
      db.DbInitialize();
      db.DbDelete();
      Assert.IsTrue(false);
    }

    [TestMethod]
    public void TestMethodClear()
    {
      SqlManager db = new SqlManager(debug: true);
      db.DbInitialize();
      db.DbClear();
      Assert.IsTrue(false);
    }

    [TestMethod]
    public void TestMethodEntryAddPhone()
    {
      SqlManager db = new SqlManager(debug: true);
      db.DbInitialize();
      db.EntryAddPhone(0, "0", ""/*TODO*/);
      Assert.IsTrue(false);
    }

    [TestMethod]
    public void TestMethodEntryAddPicture()
    {
      SqlManager db = new SqlManager(debug: true);
      db.DbInitialize();
      db.EntryAddPicture(0, ""/*TODO*/);
      Assert.IsTrue(false);
    }

    [TestMethod]
    public void TestMethodEntryRemove()
    {
      SqlManager db = new SqlManager(debug: true);
      db.DbInitialize();
      db.EntryRemove(0/*TODO*/);
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

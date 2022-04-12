using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AndroidMonitoring.Repositories
{
    public static class DataBaseProvider
    {
        public static SQLiteConnection GetSQLiteConnection()
        {
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "monitoring.db3");
            var db = new SQLiteConnection(dbPath);
            return db;
        }

    }
}
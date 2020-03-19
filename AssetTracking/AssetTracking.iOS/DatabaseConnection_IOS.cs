using SQLite;
using System;
using System.IO;
using AssetTracking.iOS;
[assembly: Xamarin.Forms.Dependency(typeof(DatabaseConnection_IOS))]
namespace AssetTracking.iOS
{
    public class DatabaseConnection_IOS : IDatabaseConnection
    {
        public SQLiteConnection DbConnection()
        {
            //var dbName = "assetTracking.db3";
            //string personalFolder =
            //  System.Environment.
            //  GetFolderPath(Environment.SpecialFolder.Personal);
            //  string libraryFolder =
            //  Path.Combine(personalFolder, "..", "Library");
            //  var path = Path.Combine(libraryFolder, dbName);
             

            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "assetTracking.db3");


            try
            {
                   return new SQLiteConnection(dbPath);
            }
            catch (Exception)
            {

                return new SQLiteConnection("assetTracking.db3");
            }

           
        }
    }
}
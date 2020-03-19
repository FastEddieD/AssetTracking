using SQLite;
using AssetTracking;
using System.IO;
using AssetTracking.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(DatabaseConnection_Android))]
namespace AssetTracking.Droid
{
    public class DatabaseConnection_Android : IDatabaseConnection 
    {
        public SQLiteConnection DbConnection()
        {
            var dbName = "AssetTracking.db3";
            var path = Path.Combine(System.Environment.
              GetFolderPath(System.Environment.
              SpecialFolder.Personal), dbName);
            return new SQLiteConnection(path);
        }
    }
}
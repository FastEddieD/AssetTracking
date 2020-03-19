using System;
using System.Collections.Generic;
using System.Text;

namespace AssetTracking
{
    public interface IDatabaseConnection
    {
        SQLite.SQLiteConnection DbConnection();
    }
}

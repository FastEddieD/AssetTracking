using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace AssetTracking.Model
{
    public class Mode
    {
            [PrimaryKey]
            public int Id { get; set; }
            public string Code { get; set; }
            public string Description { get; set; }
        }
}

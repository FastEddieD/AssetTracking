using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetTracking.Model
{
    public class Plant
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Code { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Telephone { get; set; }
    }
}

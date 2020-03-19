using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetTracking.Model
{
    public class Operator
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Initials { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PIN { get; set; }
        public override string ToString()
        {
            return $"({Id}) {FirstName} {LastName}";
        }
        public string OperatorDescription
        {
            get
            {
                return $"({Id}) - {FirstName} {LastName}";
            }
        }

    }
}

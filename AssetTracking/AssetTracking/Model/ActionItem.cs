using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace AssetTracking.Model
{
    public class ActionItem
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Code { get; set; }
        public string SortCode { get; set; }
        public string Description { get; set; }
        public int DeptId { get; set; }
        public bool ItemRequired { get; set; }

        // This property is used when you bind to a list.  This is what is displayed by default
        public override string ToString()
        {
            return $"({Id}) {Code} {Description}";
        }

        // This is a sample "derived" property.  It only has a get
        public string ItemDescription
        {
            get
            {
                return $"({Id}) Action {Code} {Description}";
            }
        }

    }

}

using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace AssetTracking.Model
{
    public class Dept
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Code { get; set; }
        public int PlantId { get; set; }
        public string DeptName { get; set; }
        public string Description { get; set; }
        public string DeptDescription
        {
            get
            {
                return $"({Id}){Code} {Description}";
            }
        }
    }
}

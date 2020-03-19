using System;
using System.Collections.Generic;
using System.Text;

namespace AssetTracking.Model
{
    public class StationStatus
    {
        public DateTime Date { get; set; }

        public int Id { get; set; }
        public string Code { get; set; }
        public int DeptId { get; set; }
        public int OperatorId { get; set; }
        public int ActionItemId { get; set; }
        public string Description { get; set; }
        public int LastScanId { get; set; }

        public int GridX { get; set; }

        public int GridY { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public string Shape { get; set; }

        public string Color { get; set; }

        public string Status { get; set; }

        public string Item { get; set; }
        public string ActionCode { get; set; }
        public string OperatorInitials { get; set; }
        public string OperatorFirstName { get; set; }
        public string OperatorLastName { get; set; }
        public DateTime StartTime { get; set; }
        public string ActionDescription { get; set; }
        public int ElapsedTime => (int)DateTime.Now.Subtract(StartTime).TotalMinutes;
    }
}

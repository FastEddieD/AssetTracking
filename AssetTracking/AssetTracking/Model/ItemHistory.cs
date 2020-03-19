using System;
using System.Collections.Generic;
using System.Text;

namespace AssetTracking.Model
{
    public class ItemHistory
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool Inprocess =>  EndTime == DateTime.MinValue ? true : false ;
        public int ElapsedTimeSinceCalc => (int)EndTime.Subtract(StartTime).TotalMinutes;
        public int ElapsedTimeSinceStart => (int)DateTime.Now.Subtract(StartTime).TotalMinutes;
        public int ElapsedTime =>  EndTime == DateTime.MinValue ?
            (int)DateTime.Now.Subtract(StartTime).TotalMinutes :
            (int)EndTime.Subtract(StartTime).TotalMinutes;
        public int OperatorId { get; set; }
        public string OperatorInitials { get; set; }
        public string OperatorFirstName { get; set; }
        public string OperatorLastName { get; set; }
        public int ActionItemId { get; set; }
        public string ActionCode { get; set; }
        public string ActionDescription { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int StationId { get; set; }
        public string StationCode { get; set; }
        public string StationDescription { get; set; }

    }
}

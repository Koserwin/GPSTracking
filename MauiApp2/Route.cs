using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp2
{
    public class Route
    {
        string name;
        string waypoints;
        TimeOnly time;
        DateOnly date;
        string startpoint,stoppoint, centerLongitude, centerLatitude;
        float centerLat, centerLon;
        public Route(string name, string waypoints, TimeOnly time, DateOnly date) 
        {
            this.name = name;
            this.waypoints = waypoints;
            this.time = time;
            this.date = date;

            string[] points = this.waypoints.Split(';');
            startpoint = points[0];
            stoppoint = points[points.Length - 1];


            string[] souStart = startpoint.Split(",");
            string[] souStop = stoppoint.Split(",");
            centerLon = float.Parse(souStart[0].Replace('.',',')) + float.Parse(souStop[0].Replace('.', ','));
            centerLat = float.Parse(souStart[1].Replace('.', ',')) + float.Parse(souStop[1].Replace('.', ','));
            centerLat = centerLat / 2;
            centerLon = centerLon / 2;

            centerLongitude = centerLon.ToString().Replace(',', '.');
            centerLatitude = centerLat.ToString().Replace(',', '.');

        }

        public string Name { get { return name; } }
        public string Waypoints { get { return waypoints; } }
        public TimeOnly Time { get { return time; } }
        public DateOnly Date { get { return date; } }
        public string Startpoint { get {  return startpoint; } }
        public string Stoppoint { get {  return stoppoint; } }
        public string CenterLatitude { get { return centerLatitude; } }
        public string CenterLongitude { get {  return centerLongitude; } }
    }
}

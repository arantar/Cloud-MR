using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_MR
{
    class AntennaAngle
    {
        private double[,] CoordinatesInRadians(double[,] coordinatesDeg)
        {
            double[,] coordinatesRad = new double[coordinatesDeg.GetLength(0), coordinatesDeg.GetLength(1)];
            for (int i = 0; i < coordinatesDeg.GetLength(0); i++) {
                for (int j = 0; j < coordinatesDeg.GetLength(1); j++) {
                    coordinatesRad[i, j] = DegreesInRadians(coordinatesDeg[i, j]);
                }
            }
            return coordinatesRad;
        }
        private double DegreesInRadians(double degrees)
        {
            double radians = degrees * Math.PI / 180;
            return radians;
        }
        private double[] DistanceBetweenPoints(double[] latRad, double[] lonRad)
        {
            double[] distance = new double[3];
            distance[0] = Math.Acos(Math.Sin(latRad[0]) * Math.Sin(latRad[1]) +
                Math.Cos(latRad[0]) * Math.Cos(latRad[1]) * Math.Cos(lonRad[0] - lonRad[1]));
            distance[1] = Math.Acos(Math.Sin(latRad[1]) * Math.Sin(latRad[2]) +
                Math.Cos(latRad[1]) * Math.Cos(latRad[2]) * Math.Cos(lonRad[1] - lonRad[2]));
            distance[2] = Math.Acos(Math.Sin(latRad[0]) * Math.Sin(latRad[2]) +
                Math.Cos(latRad[0]) * Math.Cos(latRad[2]) * Math.Cos(lonRad[0] - lonRad[2]));
            return distance;
        }
    }
}

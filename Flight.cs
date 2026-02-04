//==========================================================
// Student Number	: S10260014E
// Student Name	: Heng Jue Wei Tevin
// Partner Name	: Queh Ching Kai Javier
//==========================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PRG_2_ASSG
{
    abstract class Flight: IComparable<Flight>
    {
        public string FlightNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }

        public DateTime ExpectedTime { get; set; }

        public string Status { get; set; }

        public Flight()
        {
            
        }
        public Flight(string flightno, string origin, string dest, DateTime expTime, string status)
        {
            FlightNumber = flightno;
            Origin = origin;
            Destination = dest;
            ExpectedTime = expTime;
            Status = status;
        }

        public double CalculateFees()
        {
            double fees = 0;

            //Departing
            if (Origin == "Singapore (SIN)")
            {
                fees = 800;
            }

            else
            {
                fees = 500;
            }

            return fees;
        }
        public int CompareTo(Flight other)
        {
            return this.ExpectedTime.CompareTo(other.ExpectedTime);
        }
        public override string ToString()
        {
            return "Flight Number: " + FlightNumber + "Origin: " + Origin + "Destination: " + Destination + "Expected Time: " + ExpectedTime + "Status: " + Status;
        }
    }
}

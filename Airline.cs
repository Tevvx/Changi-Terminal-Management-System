//==========================================================
// Student Number	: S10260014E
// Student Name	: Heng Jue Wei Tevin
// Partner Name	: Queh Ching Kai Javier
//==========================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PRG_2_ASSG
{
    class Airline
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public Dictionary<string, Flight> Flights { get; set; } = new Dictionary<string, Flight>();

        public Airline(string name, string code)
        {
            Name = name;
            Code = code;
        }

        public bool AddFlight(Flight flight)
        {

            if (Flights.ContainsKey(flight.FlightNumber))
            {
                return false;
            }
            else
            {
                Flights.Add(flight.FlightNumber, flight);
                return true;
            }

        }

        public double CalculateFees()
        {

            double totalFee = 0;
            foreach (Flight flight in Flights.Values)
            {
                totalFee += flight.CalculateFees();
            }
            return totalFee;

        }

        public bool RemoveFlight(Flight flight)
        {

            if (Flights.ContainsKey(flight.FlightNumber))
            {
                Flights.Remove(flight.FlightNumber);
                return true;
            }
            else
            {
                return false;
            }

        }

        public override string ToString()
        {
            string result = "Airline: " + Name + "Code: " + Code + "Flights: ";
            int count = 1;
            foreach (Flight flight in Flights.Values)
            {
                result += $"{count}" + flight + "   ";
                count++;
            }
            return result;
        }

    }
}
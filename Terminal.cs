//==========================================================
// Student Number	: S10260014E
// Student Name	: Heng Jue Wei Tevin
// Partner Name	: Queh Ching Kai Javier
//==========================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PRG_2_ASSG
{
    class Terminal
    {
        public string TerminalName { get; set; }
        public Dictionary<string, Airline> Airlines { get; set; }

        public Dictionary<string, Flight> Flights { get; set; } 

        public Dictionary<string, BoardingGate> BoardingGates { get; set; } 

        public Dictionary<string, double> GateFees { get; set; } = new Dictionary<string, double>();

        public Terminal(string terminalName, Dictionary<string, Airline> airlines, Dictionary<string, Flight> flights, Dictionary<string, BoardingGate> boardingGates)
        {
            TerminalName = terminalName;
            Airlines = airlines;
            BoardingGates = boardingGates;
        }

        public bool AddAirline(Airline airline)
        {
            if (Airlines.ContainsKey(airline.Name))
            {
                return false;
            }
            else
            {
                Airlines.Add(airline.Name, airline);
                return true;
            }

        }

        public bool AddBoardingGate(BoardingGate boardingGate)
        {
            if (BoardingGates.ContainsKey(boardingGate.GateName))
            {
                return false;
            }
            else
            {
                BoardingGates.Add(boardingGate.GateName, boardingGate);
                return true;
            }
        }

        public Airline GetAirlineFromFlight(Flight flight)
        {
            foreach (Airline airline in Airlines.Values)
            {
                if (airline.Flights.ContainsKey(flight.FlightNumber))
                {
                    return airline;
                }
            }
            return null;
        }

        public void PrintAirlineFees()
        {
            double totalAirlinesFees = 0;
            double totalDiscount = 0;
            Console.WriteLine("Displaying the total fees for each Airline: ");
            foreach (Airline airline in Airlines.Values)
            {
                double totalAmt = 0;
                int count = 0;
                double discount = 0;
                var FlightsForTheDay = airline.Flights.Values.Where(f => f.ExpectedTime.Date == DateTime.Now.Date).ToList(); ;
                double discountIfCountMoreThan5 = 0;
                foreach (Flight dailyFlights in FlightsForTheDay)
                {
                    count++;
                    bool CFFT = false;
                    bool DDJB = false;
                    bool LWTT = false;
                    if (dailyFlights is CFFTFlight)
                    {
                        CFFT = true;
                    }
                    else if (dailyFlights is DDJBFlight)
                    {
                        DDJB = true;
                    }
                    else if (dailyFlights is LWTTFlight)
                    {
                        LWTT = true;
                    }

                    if (count % 3 == 0)
                    {
                        discount += 350;
                    }

                    if (dailyFlights.ExpectedTime < DateTime.Parse("11:00 AM") || dailyFlights.ExpectedTime > DateTime.Parse("9:00 PM"))
                    {

                        discount += 110;
                    }

                    if (dailyFlights.Origin == "Dubai (DXB)" || dailyFlights.Origin == "Bangkok (BKK)" || dailyFlights.Origin == "Tokyo (NRT)")
                    {
                        discount += 25;
                    }

                    if (CFFT == false && DDJB == false && LWTT == false)
                    {
                        discount += 50;
                    }

                    foreach(BoardingGate boardingGate in BoardingGates.Values)
                    {
                        if (!(boardingGate.Flight == (null)))
                        {
                            if (boardingGate.Flight.FlightNumber == dailyFlights.FlightNumber)
                            {
                                totalAmt += boardingGate.CalculateFees();
                            }
                            
                        }
                    }
                    
                } 
                if (count > 5)
                {
                    totalAmt = totalAmt * 0.97;
                    discountIfCountMoreThan5 = totalAmt * 0.03;
                }
                Console.WriteLine($"Total Airline Fees Summary for {airline.Name} for {DateTime.Now:yyyy-MM-dd}");
                Console.WriteLine("--------------------------------------------------------------------------------");
                Console.WriteLine($"Original subtotal: ${totalAmt:0.00}");
                Console.WriteLine($"Discount Applied: -${discount + discountIfCountMoreThan5:0.00}");
                Console.WriteLine($"Final Total: ${totalAmt - discount:0.00}");
                Console.WriteLine("\n-------------------------------------------------------------------------------");
                totalAirlinesFees += totalAmt;
                totalDiscount += discount + discountIfCountMoreThan5;
            }
            Console.WriteLine($"\nTotal Fees for all Airlines: ${totalAirlinesFees:0.00}");
            Console.WriteLine($"Total Discount for all Airlines: ${totalDiscount:0.00}");
            Console.WriteLine($"Percentage of Discounts offered: {totalDiscount / totalAirlinesFees * 100:0.00}%");
        }

        public override string ToString()
        {
            return "Terminal Name: " + TerminalName + "Airlines: " + Airlines.Count + "Boarding Gates: " + BoardingGates.Count + "Flights: " + Flights.Count;
        }

    }
}
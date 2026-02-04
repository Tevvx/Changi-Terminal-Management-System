//==========================================================
// Student Number	: S10260014E
// Student Name	: Heng Jue Wei Tevin
// Partner Name	: Queh Ching Kai Javier
//==========================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG_2_ASSG
{
     class BoardingGate
    {
        public string  GateName { get; set; }
        public bool SupportsCFFT { get; set; }
        public bool SupportsDDJB { get; set; }
        public bool SupportsLWTT { get; set; }

        public Flight Flight { get; set; }

        public BoardingGate(string gateName, bool supportsCFFT, bool supportsDDJB, bool supportsLWTT, Flight flight)
        {
            GateName = gateName;
            SupportsCFFT = supportsCFFT;
            SupportsDDJB = supportsDDJB;
            SupportsLWTT = supportsLWTT;
            Flight = flight;
        }

        public double CalculateFees()
        {
            if (Flight is CFFTFlight cFFTFlight)
            {
                return cFFTFlight.CalculateFees() + 300;
            }
            else if (Flight is DDJBFlight dDJBFlight)
            {
                return dDJBFlight.CalculateFees() + 300;
            }
            else if (Flight is LWTTFlight lWTTFlight)
            {
                return lWTTFlight.CalculateFees() + 300;
            }
            else
            {
                return Flight.CalculateFees() + 300;
            }

        }

        public override string ToString()
        {
            return "Gate Name: " + GateName + "Supports CFFT: " + SupportsCFFT + "Supports DDJB: " + SupportsDDJB + "Supports LWTT: " + SupportsLWTT + "Flight: " + Flight + "Fees: " + CalculateFees();
        }
    }
}

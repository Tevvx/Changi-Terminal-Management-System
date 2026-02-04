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
     class NORMFlight:Flight
    {
        
        public NORMFlight(string flightno, string origin, string dest, DateTime expTime, string status) :base(flightno, origin, dest, expTime, status) { }

        public double CalculateFees()
        {
            return base.CalculateFees();
        }

        public override string ToString()
        {
            return base.ToString() + "Fees: " + CalculateFees();
        }
        
    }
}

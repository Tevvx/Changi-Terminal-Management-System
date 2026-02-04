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
    class DDJBFlight : Flight
    {
        public double RequestFee { get; set; }
        public DDJBFlight(string flightno, string origin, string dest, DateTime expTime, string status) : base(flightno, origin, dest, expTime, status)
        {
            RequestFee = 300;
        }

        public double CalculateFees()
        {
           return base.CalculateFees() + RequestFee;
        }

        public override string ToString()
        {
            return base.ToString() + "Fees: " + CalculateFees();
        }

    }
}

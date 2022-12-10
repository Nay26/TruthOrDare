using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruthOrDare.Models
{
    public class Player
    {
        public string FirstName;
        public string SecondName;
        public int Roll;

        public Player()
        {
            FirstName = string.Empty;
            SecondName = string.Empty;
        }
    }
}

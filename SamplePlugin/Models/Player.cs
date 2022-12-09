using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFSpeedDate.Models
{
    public class Player
    {
        public string FirstName;
        public string SecondName;
        public Gender Gender;
        public bool LikesMale;
        public bool LikesFemale;

        public Player()
        {
            FirstName = string.Empty;
            SecondName = string.Empty;
            Gender = Gender.Female;
        }
    }
}

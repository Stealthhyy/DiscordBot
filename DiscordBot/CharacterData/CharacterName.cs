using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.CharacterData
{
    public class CharacterName
    {
        public CharacterName()
        {
            first = "first";
            last = "last";
        }
        public CharacterName(string firstName, string lastName)
        {
            first = firstName;
            last = lastName;
        }
        public string first { get; set; }
        public string last { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.CharacterData
{
    public class Character
    {
        public Character()
        {
            Name = new CharacterName();
            Race = default;
            Class = default;
            Background = default;
            Level = 1;
        }
        public Character(string firstName, string lastName)
        {
            Name = new CharacterName(firstName, lastName);
            Race = default;
            Class = default;
            Background = default;
            Level = 1;
        }

        public CharacterName Name { get; set; }
        public string Race { get; set; }
        public string Class { get; set; }
        public string Background { get; set; }
        public int Level { get; set; }
    }
}

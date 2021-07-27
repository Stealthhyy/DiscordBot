using DiscordBot.CharacterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class PlayerCharaterData
    {
        public PlayerCharaterData()
        {
            characterData = new Character();
            AttributeData = new Attributes();
        }
        public PlayerCharaterData(string firstName, string lastName)
        {
            characterData = new Character(firstName, lastName);
            AttributeData = new Attributes();
        }
        public Character characterData {get; set;}
        public Attributes AttributeData { get; set; }        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.CharacterData
{
    public class Attribute
    {
        public Attribute()
        {
            this.score = default;
            this.mod = default;
        }
        public Attribute(int score, int mod)
        {
            this.score = score;
            this.mod = mod;
        }
        public int score { get; set; }
        public int mod { get; set; }
    }
    public class Attributes
    {
        public int CurrentHealth { get; set; }
        public int MaxHealth { get; set; }
        public int Proficiency { get; set; }

        public Attribute Strength { get; set; }
        public Attribute Dexterity { get; set; }
        public Attribute Constitution { get; set; }
        public Attribute Intelligence { get; set; }
        public Attribute Wisdom { get; set; }
        public Attribute Charisma { get; set; }

        public int Acrobatics { get; set; }
        public int AnimalHandling { get; set; }
        public int Arcana { get; set; }
        public int Athletics { get; set; }
        public int Deception { get; set; }
        public int History { get; set; }
        public int Insight { get; set; }
        public int Intimidation { get; set; }
        public int Investigation { get; set; }
        public int Medicine { get; set; }
        public int Nature { get; set; }
        public int Perception { get; set; }
        public int Persuasion { get; set; }
        public int Religion { get; set; }
        public int SleightOfHand { get; set; }
        public int Stealth { get; set; }
        public int Survival { get; set; }
    }
}

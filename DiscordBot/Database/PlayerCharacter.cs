using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DiscordBot.Database
{
    public partial class PlayerCharacter
    {
        [Key]
        public long AnswerId { get; set; }
        public string Skill { get; set; }
        public int SkillValue { get; set; }
    }
}

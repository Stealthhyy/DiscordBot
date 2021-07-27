using Discord;
using Discord.Net;
using Discord.WebSocket;
using Discord.Commands;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using DiscordBot.Database;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System.IO;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace DiscordBot.Modules
{
    // for commands to be available, and have the Context passed to them, we must inherit ModuleBase
    public class DnDCommands : ModuleBase
    {
        private readonly DiscordBotEntities _db;
        private List<String> _validColors = new List<String>();
        private readonly IConfiguration _config;

        public DnDCommands(IServiceProvider services)
        {
            // we can pass in the db context via depedency injection
            _db = services.GetRequiredService<DiscordBotEntities>();
            _config = services.GetRequiredService<IConfiguration>();

            _validColors.Add("green");
            _validColors.Add("red");
            _validColors.Add("blue");
        }

        [Command("CreateCharacter")]
        [Alias("createcharacter","cc")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task CreateCharacter(string firstName, string lastName)
        {
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();

            // get user info from the Context
            var user = Context.User;

            var userpath = $"Users\\{user.Username}{user.Discriminator}";
            var filepath = $"{userpath}\\{firstName} {lastName}.json";

            if (($"{firstName}{lastName}").IndexOfAny(Path.GetInvalidFileNameChars()) > 0)
            {
                sb.AppendLine($"{firstName} {lastName} contains illegal characters.");
                sb.AppendLine("Only letters and numbers are allowed!");
                // set embed
                embed.Title = "[DnD] Character Creation Failed!";
                embed.Color = Color.Red;
                embed.Description = sb.ToString();

                // send embed reply
                await ReplyAsync(null, false, embed.Build());
                return;
            }
            //Does exist?
            if (File.Exists(filepath))
            {
                sb.AppendLine($"{firstName} {lastName} already exists!");

                // set embed
                embed.Title = "[DnD] Character Creation Failed!";
                embed.Color = Color.Red;
                embed.Description = sb.ToString();

                // send embed reply
                await ReplyAsync(null, false, embed.Build());
                return;
            }

            Directory.CreateDirectory($"Users\\{user.Username}{user.Discriminator}");

            PlayerCharaterData playerCharaterData = new PlayerCharaterData(firstName, lastName);

            var serializedData = JsonConvert.SerializeObject(playerCharaterData);

            await File.WriteAllTextAsync(filepath, serializedData);

            sb.AppendLine($"{firstName} {lastName} has been brought into this world!");

            //// check to see if the color is valid
            //if (!_validColors.Contains(color.ToLower()))
            //{
            //    sb.AppendLine($"**Sorry, [{user.Username}], you must specify a valid color.**");
            //    sb.AppendLine("Valid colors are:");
            //    sb.AppendLine();
            //    foreach (var validColor in _validColors)
            //    {
            //        sb.AppendLine($"{validColor}");
            //    }
            //    embed.Color = new Color(255, 0, 0);
            //}
            //else
            //{
            //    // add answer/color to table
            //    await _db.AddAsync(new PlayerCharacter
            //    {
            //        Skill = "skill",
            //        SkillValue = 1
            //    }
            //    );
            //    // save changes to database
            //    await _db.SaveChangesAsync();
            //    sb.AppendLine();
            //    sb.AppendLine("**Added answer:**");
            //    sb.AppendLine(answer);
            //    sb.AppendLine();
            //    sb.AppendLine("**With color:**");
            //    sb.AppendLine(color);
            //    embed.Color = new Color(0, 255, 0);
            //}

            // set embed
            embed.Title = "[DnD] Character Creation Successful!";
            embed.Color = Color.Green;
            embed.Description = sb.ToString();

            // send embed reply
            await ReplyAsync(null, false, embed.Build());
        }

        [Command("SelectCharacter")]
        [Alias("sc", "selectcharcter")]
        public async Task SelectCharacter(string firstName, string lastName)
        {
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();

            // get user info from the Context
            var user = Context.User;

            var userpath = $"Users\\{user.Username}{user.Discriminator}";
            var filepath = $"{userpath}\\{firstName} {lastName}.json";

            //Does exist?
            if (!File.Exists(filepath))
            {
                // set embed
                embed.Title = "[DnD] Character Selection Failed!";
                embed.Color = Color.Red;
                embed.Description = sb.ToString();

                // send embed reply
                await ReplyAsync(null, false, embed.Build());
                return;
            }
            //-------------------------------------------
            //--------------READ FROM JSON
            //-------------------------------------------

            var player = JsonConvert.DeserializeObject<PlayerCharaterData>(await File.ReadAllTextAsync(filepath));

            sb.AppendLine($"{player.characterData.Name.first} {player.characterData.Name.last} has been selected!");

            // set embed
            embed.Title = "[DnD] Character Selected!";
            embed.Color = Color.Green;
            embed.Description = sb.ToString();

            // send embed reply
            await ReplyAsync(null, false, embed.Build());
        }

        [Command("ListCharacters")]
        [Alias("listcharacers", "lc")]
        public async Task ListCharacters()
        {
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();

            // get user info from the Context
            var user = Context.User;
            var userpath = $"Users\\{user.Username}{user.Discriminator}";

            DirectoryInfo di = new DirectoryInfo(userpath);
            FileInfo[] files = di.GetFiles("*.json");

            foreach (FileInfo file in files)
            {
                sb.AppendLine((file.Name).Replace(".json",""));
            }

            // set embed
            embed.Title = "[DnD] My Character List:";
            embed.Color = Color.Blue;
            embed.Description = sb.ToString();

            // send embed reply
            await ReplyAsync(null, false, embed.Build());
        }

        [Command("remove")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task RemoveAnswer(int id)
        {
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();

            // get user info from the Context
            var user = Context.User;

            var answers = await _db.PlayerCharacter.ToListAsync();
            var answerToRemove = answers.Where(a => a.AnswerId == id).FirstOrDefault();

            if (answerToRemove != null)
            {
                _db.Remove(answerToRemove);
                await _db.SaveChangesAsync();
                sb.AppendLine($"Removed answer -> [{answerToRemove.Skill}]");
            }
            else
            {
                sb.AppendLine($"Did not find answer with id [**{id}**] in the database");
                sb.AppendLine($"Perhaps use the {_config["prefix"]}list command to list out answers");
            }

            // set embed
            embed.Title = "Eight Ball Answer List";
            embed.Description = sb.ToString();

            // send embed reply
            await ReplyAsync(null, false, embed.Build());
        }

        [Command("8ball")]
        [Alias("ask")]
        public async Task AskEightBall([Remainder] string args = null)
        {
            // I like using StringBuilder to build out the reply
            var sb = new StringBuilder();

            // let's use an embed for this one!
            var embed = new EmbedBuilder();

            // add our possible replies from the database
            var replies = await _db.PlayerCharacter.ToListAsync();

            // add a title                        
            embed.Title = "Welcome to the 8-ball!";

            // we can get lots of information from the Context that is passed into the commands
            // here I'm setting up the preface with the user's name and a comma
            sb.AppendLine($"{Context.User.Username},");
            sb.AppendLine();

            // let's make sure the supplied question isn't null 
            if (args == null)
            {
                // if no question is asked (args are null), reply with the below text
                sb.AppendLine("Sorry, can't answer a question you didn't ask!");
            }
            else
            {
                // if we have a question, let's give an answer!
                // get a random number to index our list with 
                var answer = replies[new Random().Next(replies.Count)];

                // build out our reply with the handy StringBuilder
                sb.AppendLine($"You asked: [**{args}**]...");
                sb.AppendLine();
                //sb.AppendLine($"...your answer is [**{answer.AnswerText}**]");

                embed.WithColor(0, 255, 0);
                //switch (answer.AnswerColor)
                //{
                //    case "red":
                //        {
                //            embed.WithColor(255, 0, 0);
                //            break;
                //        }
                //    case "blue":
                //        {
                //            embed.WithColor(0, 0, 255);
                //            break;
                //        }
                //    case "green":
                //        {
                //            embed.WithColor(0, 255, 0);
                //            break;
                //        }
                //}
            }

            // now we can assign the description of the embed to the contents of the StringBuilder we created
            embed.Description = sb.ToString();

            // this will reply with the embed
            await ReplyAsync(null, false, embed.Build());
        }
    }
}
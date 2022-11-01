using Spectre.Console.Cli;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using k2s.Data;
using k2s.Kube;
using k2s.Cli.Helpers;
using k2s.Models.k8s;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;

namespace k2s.Cli.Commands
{
    public class InfoCommandSettings : CommandSettings { }

    public class InfoCommand : AsyncCommand<InfoCommandSettings>
    {
        private readonly k2sDbContext _ctx;
        private readonly IKubernetesService _kube;

        public InfoCommand(k2sDbContext ctx, IKubernetesService kube) {
            _ctx = ctx;
            _kube = kube;
        }

        public override async Task<int> ExecuteAsync(CommandContext context, InfoCommandSettings settings)
        {
            // details are omitted for brevity
            // the actual implementation parses HTML file and collects bots
            AnsiConsole.Write(new Rows(
                new Text("⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣀⣀⣠⣤⣤⣄⣀⣀"), 
new Text("⠀⠀⠀⠀⠀⠀⠀⣀⡴⠾⠛⠋⠉⠉⠁⠈⠉⠉⠙⠛⠷⢦⣄"),
new Text("⠀⠀⠀⠀⢀⣴⠟⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠻⣦⡀"),
new Text("⠀⠀⠀⣠⠟⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣴⡀⠈⠻⣄"),
new Text("⠀⠀⣰⠏⠀⢀⣴⣄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣴⣿⣿⣿⣦⡀⠹⣆"),
new Text("⠀⢰⡟⢀⣴⣿⣿⣿⣷⡄⠀⠀⠀⢀⣴⣷⣄⠀⣠⡿⠟⡿⠻⢿⡟⠻⣦⣻⡆"),
new Text("⠀⣾⣷⡿⢟⣿⠟⣿⠈⠙⢦⣀⣴⣿⣿⣿⣿⣿⣯⡀⠀⠀⠀⠀⠈⠀⠈⠻⣷"),
new Text("⠀⣿⠋⠀⠜⠁⠀⠈⠀⠀⣰⣿⣿⣿⣿⣿⣿⣿⣿⣿⣦⡀⠀⠀⠀⠀⠀⠀⣿"),
new Text("⠀⢿⡄⠀⠀⠀⠀⠀⣠⣾⡿⣿⣿⢿⡟⢿⣧⠙⣿⠉⠻⢿⣦⡀⠀⠀⠀⢠⣿"),
new Text("⠀⠸⣧⠀⠀⠀⣠⡾⠋⠁⢠⡟⠁⠈⠀⠈⢻⡄⠈⠀⠀⠀⠉⠻⣦⠀⠀⣸⠇"),
new Text("⠀⠀⠹⡆⣠⡾⠋⠀⠀⠀⠊⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠳⣴⡟"),
new Text("⠀⠀⠀⠹⣟⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣰⠏"),
new Text("⠀⠀⠀⠀⠈⠳⣄⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣠⠞⠁"),
new Text("⠀⠀⠀⠀⠀⠀⠈⠙⠶⣤⣄⣀⡀⠀⠀⠀⠀⢀⣀⣠⣤⠶⠋⠁"),
new Text("⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠉⠉⠛⠛⠛⠛⠛⠛⠉⠉")
                ));
            AnsiConsole.Write(
    new FigletText("k2s")
        .LeftAligned()
        .Color(Color.Cyan1));

            AnsiConsole.MarkupLine($"[grey]v{GetAssemblyVersionInfo()}[/]");
            AnsiConsole.MarkupLine($"");

            var curCtx = _kube.GetCurrentContext();
            ErrorHandler<string>.HandleResult(curCtx);

            if (curCtx.isOk())
            {



                Outputs.Success("Current Context", $"{curCtx.Content}");


            }
            else
            {
                Outputs.Error("Current Context", $"NA");


            }

            var curNs = _kube.GetCurrentNameSpace(curCtx.Content);
            ErrorHandler<string>.HandleResult(curNs);

            if (curNs.isOk())
            {


                if (curNs.isSuccess() && !string.IsNullOrEmpty(curNs.Content))
                {
                    Outputs.Success("Current Namespace", $"{curNs.Content}");
                }
                else {
                    Outputs.Warning("Current Namespace", $"NA");

                }


            }
            else
            {
                Outputs.Error("Current Namespace", $"NA");


            }

            var contexts = _kube.GetContexts();
            ErrorHandler<List<ContextModel>>.HandleResult(contexts);

            if (contexts.isOk() && contexts.HasContent())
            {

                Emoji.Remap("spiral_notepad", "🗒️");
                Outputs.Success(":spiral_notepad: Available contexts", $"{contexts.Content.Count}");
                var root = new Tree("");

                foreach (var ctx in contexts.Content) {

                    var ctxNode = root.AddNode($"[cyan]{ctx.Name}[/]");
                }

                // Add some nodes


                AnsiConsole.Write(root);


            }
            else
            {
                Outputs.Error("Available contexts", $"NA");


            }
            Emoji.Remap("big_hearth", "❤️");

            var rule = new Rule("Made with :big_hearth: in [default on green] [/][default on white] [/][default on red] [/]");
            rule.Centered();
            
            AnsiConsole.Write(rule);


            return 0;
        }

        public string GetAssemblyVersion()
        {
            return GetType().Assembly.GetName().Version.ToString();
        }

        public string GetAssemblyVersionInfo()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var informationVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            return informationVersion;
        }
    }
}

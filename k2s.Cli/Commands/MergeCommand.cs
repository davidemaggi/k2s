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
using k2s.Models;

namespace k2s.Cli.Commands
{
    public class MergeCommandSettings : GlobalSettings
    {

        [CommandArgument(0, "<filePath>")]
        public string FilePath { get; set; }

    }

    public class MergeCommand : AsyncCommand<MergeCommandSettings>
    {
        private readonly k2sDbContext _ctx;
        private readonly IKubernetesService _kube;

        public MergeCommand(k2sDbContext ctx, IKubernetesService kube) {
            _ctx = ctx;
            _kube = kube;
        }

        public override async Task<int> ExecuteAsync(CommandContext context, MergeCommandSettings settings)
        {
            // details are omitted for brevity
            // the actual implementation parses HTML file and collects bots
            Statics.setGlobals(settings);

            var setOver = _kube.SetOverrideFile(settings.KubeConfigFile);
            if (!setOver.isSuccess()) { Outputs.Warning("KubeConfig File", $"{setOver.Msg}"); }

            Outputs.Info("Merging",settings.FilePath);
            Outputs.Info("Into",_kube.GetConfigPath());

            var merged =  _kube.MergeKubeConfig(settings.FilePath,true);
            ErrorHandler<MergeResult>.HandleResult(merged);

            if (merged.isOk() && merged.HasContent())
            {

                var root = new Tree("");

                // Add some nodes
                var outAdded = root.AddNode("[green]Added[/]");

                foreach (var item in merged.Content.Added) {
                    var tmp = outAdded.AddNode(item);
                }


                var outEdit = root.AddNode("[yellow]Modified[/]");
                foreach (var item in merged.Content.Modified)
                {
                    var tmp = outEdit.AddNode(item);
                }
                var outDeleted = root.AddNode("[red]Deleted[/]");

                foreach (var item in merged.Content.Deleted)
                {
                    var tmp = outDeleted.AddNode(item);
                }

                // Render the tree
                AnsiConsole.Write(root);






                if (AnsiConsole.Confirm("Confirm Merge?"))
                {
                    _kube.SetConfig(merged.Content.Merged, true);
                }
                else {
                    Outputs.Warning("Merge", "Aborted");
                }

               



            }
            else {
                Outputs.Error("Available contexts", $"NA");


            }

            

            return 0;
        }
    }
}

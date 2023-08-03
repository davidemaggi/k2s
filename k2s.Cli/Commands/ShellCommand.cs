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

namespace k2s.Cli.Commands
{
    public class ShellCommandSettings : GlobalSettings { }

    public class ShellCommand : AsyncCommand<ShellCommandSettings>
    {
        private readonly k2sDbContext _ctx;
        private readonly IKubernetesService _kube;

        public ShellCommand(k2sDbContext ctx, IKubernetesService kube) {
            _ctx = ctx;
            _kube = kube;
        }

        public override async Task<int> ExecuteAsync(CommandContext context, ShellCommandSettings settings)
        {
            // details are omitted for brevity
            // the actual implementation parses HTML file and collects bots

            Statics.setGlobals(settings);

            var setOver = _kube.SetOverrideFile(settings.KubeConfigFile);
            if (!setOver.isSuccess()) { Outputs.Warning("KubeConfig File", $"{setOver.Msg}"); }


            var fwdCtx = _kube.GetCurrentContext().Content;

            // Echo the fruit back to the terminal

            Outputs.Success("Shell Connection for Context", fwdCtx);

            var namespaces = await _kube.GetNamespaces(fwdCtx);
            
            ErrorHandler<List<NamespaceModel>>.HandleResult(namespaces);
            var fwdNs = AnsiConsole.Prompt(
   new SelectionPrompt<string>()
       .Title("Which [green]Namespace[/]?")
       .PageSize(10)
       .MoreChoicesText("[grey](Move up and down to reveal more coontext)[/]")
       .AddChoices(namespaces.Content.Select(x => x.Name)));

            Outputs.Success("Selected Namespace", fwdNs);

            

            var icon = ":fwd_pod:";

            Emoji.Remap("fwd_pod", "📦");
            

            Outputs.Info("Connecting to ", $"{icon} Pod");

           

                var pods = await _kube.GetPods(fwdCtx, fwdNs);

                ErrorHandler<List<PodModel>>.HandleResult(pods);

                var podfwd = AnsiConsole.Prompt(
   new SelectionPrompt<string>()
       .Title("Which [green]Pod[/] you want to connect to?")
       .AddChoices(pods.Content.Select(x=>x.Name)));

                Outputs.Success("Selected Pod", podfwd);

                var tmpPod = pods.Content.Where(x => x.Name == podfwd).FirstOrDefault();


                var tmpFwd=await _kube.ShellConnectPod(fwdCtx, fwdNs, podfwd);
                ErrorHandler.HandleResult(tmpFwd);
           

            return 0;
        }
    }
}

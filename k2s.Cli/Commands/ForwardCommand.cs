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
    public class ForwardCommandSettings : GlobalSettings { }

    public class ForwardCommand : AsyncCommand<ForwardCommandSettings>
    {
        private readonly k2sDbContext _ctx;
        private readonly IKubernetesService _kube;

        public ForwardCommand(k2sDbContext ctx, IKubernetesService kube) {
            _ctx = ctx;
            _kube = kube;
        }

        public override async Task<int> ExecuteAsync(CommandContext context, ForwardCommandSettings settings)
        {
            // details are omitted for brevity
            // the actual implementation parses HTML file and collects bots

            Statics.setGlobals(settings);

            var setOver = _kube.SetOverrideFile(settings.KubeConfigFile);
            if (!setOver.isSuccess()) { Outputs.Warning("KubeConfig File", $"{setOver.Msg}"); }


            var fwdCtx = _kube.GetCurrentContext().Content;

            // Echo the fruit back to the terminal

            Outputs.Success("Forward for Context", fwdCtx);

            var namespaces = await _kube.GetNamespaces(fwdCtx);
            
            ErrorHandler<List<NamespaceModel>>.HandleResult(namespaces);
            var fwdNs = AnsiConsole.Prompt(
   new SelectionPrompt<string>()
       .Title("Which [green]Namespace[/]?")
       .PageSize(10)
       .MoreChoicesText("[grey](Move up and down to reveal more coontext)[/]")
       .AddChoices(namespaces.Content.Select(x => x.Name)));

            Outputs.Success("Selected Namespace", fwdNs);

            var entityType = AnsiConsole.Prompt(
   new SelectionPrompt<string>()
       .Title("Which [green]Entyty[/] you want to forward?")
       .AddChoices(new[] {
            "Pod", "Service"
        }));

            var icon = entityType == "Pod" ? ":fwd_pod:" : ":fwd_service:";

            Emoji.Remap("fwd_pod", "📦");
            Emoji.Remap("fwd_service", "🌐");

            Outputs.Info("Forwarding", $"{icon} {entityType}");

            if (entityType == "Pod") {

                var pods = await _kube.GetPods(fwdCtx, fwdNs);

                ErrorHandler<List<PodModel>>.HandleResult(pods);

                var podfwd = AnsiConsole.Prompt(
   new SelectionPrompt<string>()
       .Title("Which [green]Pod[/] you want to forward?")
       .AddChoices(pods.Content.Select(x=>x.Name)));

                Outputs.Success("Selected Pod", podfwd);

                var tmpPod = pods.Content.Where(x => x.Name == podfwd).FirstOrDefault();



                var port = AnsiConsole.Prompt(
   new SelectionPrompt<string>()
       .Title("Which [green]Service[/] you want to forward?")
       .AddChoices(tmpPod.Ports.Select(x => $"{x.ExternalPort} ({x.Protocol})")));
                Outputs.Info("Forwarding Port", $"{port}");

                var localport = AnsiConsole.Ask<int>("On local [green]port[/]:");

                //Outputs.Success("On Local Port", localport.ToString());
                var tmpFwd=await _kube.PortForwardPod(fwdCtx, fwdNs, podfwd, port, localport);
                ErrorHandler.HandleResult(tmpFwd);
            }
            else {

                var services = await _kube.GetServices(fwdCtx, fwdNs);

                ErrorHandler<List<ServiceModel>>.HandleResult(services);

                var servicefwd = AnsiConsole.Prompt(
   new SelectionPrompt<string>()
       .Title("Which [green]Service[/] you want to forward?")
       .AddChoices(services.Content.Select(x => x.Name)));

                Outputs.Success("Selected Service", servicefwd);

                var tmpSvc = services.Content.Where(x=>x.Name==servicefwd).FirstOrDefault();



                var port = AnsiConsole.Prompt(
   new SelectionPrompt<string>()
       .Title("Which [green]Port[/] you want to forward?")
       .AddChoices(tmpSvc.Ports.Select(x => $"{x.ExternalPort} -> {x.InternalPort} ({x.Protocol})")));

                Outputs.Info("Forwarding Port", $"{port}");

                var localport = AnsiConsole.Ask<int>("On local [green]port[/]:");

                // Outputs.Success("On Local Port", localport.ToString());

                var tmpFwd=await _kube.PortForwardService(fwdCtx,fwdNs,servicefwd,port,localport);
                ErrorHandler.HandleResult(tmpFwd);
            }


            return 0;
        }
    }
}

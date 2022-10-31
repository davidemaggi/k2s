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
    public class WizardCommandSettings : CommandSettings { }

    public class WizardCommand : AsyncCommand<WizardCommandSettings>
    {
        private readonly k2sDbContext _ctx;
        private readonly IKubernetesService _kube;

        public WizardCommand(k2sDbContext ctx, IKubernetesService kube) {
            _ctx = ctx;
            _kube = kube;
        }

        public override async Task<int> ExecuteAsync(CommandContext context, WizardCommandSettings settings)
        {
            // details are omitted for brevity
            // the actual implementation parses HTML file and collects bots

            

            var contexts =  _kube.GetContexts();
            ErrorHandler<List<ContextModel>>.HandleResult(contexts);

            var newCtx = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title("Which [green]Context[/]?")
        .PageSize(10)
        .MoreChoicesText("[grey](Move up and down to reveal more coontext)[/]")
        .AddChoices(contexts.Content.Select(x=>x.Name)));

            // Echo the fruit back to the terminal

            Outputs.Success("Selected Context", newCtx);

            var namespaces = await _kube.GetNamespaces(newCtx);
            
            ErrorHandler<List<NamespaceModel>>.HandleResult(namespaces);
            var newNs = AnsiConsole.Prompt(
   new SelectionPrompt<string>()
       .Title("Which [green]Namespace[/]?")
       .PageSize(10)
       .MoreChoicesText("[grey](Move up and down to reveal more Namespaces)[/]")
       .AddChoices(namespaces.Content.Select(x => x.Name)));


            Outputs.Success("Selected Namespace", newNs);


            var setCtx = _kube.SetContext(newCtx);

            ErrorHandler.HandleResult(setCtx);


            var setNs = _kube.SetNameSpace(newCtx,newNs);
            ErrorHandler.HandleResult(setNs);


            var save = _kube.SaveKubeConfig(true);
            ErrorHandler.HandleResult(save);

            

            return 0;
        }
    }
}

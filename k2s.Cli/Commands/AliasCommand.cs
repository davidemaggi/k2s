using k2s.Cli.Helpers;
using k2s.Data;
using k2s.Kube;
using k2s.Models.k8s;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace k2s.Cli.Commands
{
    public class AliasCommandSettings : CommandSettings { }
    public class AliasCommand : AsyncCommand<AliasCommandSettings>
    {

        private readonly k2sDbContext _ctx;
        private readonly IKubernetesService _kube;

        public AliasCommand(k2sDbContext ctx, IKubernetesService kube)
        {
            _ctx = ctx;
            _kube = kube;
        }
        public override async Task<int> ExecuteAsync(CommandContext context, AliasCommandSettings settings)
        {

            var contexts = _kube.GetContexts();
            ErrorHandler<List<ContextModel>>.HandleResult(contexts);

            var renameCtx = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title("Which context you want to [green]rename[/]?")
        .PageSize(10)
        .MoreChoicesText("[grey](Move up and down to reveal more coontexts)[/]")
        .AddChoices(contexts.Content.Select(x => x.Name)));

            // Echo the fruit back to the terminal

            Outputs.Success("Selected Context", renameCtx);

            var newName = AnsiConsole.Ask<string>("What's the new [green]name[/]?");

            _kube.RenameContext(renameCtx, newName);


            return 0;
        }

        }
}

﻿using k2s.Cli.Helpers;
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
    public class DeleteCommandSettings : GlobalSettings { }
    public class DeleteCommand : AsyncCommand<DeleteCommandSettings>
    {

        private readonly k2sDbContext _ctx;
        private readonly IKubernetesService _kube;

        public DeleteCommand(k2sDbContext ctx, IKubernetesService kube)
        {
            _ctx = ctx;
            _kube = kube;
        }
        public override async Task<int> ExecuteAsync(CommandContext context, DeleteCommandSettings settings)
        {
            Statics.setGlobals(settings);

            var setOver = _kube.SetOverrideFile(settings.KubeConfigFile);
            if (!setOver.isSuccess()) { Outputs.Warning("KubeConfig File", $"{setOver.Msg}"); }

            var contexts = _kube.GetContexts();
            ErrorHandler<List<ContextModel>>.HandleResult(contexts);

            var deleteCtx = AnsiConsole.Prompt(
    new MultiSelectionPrompt<string>()
        .Title("Which context you want to [red]delete[/]?")
        .PageSize(10)
        .MoreChoicesText("[grey](Move up and down to reveal more coontexts)[/]")
        .AddChoices(contexts.Content.Select(x => x.Name)));

            // Echo the fruit back to the terminal

            //Outputs.Success("Selected Context", deleteCtx);

            

            if (AnsiConsole.Confirm($"Are you sure ou want to delete [red]{string.Join(", ", deleteCtx)}[/]?"))
            {
                var resDel=_kube.DeleteContexts(deleteCtx);
                ErrorHandler.HandleResult(resDel);
            }







            return 1;
        }

        }
}

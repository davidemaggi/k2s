using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace k2s.Cli
{
    public  class GlobalSettings: CommandSettings
    {
        [CommandOption("-f|--file")]
        [DefaultValue("")]
        [Description("The kubeconfig to use")]
        public string? KubeConfigFile { get; set; }

        [CommandOption("-v|--verbose")]
        [Description("Do you want some logs?")]

        public bool? Verbose { get; set; }

        [CommandOption("-k|--kubectl")]
        [Description("Don't do anything, just give me a kubectl command")]

        public bool? KubeCtl { get; set; }

    }
}

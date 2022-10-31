using k2s.Cli.Commands;
using k2s.Data;
using k2s.Kube;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using Spectre.Console.Cli;

namespace k2s.Cli
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                System.Console.OutputEncoding = System.Text.Encoding.UTF8;

                var services = new ServiceCollection();

                ConfigureServices(services);

                var app = new CommandApp(new TypeRegistrar(services));

                app.SetDefaultCommand<WizardCommand>();

                app.Configure(config =>
                {
                    config.SetApplicationName("k2s");

                    config.AddCommand<WizardCommand>("wizard")
                    .WithAlias("w")
                    .WithDescription("Set your new Context and Namespace");

                    config.AddCommand<MergeCommand>("merge")
                   .WithAlias("m")
                   .WithDescription("Merge new YAML file with your KubeConfig");

                    config.AddCommand<ForwardCommand>("forward")
                   .WithAlias("f")
                   .WithAlias("fwd")
                   .WithDescription("Forward a service/pod port to localhost");
                    config.AddCommand<AliasCommand>("alias")
                .WithAlias("a")
                .WithDescription("Change Context Name to an alias");

                    config.AddCommand<AliasCommand>("delete")
                   .WithAlias("d")
                   .WithDescription("Delete context from KubeConfig");

                    config.AddCommand<InfoCommand>("info")
                  .WithAlias("i")
                  .WithDescription("Get info about the current configuration");




#if DEBUG
                    config.PropagateExceptions();
                    config.ValidateExamples();
#endif
                });


                app.Run(args);
            }
            catch (Exception e) { 
            
            
                Console.WriteLine(e.ToString());
            
            }

        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkSqlite().AddDbContext<k2sDbContext>();


            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSingleton<IKubernetesService, KubernetesService>();


        }

    }
}
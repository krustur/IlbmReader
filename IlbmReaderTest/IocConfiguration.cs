using Autofac;
using AutofacSerilogIntegration;
using Serilog;

namespace IlbmReaderTest
{
    public class IocConfiguration
    {
        public static IContainer Configure(ILogger logger)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<MainForm>();
            builder.RegisterType<IlbmReader>();
            builder.RegisterType<IlbmForm>();
            builder.RegisterLogger(logger);
            var container = builder.Build();
            return container;
        }
    }
}
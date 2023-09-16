using Journal.Application.DI;
using Journal.Application.DI.Container;
using Journal.Application.Extensions;
using Journal.Application.Panels.Abstractions;
using Journal.DataAccess.DI;

ConsoleCancelEventHandler HandleGracefulShutdown = (_, _) =>
{
    Console.CursorVisible = true;
    Console.WriteLine("Goodbye!".ToColor(ConsoleColor.Cyan));
    Environment.Exit(0);
};

Console.CursorVisible = false;
Console.CancelKeyPress += HandleGracefulShutdown;

DIContainer container = new();
container.RegisterDataAccessTypes();
container.RegisterApplicationTypes();
IPanelController panelController = container.Resolve<IPanelController>();

Console.Clear();
panelController.Loop();
using Journal.Application.DI;
using Journal.Application.DI.Container;
using Journal.Application.Panels.Abstractions;
using Journal.DataAccess.DI;
System.Console.WriteLine(ConsoleColor.Cyan);
DIContainer container = new();
container.RegisterDataAccessTypes();
container.RegisterApplicationTypes();

IPanelController panelController = container.Resolve<IPanelController>();
panelController.Loop();
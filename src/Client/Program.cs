// See https://aka.ms/new-console-template for more information
using Journal.Application.DI;
using Journal.Application.DI.Container;
using Journal.Application.Panels.Abstractions;

DIContainer container = new();
container.RegisterApplicationTypes();

IPanelController panelController = container.Resolve<IPanelController>();
panelController.Loop();
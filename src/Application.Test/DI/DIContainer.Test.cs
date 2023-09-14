using Journal.Application.DI.Container;

namespace Application.Test.DI;

interface ITest
{
    IList<string> List { get; }
};
class Test : ITest
{
    public IList<string> List { get; }
    public Test(IList<string> list)
    {
        List = list;
    }
}

[TestClass]
public class DIContainerTest
{
    private DIContainer container = null!;
    [TestInitialize]
    public void Initialize()
    {
        container = new DIContainer();
    }

    [TestMethod]
    public void Should_Register_Transient()
    {
        container.RegisterTransient<IList<string>, List<string>>();
        var list1 = container.Resolve<IList<string>>();
        var list2 = container.Resolve<IList<string>>();
        Assert.AreNotEqual(list1, list2);
        Assert.IsInstanceOfType(list1, typeof(List<string>));
        Assert.IsInstanceOfType(list2, typeof(List<string>));
    }

    [TestMethod]
    public void Should_Register_Singleton()
    {
        container.RegisterSingleton<IList<string>, List<string>>();
        var list1 = container.Resolve<IList<string>>();
        var list2 = container.Resolve<IList<string>>();
        Assert.AreEqual(list1, list2);
        Assert.IsInstanceOfType(list1, typeof(List<string>));
    }

    [TestMethod]
    public void Should_Register_Singleton_With_Instance()
    {
        var list = new List<string>() { "test" };
        container.RegisterSingleton<IList<string>>(list);
        var resolvedList = container.Resolve<IList<string>>();
        Assert.AreEqual(list, resolvedList);
    }

    [TestMethod]
    public void Should_Resolve_Recursive_Types()
    {
        var list = new List<string>() { "test" };
        container.RegisterSingleton<IList<string>>(list);
        container.RegisterTransient<ITest, Test>();
        var resolvedClass = container.Resolve<ITest>();
        Assert.AreEqual(list, resolvedClass.List);
    }
}
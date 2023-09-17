using Journal.Application.Common.Utilities;
using Journal.Application.Extensions;
using Journal.Application.Tags.Utilities;
using Journal.Domain.Models;

namespace Application.Test.Tags.Utilities;

[TestClass]
public class TagUtilitiesTest
{
    private const string ANSI_RED = "\u001b[31m";
    private const string ANSI_BLUE = "\u001b[34m";
    private const string ANSI_GRAY = "\u001b[90m";

    [TestInitialize]
    public void BeforeEach()
    {
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.White;
    }

    [TestMethod]
    public void Should_Get_Tag_String_With_Correct_Color()
    {
        var tag = new EntryTag("Test", ConsoleColor.Red);
        string tagString = TagUtilities.GetTagString(tag);
        string expectedString = "Test".ToColor(ConsoleColor.Red);

        Assert.AreEqual(tagString, expectedString);
    }

    [TestMethod]
    public void Should_Format_Tags_Correctly_When_There_Are_Less_Than_Three_Tags()
    {
        var tags = new EntryTag[]
        {
            new("Test1", ConsoleColor.Red),
            new("Test2", ConsoleColor.Blue)
        };
        string formattedTags = TagUtilities.GetFormattedTags(tags);
        string expectedString = $"{"Test1".ToColor(ConsoleColor.Red)}, {"Test2".ToColor(ConsoleColor.Blue)}";

        Assert.AreEqual(formattedTags, expectedString);
    }

    [TestMethod]
    public void Should_Format_Tags_Correctly_When_There_Are_More_Than_Three_Tags()
    {
        var tags = new EntryTag[]
        {
            new("Test1", ConsoleColor.Red),
            new("Test2", ConsoleColor.Blue),
            new("Test3", ConsoleColor.Gray),
            new("Test4", ConsoleColor.Green)
        };
        string formattedTags = TagUtilities.GetFormattedTags(tags);
        string expectedString =
            $"{"Test1".ToColor(ConsoleColor.Red)}, {"Test2".ToColor(ConsoleColor.Blue)}, {"Test3".ToColor(ConsoleColor.Gray)}, and 1 more";

        Assert.AreEqual(formattedTags, expectedString);
    }
}
using Journal.Domain.Models;

namespace Journal.Domain.Test.Domain;

[TestClass]
public class JournalEntryTest
{
    [TestMethod]
    public void Should_Copy_With_Updated_Name_And_Dates()
    {
        JournalEntry entry = new("Test", "Test Content", new List<EntryTag>())
        {
            CreatedAt = new DateTime(2021, 1, 1),
            UpdatedAt = new DateTime(2021, 1, 1)
        };

        JournalEntry updatedEntry = entry.Copy();
        Assert.AreEqual(updatedEntry.Title, "Test (Copy)");
        Assert.AreEqual(updatedEntry.CreatedAt.Day, DateTime.Now.Day);
        Assert.AreEqual(updatedEntry.UpdatedAt.Day, DateTime.Now.Day);
    }
}
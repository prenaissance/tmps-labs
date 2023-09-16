using Journal.Application.Extensions;
using Journal.Domain.Models;

namespace Journal.Application.Tags.Utilities;

public static class TagUtilities
{
    public static string GetTagString(EntryTag tag)
    {
        return $"{tag.Name.ToColor(ConsoleColor.Cyan)}";
    }
}
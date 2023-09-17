using System.Text;
using Journal.Application.Common.Utilities;
using Journal.Application.Extensions;
using Journal.Domain.Models;

namespace Journal.Application.Tags.Utilities;

public static class TagUtilities
{
    public static string GetTagString(EntryTag tag)
    {
        return $"{tag.Name.ToColor(tag.Color)}";
    }

    public static string GetFormattedTags(IList<EntryTag> tags)
    {
        StringBuilder sb = new();
        var first3TagsString = string.Join(", ", tags.Take(3).Select(GetTagString));
        sb.Append(first3TagsString);
        if (tags.Count > 3)
        {
            sb.Append($", and {tags.Count - 3} more");
        }

        return sb.ToString();
    }
}
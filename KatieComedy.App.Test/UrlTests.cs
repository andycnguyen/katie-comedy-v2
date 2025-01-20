using System.Text.RegularExpressions;

namespace KatieComedy.App.Test;

public partial class UrlTests
{
    [Theory]
    [InlineData("https://nytimes.com", true)]
    [InlineData("nytimes.com", true)]
    [InlineData("https://nytimes.com/news/article", true)]
    [InlineData("https://nytimes.com/news/article-abc-123", true)]
    [InlineData("https://nytimes.com/news/article/", true)]
    [InlineData("https://nytimes.com/news/article/story/scoop", true)]
    [InlineData("https://nytimes.", false)]
    [InlineData("htp://nytimes.com", false)]
    [InlineData("://nytimes.", false)]
    public void Foo(string input, bool isValid)
    {
        var match = UrlRegex().Match(input);
        Assert.Equal(isValid, match.Success);
    }

    [GeneratedRegex(Constants.UrlPattern)]
    private static partial Regex UrlRegex();
}

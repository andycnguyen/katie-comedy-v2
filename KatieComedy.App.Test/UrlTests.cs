﻿using System.Text.RegularExpressions;

namespace KatieComedy.App.Test;

public partial class UrlTests
{
    [Theory]
    [InlineData("https://nytimes.com", true)]
    [InlineData("http://nytimes.com", true)]
    [InlineData("https://subdomain.nytimes.com/news/article", true)]
    [InlineData("https://nytimes.com/news/article", true)]
    [InlineData("https://nytimes.com/news/article-abc-123", true)]
    [InlineData("https://nytimes.com/news/article/", true)]
    [InlineData("https://nytimes.com/news/article/story/scoop", true)]
    [InlineData("nytimes", false)]
    [InlineData("https://nytimes.", false)]
    [InlineData("htp://nytimes.com", false)]
    [InlineData("://nytimes.", false)]
    [InlineData("https://app.arts-people.com/index.php?show=267862", true)]
    [InlineData("https://www.youtube.com/watch?v=s0_U-B1f538", true)]
    public void UrlRegex_WhenMatchesInput_ValidatesUrl(string input, bool isValid)
    {
        var match = UrlRegex().Match(input);
        Assert.Equal(isValid, match.Success);
    }

    [GeneratedRegex(Constants.UrlPattern)]
    private static partial Regex UrlRegex();
}

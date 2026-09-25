using AccessDoor.Core;

namespace AccessDoor.Core.Tests;

public class LoginRequestTests
{
    [Fact]
    public void ToUri_EscapesQueryValues()
    {
        var uri = new LoginRequest("http://host:130/", "a b&c", "k=1", "abc").ToUri();
        Assert.Equal("http://host:130/?username=a%20b%26c&key=k%3D1&token=abc", uri.AbsoluteUri);
    }

    [Theory]
    [InlineData("", "u", "k")]
    [InlineData("ftp://x", "u", "k")]
    [InlineData("http://x", "", "k")]
    [InlineData("http://x", "u", " ")]
    public void Validate_RejectsBadInput(string url, string user, string key) =>
        Assert.NotNull(new LoginRequest(url, user, key, "t").Validate());

    [Fact]
    public void Validate_AcceptsGoodInput() =>
        Assert.Null(new LoginRequest("https://x/", "u", "k", "t").Validate());
}

public class LoginResultTests
{
    [Fact]
    public void TryParse_ReadsMessageAndFlag()
    {
        var result = LoginResult.TryParse("http://x/?msg=Access%20denied&IsAllow=false");
        Assert.Equal(new LoginResult(false, "Access denied"), result);
    }

    [Fact]
    public void TryParse_AllowedIsCaseInsensitive() =>
        Assert.True(LoginResult.TryParse("http://x/?IsAllow=True&msg=ok")!.IsAllowed);

    [Fact]
    public void TryParse_MissingIsAllow_IsDenied() =>
        Assert.False(LoginResult.TryParse("http://x/?msg=hi")!.IsAllowed);

    [Theory]
    [InlineData(null)]
    [InlineData("http://x/home")]
    [InlineData("http://x/?page=2")]
    public void TryParse_NoResult_ReturnsNull(string? url) => Assert.Null(LoginResult.TryParse(url));
}

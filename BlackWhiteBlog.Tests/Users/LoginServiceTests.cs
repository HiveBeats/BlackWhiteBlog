using BlackWhiteBlog.Services;
using Xunit;

public class LoginServiceTests
{
    [Fact]
    public void Hash_Password_Exists()
    {
        var pass = "ndt5bm#gY";
        
        var hashedPass = LoginService.HashPassword(pass);
        
        Assert.NotNull(hashedPass);
    }

    [Fact]
    public void Hashed_Pass_CanCheck()
    {
        var pass = "ndt5bm#gY";
        var hashedPass = LoginService.HashPassword(pass);

        var isValid = LoginService.CheckPassword(hashedPass, pass);

        Assert.True(isValid);
    }

    [Fact]
    public void Invalid_Pass_Cant_Check()
    {
        var pass = "ndt5bm#gY";
        var hashedPass = LoginService.HashPassword(pass);

        var isValid = LoginService.CheckPassword(hashedPass, "ndt5bm#fY");

        Assert.False(isValid);
    }

    [Fact]
    public void Can_Get_Token()
    {
        var userName = "JohnGrave";

        var token = LoginService.GetToken(userName);

        Assert.NotNull(token);
    }
}
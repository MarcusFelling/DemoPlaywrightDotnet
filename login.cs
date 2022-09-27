using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace PlaywrightTests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class Tests : PageTest
{
    // Use parameters from .runsettings file
    public static string BaseURL;
    public static string UserName;
    public static string Password;

    [OneTimeSetUp]
    public void Init()
    {
        BaseURL = TestContext.Parameters["BaseURL"]
                ?? throw new Exception("BaseURL is not configured as a parameter.");
        UserName = TestContext.Parameters["UserName"]
                ?? throw new Exception("UserName is not configured as a parameter.");
        Password = TestContext.Parameters["Password"]
                ?? throw new Exception("Password is not configured as a parameter.");
    }

    [Test]
    public async Task ShouldLogIn()
    {
        // Go to login page
        await Page.GotoAsync(BaseURL);

        // Fill user name and password
        await Page.Locator("[data-test=\"username\"]").ClickAsync();
        await Page.Locator("[data-test=\"username\"]").FillAsync(UserName);
        await Page.Locator("[data-test=\"password\"]").ClickAsync();
        await Page.Locator("[data-test=\"password\"]").FillAsync(Password);
        
        // Click login button
        await Page.Locator("[data-test=\"login-button\"]").ClickAsync();

        // Expect the inventory page to be shown
        await Expect(Page).ToHaveURLAsync(new Regex(".*inventory"));
    }
}
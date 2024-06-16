using WpfPilot;

namespace UiTests;

public class ExampleTests
{
    private const string _appPath = @"WpfAutomationPOC.exe";

    [Fact]
    public void Add_value_to_total()
    {
        using var app = AppDriver.Launch(_appPath);
        using var record = AppDriver.Record(@"media\add.mp4");

        app.GetElement(x => x.TypeName == "TextBox")
            .Type("22");

        app.GetElement(x => x.TypeName == "Button")
            .Click();

        app.Screenshot(@"media\add.png");

        var total = app.GetElement(x => x["AutomationProperties.AutomationId"] == "Total");
        Assert.Equal("Total: 22", total["Text"]);
    }
}

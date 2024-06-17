using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Capturing;
using FlaUI.Core.Definitions;
using FlaUI.UIA3;
using Assert = Xunit.Assert;

namespace FlaUI;

public class FlaUiTests : IDisposable
{
    private Application _app;
    private UIA3Automation _automation;
    private const string _appPath = @"WpfAutomationPOC.exe";

    public FlaUiTests()
    {
        _app = Application.Launch(_appPath);
        _automation = new UIA3Automation();
    }

    public void Dispose()
    {
        _app.Close();
        _app.Dispose();
        _automation.Dispose();
    }

    [Fact]
    public void FlaUI_Tests()
    {
        using var recorder = StartVideoRecorder(@"media\add.mp4");

        var window = _app.GetMainWindow(_automation);

        window
            .FindFirstChild(x => x.ByControlType(ControlType.Edit))
            .AsTextBox()
            .Enter("22");

        window
            .FindFirstChild(x => x.ByControlType(ControlType.Button))
            .AsButton()
            .Invoke();

        Capture.MainScreen().ToFile(@"media\add.png");

        var total = window.FindFirstDescendant(x => x.ByAutomationId("Total")).AsLabel().Text;
        Assert.Equal("Total: 22", total);
    }

    private static async Task<VideoRecorder> StartVideoRecorder(string videoName)
    {
        var videoRecorderSettings = new VideoRecorderSettings
        {
            VideoFormat = VideoFormat.xvid,
            VideoQuality = 6,
            TargetVideoPath = videoName,
            FrameRate = 24,
            ffmpegPath = await VideoRecorder.DownloadFFMpeg("C:\\temp")
        };

        return new VideoRecorder(videoRecorderSettings, _ => Capture.MainScreen());
    }
}

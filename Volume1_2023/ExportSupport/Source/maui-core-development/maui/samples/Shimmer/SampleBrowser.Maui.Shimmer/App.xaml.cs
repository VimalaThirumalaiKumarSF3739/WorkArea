using System.Reflection;

namespace SampleBrowser.Maui.Shimmer;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

        var appInfo = typeof(App).GetTypeInfo().Assembly;
        SampleBrowser.Maui.Base.BaseConfig.IsIndividualSB = true;
        MainPage = SampleBrowser.Maui.Base.BaseConfig.MainPageInit(appInfo);
    }
}

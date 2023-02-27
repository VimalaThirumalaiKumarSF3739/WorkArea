namespace WindowOverlay.Samples;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

        //MainPage = new TopOverlayPage();

        //MainPage = new OverlayWithTabbedPage();

        MainPage = new ShellHomePage();

        //MainPage = new NavigationPage(new HomePage());

        //MainPage = new NavigationPage(new AbsoluteOverlayPage());

        //MainPage = new ContentPage() { Content = new UnconstrainedListViewOverlay() };
    }
}

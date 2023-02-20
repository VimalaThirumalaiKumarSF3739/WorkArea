using Syncfusion.Maui.Core;

namespace SaveAsImageSample;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
        //chart control
        //Default jpg format
        chart.SaveAsImage("Test");

        //To save image in jpg format
        //chart.SaveAsImage("Test.jpg");

        //To save image in png format
        //chart.SaveAsImage("Test.png");
    }
}


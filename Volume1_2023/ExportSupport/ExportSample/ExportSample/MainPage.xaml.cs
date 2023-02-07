using Syncfusion.Maui.Core;

namespace ExportSample;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
        BindingContext = new ViewModel();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        //chart control 
        Stream stream = await chart.GetStreamAsync(ImageFileFormat.Png);
        image.Source = ImageSource.FromStream(() => stream);
    }
}


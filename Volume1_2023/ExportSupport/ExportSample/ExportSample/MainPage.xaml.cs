using Syncfusion.Maui.Core.Internals;

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
        //chart control - PNG file format
        Stream stream = await chart.GetStreamAsync(Syncfusion.Maui.Core.ImageFileFormat.Png);
        image.Source = ImageSource.FromStream(() => stream);

        //chart control - JPEG file format
        //Stream stream = await chart.GetStreamAsync(Syncfusion.Maui.Core.ImageFileFormat.Jpeg);
        //image.Source = ImageSource.FromStream(() => stream);
    }
}


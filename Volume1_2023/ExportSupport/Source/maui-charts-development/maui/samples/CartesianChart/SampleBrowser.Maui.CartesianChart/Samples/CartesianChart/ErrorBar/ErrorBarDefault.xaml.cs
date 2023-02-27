#region Copyright Syncfusion Inc. 2001-2022.
// Copyright Syncfusion Inc. 2001-2022. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using SampleBrowser.Maui.Base;
using Syncfusion.Maui.Charts;
using System.Collections.ObjectModel;

namespace SampleBrowser.Maui.CartesianChart.SfCartesianChart
{
    public partial class ErrorBarDefault : SampleView
    {

        #region Constructor

        #region  Public Constructor

        public ErrorBarDefault()
        {
            InitializeComponent();
        }
        public override void OnAppearing()
        {
            base.OnAppearing();
            hyperLinkLayout.IsVisible = !IsCardView;
            if (!IsCardView)
            {
                var label = new Label()
                {
                    Text = "Thermal Expansion of Metals",
                    TextColor = Colors.Black,
                    FontSize = 16,
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.Center,
                    Padding = new Thickness(0, 0, 0, 20),
                    LineBreakMode = LineBreakMode.WordWrap,
                    HorizontalTextAlignment = TextAlignment.Center,
                };

                errorBarChart.Title = label;
            }

            if(IsCardView)
            {
                xAxisTitle.TextColor = Colors.Transparent;
                yAxisTitle.TextColor = Colors.Transparent;
            }

        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            errorBarChart.Handler?.DisconnectHandler();
        }

        #endregion

        #endregion

    }
}

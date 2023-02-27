using Microsoft.Maui.Controls;
using Syncfusion.Maui.Core;

namespace Syncfusion.Maui.Shimmer
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SfShimmer"/> class.
    /// Represents a view used to achieve shimmer custom view. 
    /// </summary>
    public class ShimmerView : SfView
    {
        /// <summary>
        /// Identifies the <see cref="ShapeType"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ShapeType"/> bindable property.
        /// </value>
        public static readonly BindableProperty ShapeTypeProperty =
            BindableProperty.Create(nameof(ShapeType), typeof(ShimmerShapeType), typeof(ShimmerView), ShimmerShapeType.Rectangle);

        /// <summary>
        /// Gets or sets the shape for the shimmer view. 
        /// </summary>
        /// <value>The default value of <see cref="ShapeType"/> is <see cref="ShimmerShapeType.Rectangle"/>.</value>
        /// <example>
        /// The following code demonstrates, how to use the ShapeType property in the Shimmer.
        /// # [XAML](#tab/tabid-1)
        /// <code Lang="XAML"><![CDATA[
        /// <shimmer:SfShimmer x:Name="Shimmer" 
        ///                    Type="Custom">
        ///     <shimmer:SfShimmer.CustomView>
        ///         <Grid HeightRequest="50" WidthRequest="200">
        ///             <Grid.RowDefinitions>
        ///                 <RowDefinition/>
        ///                 <RowDefinition/>
        ///             </Grid.RowDefinitions>
        ///             <Grid.ColumnDefinitions>
        ///                 <ColumnDefinition Width="0.25*"/>
        ///                 <ColumnDefinition Width="0.75*"/>
        ///             </Grid.ColumnDefinitions>
        /// 
        ///             <shimmer:ShimmerView ShapeType="Circle" Grid.RowSpan="2"/>
        ///             <shimmer:ShimmerView Grid.Column="1" Margin="5"/>
        ///             <shimmer:ShimmerView ShapeType="RoundedRectangle" Grid.Row="1" Grid.Column="1" Margin="5"/>
        ///         </Grid>
        ///     </shimmer:SfShimmer.CustomView>
        /// </shimmer:SfShimmer>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code Lang="C#"><![CDATA[
        /// Grid grid = new Grid
        /// {
        ///     HeightRequest = 50, 
        ///     WidthRequest = 200,
        ///     RowDefinitions =
        ///     {
        ///         new RowDefinition(),
        ///         new RowDefinition(),
        ///     },
        ///     ColumnDefinitions =
        ///     {
        ///         new ColumnDefinition { Width = new GridLength(0.25, GridUnitType.Star) },
        ///         new ColumnDefinition { Width = new GridLength(0.75, GridUnitType.Star) }
        ///     }
        /// };
        /// 
        /// ShimmerView circleView = new ShimmerView() { ShapeType = ShimmerShapeType.Circle};
        /// grid.SetRowSpan(circleView, 2);
        /// grid.Add(circleView);
        /// grid.Add(new ShimmerView { Margin = 5 }, 1);
        /// grid.Add(new ShimmerView { Margin = 5, ShapeType = ShimmerShapeType.RoundedRectangle }, 1, 1);
        /// Shimmer.Type = ShimmerType.Custom;
        /// Shimmer.CustomView = grid;
        /// ]]></code>
        /// </example>
        public ShimmerShapeType ShapeType
        {
            get { return (ShimmerShapeType)GetValue(ShapeTypeProperty); }
            set { SetValue(ShapeTypeProperty, value); }
        }
    }
}
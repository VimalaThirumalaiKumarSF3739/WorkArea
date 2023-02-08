
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Core.Internals;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Core
{
    /// <summary>
    /// 
    /// </summary>
    internal class SfLegend : SfView, ILegend
    {
        #region Fields

        private const double maxSize = 8388607.5;

#if MACCATALYST || IOS
        // Adding default value for item margin to set maximum height and width for ColletionView in iOS and mac OS only .
        private const double itemMargin = 40;
#endif
        private readonly CollectionView legendView;

#endregion

#region Bindable Properties

#region Public Bindable Properties

        /// <summary>
        /// Gets or sets the items source for the legend. This is a bindable property.
        /// </summary>
        public static readonly BindableProperty ItemsSourceProperty =
           BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(SfLegend), null, BindingMode.Default, null);

        /// <summary>
        /// The DependencyProperty for <see cref="ToggleVisibility"/> property.
        /// </summary>
        public static readonly BindableProperty ToggleVisibilityProperty = BindableProperty.Create(nameof(ToggleVisibility), typeof(bool), typeof(SfLegend), false, BindingMode.Default, null, OnToggleVisibilityChanged, null, null);

#endregion

#region Internal Bindable Properties
        /// <summary>
        /// Gets or sets a data template for all series legend item. This is a bindable property.
        /// </summary>
        internal static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(SfLegend), null, BindingMode.Default, null, OnItemTemplateChanged, null, null);

        /// <summary>
        ///  Gets or sets placement of the legend. This is a bindable property.
        /// </summary>
        internal static readonly BindableProperty PlacementProperty = BindableProperty.Create(nameof(Placement), typeof(LegendPlacement), typeof(SfLegend), LegendPlacement.Top, BindingMode.Default, null, OnPlacementChanged, null, null);

        /// <summary>
        /// Gets or sets the orientation of the legend items. This is a bindable property.
        /// </summary>
        internal static readonly BindableProperty ItemsLayoutProperty = BindableProperty.Create(nameof(ItemsLayout), typeof(IItemsLayout), typeof(SfLegend), null, BindingMode.Default, null, OnOrientationChanged, null, null);

#endregion

#endregion

#region Public Properties

        /// <summary>
        ///  Gets or sets the ItemsSource for the legend.
        /// </summary>
        /// <remarks>The default will be of <see cref="LegendItem"/> type.</remarks>
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to bind the series visibility with its corresponding legend item in the legend. This is bindable property.
        /// </summary>
        public bool ToggleVisibility
        {
            get { return (bool)GetValue(ToggleVisibilityProperty); }
            set { SetValue(ToggleVisibilityProperty, value); }
        }

#endregion

#region Internal Properties

        /// <summary>
        /// Gets or sets the data template for legend item.
        /// </summary>
        /// <value>This property takes the DataTemplate value.</value>
        internal DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets placement of the legend. This is a bindable property.
        /// </summary>
        internal LegendPlacement Placement
        {
            get { return (LegendPlacement)GetValue(PlacementProperty); }
            set { SetValue(PlacementProperty, value); }
        }

        /// <summary>
        /// Gets or sets the orientation of the legend items. This is a bindable property.
        /// </summary>
        /// <value>This property takes <see cref="IItemsLayout"/> as its value.</value>
        internal IItemsLayout ItemsLayout
        {
            get { return (IItemsLayout)GetValue(ItemsLayoutProperty); }
            set { SetValue(ItemsLayoutProperty, value); }
        }
        LegendPlacement ILegend.Placement { get => Placement; set { } }

        IItemsLayout ILegend.ItemsLayout { get => ItemsLayout; set { } }

        DataTemplate ILegend.ItemTemplate { get => ItemTemplate; set { } }

        bool ILegend.IsVisible { get; set; }

        internal event EventHandler<LegendItemClickedEventArgs>? ItemClicked;

#endregion

#region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfLegend"/> class.
        /// </summary>
        public SfLegend()
        {
            legendView = new CollectionView();
            
#if ANDROID || WINDOWS
            legendView.HorizontalOptions = LayoutOptions.Center;
            legendView.VerticalOptions = LayoutOptions.Center;
#endif

            legendView.BindingContext = this;
            legendView.SetBinding(CollectionView.ItemsSourceProperty, nameof(ItemsSource));
            legendView.SelectionMode = SelectionMode.None;
            legendView.HorizontalScrollBarVisibility = ScrollBarVisibility.Never;
            legendView.VerticalScrollBarVisibility = ScrollBarVisibility.Never;
            legendView.ItemsLayout = LinearItemsLayout.Horizontal;

#if WINDOWS
            //Todo : Remove the method once the collection view item minimum width value was reduced . <see href="https://github.com/dotnet/maui/issues/4520">HERE</see>
            legendView.HandlerChanged += CollectionView_HandlerChanged;
#endif

            UpdateLegendTemplate();
            base.Children.Add(legendView);
        }

#endregion

#region Methods

#region Public Methods
       
        /// <summary>
        /// Method used to get the legend rect size.
        /// </summary>
        /// <param name="legend"></param>
        /// <param name="availableSize"></param>
        /// <param name="maxSizePercentage"></param>
        /// <returns></returns>
        public static Rect GetLegendRectangle(SfLegend legend, Rect availableSize, double maxSizePercentage)
        {
            if (legend != null)
            {
                Size size = Size.Zero;
                double maxSize = 8388607.5;
                double position = 0;
                double maxWidth = 0;

#if MACCATALYST || IOS
                // Todo : Need to remove the method when collection view issue in ios and mac OS is fixed Github Link <see href="https://github.com/dotnet/maui/issues/9135">HERE</see>
                size = legend.MeasureSize();
#endif
                switch (legend.Placement)
                {
                    case LegendPlacement.Top:

#if WINDOWS || ANDROID
                        size = legend.Measure(availableSize.Width, double.PositiveInfinity, MeasureFlags.IncludeMargins);
#endif

                        maxWidth = availableSize.Height * maxSizePercentage < size.Height ? availableSize.Height * maxSizePercentage : size.Height;
                        position = (availableSize.Height != maxSize) ? availableSize.Height - maxWidth : 0;
                        return new Rect(availableSize.X, availableSize.Y, availableSize.Width, maxWidth);

                    case LegendPlacement.Bottom:

#if WINDOWS || ANDROID
                        size = legend.Measure(availableSize.Width, double.PositiveInfinity, MeasureFlags.IncludeMargins);
#endif

                        maxWidth = availableSize.Height * maxSizePercentage < size.Height ? availableSize.Height * maxSizePercentage : size.Height;
                        position = (availableSize.Height != maxSize) ? availableSize.Height - maxWidth : 0;
                        return new Rect(availableSize.X, availableSize.Y + position, availableSize.Width, maxWidth);

                    case LegendPlacement.Left:

#if WINDOWS || ANDROID
                        size = legend.Measure(double.PositiveInfinity, availableSize.Height, MeasureFlags.IncludeMargins);
#endif

                        maxWidth = availableSize.Width * maxSizePercentage < size.Width ? availableSize.Width * maxSizePercentage : size.Width;
                        return new Rect(availableSize.X, availableSize.Y, maxWidth, availableSize.Height);

                    case LegendPlacement.Right:

#if WINDOWS || ANDROID
                        size = legend.Measure(double.PositiveInfinity, availableSize.Height, MeasureFlags.IncludeMargins);
#endif

                        maxWidth = availableSize.Width * maxSizePercentage < size.Width ? availableSize.Width * maxSizePercentage : size.Width;
                        position = (availableSize.Width != maxSize) ? availableSize.Width - maxWidth : 0;
                        return new Rect(availableSize.X + position, availableSize.Y, maxWidth, availableSize.Height);
                }
            }

            return Rect.Zero;
        }

#if MACCATALYST || IOS
        public Size MeasureSize()
        {
            if (legendView.ItemTemplate.CreateContent() is View view && Handler?.MauiContext != null)
            {
                nfloat width = 0;
                nfloat height = 0;
                var handler = Microsoft.Maui.Platform.ViewExtensions.ToHandler(view, Handler.MauiContext);

                if (handler.PlatformView is UIKit.UIView nativeView)
                {
                    // Calculating height and width of the items to solve space issue in mac OS and iOS platforms.
                    foreach (var item in ItemsSource)
                    {
                        view.BindingContext = item;
                        var size = nativeView.SizeThatFits(new CoreGraphics.CGSize(double.PositiveInfinity, double.PositiveInfinity));

                        if (Placement == LegendPlacement.Left || Placement == LegendPlacement.Right)
                        {
                            height += size.Height;
                            width = width > size.Width ? width : size.Width;
                        }
                        else
                        {
                            width += size.Width;
                            height = height > size.Height ? height : size.Height;
                        }
                    }
                }

                legendView.VerticalOptions = LayoutOptions.Center;
                HorizontalOptions = LayoutOptions.Center;
                legendView.MaximumHeightRequest = height + itemMargin;
                legendView.MaximumWidthRequest = width + itemMargin;
            }

            return Measure(double.PositiveInfinity, double.PositiveInfinity, MeasureFlags.IncludeMargins);
        }
#endif
#endregion

#region Protected Methods

        /// <summary>
        /// Creates the <see cref="SfShapeView"/> type for the default legend icon.
        /// </summary>
        /// <returns>Returns the ShapeView.</returns>
        protected virtual SfShapeView CreateShapeView()
        {
            return new SfShapeView();
        }

        /// <summary>
        /// Creates the <see cref="Label"/> type for the default legend text.
        /// </summary>
        /// <returns>Returns the Label.</returns>
        /// <remarks>Method must return label type and should not return any null value.</remarks>
        protected virtual Label CreateLabelView()
        {
            return new Label();
        }

#endregion

#region Private Call backs

        private static void OnItemTemplateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var legend = bindable as SfLegend;
            if (legend != null)
            {
                legend.OnItemTemplateChanged(oldValue, newValue);
            }
        }

        private static void OnPlacementChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var legend = bindable as SfLegend;
            if (legend != null)
            {
                legend.UpdateLegendPlacement();
            }
        }

        private static void OnToggleVisibilityChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }
        private static void OnOrientationChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var legend = bindable as SfLegend;
            if (legend != null)
            {
                legend.legendView.ItemsLayout = newValue as LinearItemsLayout;
            }
        }
        #endregion

#region Private Methods

#if WINDOWS
        private void CollectionView_HandlerChanged(object? sender, EventArgs e)
        {
            if (sender is CollectionView collectionVirtualView)
            {
                var collectionViewHandler = collectionVirtualView.Handler;
                if (collectionViewHandler != null && collectionViewHandler.PlatformView is Microsoft.UI.Xaml.Controls.ListView listView)
                {
                    listView.ItemContainerStyle?.Setters.Add(new Microsoft.UI.Xaml.Setter(Microsoft.UI.Xaml.Controls.ListViewItem.MinWidthProperty, 5));
                    listView.ItemContainerTransitions = null;
                }
            }
        }
#endif

        internal void UpdateLegendPlacement()
        {
            if (ItemsLayout == null)
            {
                if (Placement == LegendPlacement.Top || Placement == LegendPlacement.Bottom)
                {
                    ItemsLayout = LinearItemsLayout.Horizontal;
                }
                else
                {
                    ItemsLayout = LinearItemsLayout.Vertical;
                }
                legendView.ItemsLayout = ItemsLayout;
            }
        }
        public void LegendTappedAction(LegendItem legendItem)
        {
            if (ToggleVisibility && legendItem != null)
            {
                legendItem.IsToggled = !legendItem.IsToggled;

                if (ItemClicked != null)
                {
                    ItemClicked(this, new LegendItemClickedEventArgs() { LegendItem = legendItem });
                }
            }
        }
        /// <summary>
        /// Metod used to get the default legend template.
        /// </summary>
        /// <returns></returns>
        private DataTemplate GetDefaultLegendTemplate()
        {
            var template = new DataTemplate(() =>
            {
                HorizontalStackLayout stack = new HorizontalStackLayout()
                {
                    Spacing = 6,
                    Padding = new Thickness(8, 10)
                };

                ToggleColorConverter toggleColorConverter = new ToggleColorConverter();
                Binding binding;
                Binding binding1;
                MultiBinding multiBinding;

                SfShapeView shapeView = CreateShapeView();
                if (shapeView != null)
                {
                    shapeView.HorizontalOptions = LayoutOptions.Start;
                    shapeView.VerticalOptions = LayoutOptions.Center;
                    binding = new Binding(nameof(LegendItem.IsToggled));
                    binding.Converter = toggleColorConverter;
                    binding.ConverterParameter = shapeView;
                    binding1 = new Binding(nameof(LegendItem.IconBrush));
                    multiBinding = new MultiBinding()
                    {
                        Bindings = new List<BindingBase>() { binding, binding1 },
                        Converter = new MultiBindingIconBrushConverter(),
                        ConverterParameter = shapeView
                    };

                    shapeView.SetBinding(SfShapeView.IconBrushProperty, multiBinding);
                    shapeView.SetBinding(SfShapeView.ShapeTypeProperty, nameof(LegendItem.IconType));
                    shapeView.SetBinding(SfShapeView.HeightRequestProperty, nameof(LegendItem.IconHeight));
                    shapeView.SetBinding(SfShapeView.WidthRequestProperty, nameof(LegendItem.IconWidth));
                    stack.Children.Add(shapeView);
                }

                Label label = CreateLabelView();
                if (label != null)
                {
                    label.VerticalTextAlignment = TextAlignment.Center;
                    label.SetBinding(Label.TextProperty, nameof(LegendItem.Text));
                    binding = new Binding(nameof(LegendItem.IsToggled));
                    binding.Converter = toggleColorConverter;
                    binding.ConverterParameter = label;
                    label.SetBinding(Label.TextColorProperty, binding);
                    label.SetBinding(Label.MarginProperty, nameof(LegendItem.TextMargin));
                    label.SetBinding(Label.FontSizeProperty, nameof(LegendItem.FontSize));
                    label.SetBinding(Label.FontFamilyProperty, nameof(LegendItem.FontFamily));
                    label.SetBinding(Label.FontAttributesProperty, nameof(LegendItem.FontAttributes));
                    stack.Children.Add(label);
                }
                return stack;
            });
            return template;
        }

        //Todo : Need to change the Data Template tap gesture recognizer to collection view selection when selection issue in mac OS and iOS solved . 
        private void UpdateLegendTemplate()
        {
            var template = new DataTemplate(() =>
            {
                LegendItemView views = new LegendItemView(LegendTappedAction);
                views.ItemTemplate = GetDefaultLegendTemplate();
                return views;
            });
            legendView.ItemTemplate = template;
        }

        private void OnItemTemplateChanged(object oldValue, object newValue)
        {
            if (Equals(oldValue, newValue))
            {
                return;
            }

            if (legendView != null)
            {
                if (newValue != null && newValue is DataTemplate dataTemplate)
                {
                    var template = new DataTemplate(() =>
                    {
                        LegendItemView views = new LegendItemView(LegendTappedAction);
                        views.ItemTemplate = dataTemplate;
                        return views;
                    });
                    legendView.ItemTemplate = template;
                }
                else
                {
                    UpdateLegendTemplate();
                }
            }
        }
#endregion

#endregion
    }

    /// <summary>
    /// 
    /// </summary>
    internal class LegendItemView : ContentView, ITapGestureListener
    {
        readonly Action<LegendItem> legendAction;
        /// <summary>
        /// 
        /// </summary>
        internal static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(LegendItemView), default(DataTemplate), BindingMode.Default, null, OnItemTemplateChanged, null, null);

        /// <summary>
        /// 
        /// </summary>
        internal DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public LegendItemView(Action<LegendItem> action)
        {
            legendAction = action;
            this.AddGestureListener(this);
        }

        private static void OnItemTemplateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var template = bindable as LegendItemView;
            if (template != null)
            {
                template.OnItemTemplateChanged(oldValue, newValue);
            }
        }
        private void OnItemTemplateChanged(object oldValue, object newValue)
        {
            if (newValue != null && newValue is DataTemplate template)
            {
                base.Content = (View)template.CreateContent();
            }
        }

        public void OnTap(TapEventArgs e)
        {
            if (legendAction != null && BindingContext is LegendItem legendItem)
            {
                legendAction.Invoke(legendItem);
            }
        }
    }
}

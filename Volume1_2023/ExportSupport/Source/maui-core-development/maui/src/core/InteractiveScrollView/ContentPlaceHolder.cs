using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core.Internals
{
    internal class ContentPlaceHolder : AbsoluteLayout
    {
        PanZoomListener m_panZoomListener;

        internal ContentPlaceHolder(PanZoomListener panZoomListener)
        {
            m_panZoomListener = panZoomListener;
            this.ChildAdded += OnChildAdded;
            this.ChildRemoved += OnChildRemoved;
        }

        private void OnChildRemoved(object? sender, ElementEventArgs e)
        {
            e.Element.PropertyChanged -= OnChildPropertyChanged;
        }

        private void OnChildPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(HorizontalOptions):
                case nameof(VerticalOptions):
                    LayoutContent();
                    break;
            }
        }

        private void OnChildAdded(object? sender, ElementEventArgs e)
        {
            e.Element.PropertyChanged += OnChildPropertyChanged;
        }

        internal void AddZoomGestures()
        {
            if (m_panZoomListener != null)
            {
                this.AddGestureListener(m_panZoomListener);
                this.AddKeyboardListener(m_panZoomListener);
                this.AddTouchListener(m_panZoomListener);
            }
        }

        internal void RemoveZoomGestures()
        {
            this.ClearKeyboardListeners();
            this.ClearTouchListeners();
            this.ClearGestureListeners();
        }

        double GetAlignmentProportion(LayoutAlignment layoutAlignment)
        {
            switch (layoutAlignment)
            {
                case LayoutAlignment.Start:
                    return 0;
                case LayoutAlignment.End:
                    return 1;
                default:
                    return 0.5;
            }
        }

        internal void LayoutContent()
        {
            if (this.Children.Count > 0 && this.Children[0] is View content)
            {
                this.SetLayoutFlags(content, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.PositionProportional);
                double horizontalAlignmentProportion = GetAlignmentProportion(content.HorizontalOptions.Alignment);
                double verticalAlignmentProportion = GetAlignmentProportion(content.VerticalOptions.Alignment);
#if IOS || MACCATALYST
                // The following workaround line of code (for iOS and MAC alone) is added due to the existing content alignment issue in .NET MAUI (MAC) with RTL flow direction. GitHub link https://github.com/dotnet/maui/issues/9970
                if (this.FlowDirection == FlowDirection.RightToLeft)
                    horizontalAlignmentProportion = 1 - horizontalAlignmentProportion;
#endif
                this.SetLayoutBounds(content, new Rect(horizontalAlignmentProportion, verticalAlignmentProportion, AutoSize, AutoSize));
            }
        }

        internal void RequestSize(double width, double height)
        {
            this.WidthRequest = width;
            this.HeightRequest = height;
        }

        /// <summary>
        /// Clears the children and releases the used resources.
        /// </summary>
        internal void Unload()
        {
            RemoveZoomGestures();
            if (Children.Count > 0)
                Children.Clear();
        }

        ~ContentPlaceHolder()
        {
            this.ChildAdded -= OnChildAdded;
            this.ChildRemoved -= OnChildRemoved;
        }

        internal void Reset()
        {
            if (Children.Count > 0)
                this.Children.Clear();
            RequestSize(-1, -1);
        }
    }
}
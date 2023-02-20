using System.Collections.Generic;
using System.Reflection;
using System.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Shapes;
using Windows.Foundation;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class XySeriesDraggingBase : XyDataSeries
    {
        #region Dependency Property Registration
        
        /// <summary>
        /// The DependencyProperty for <see cref="EnableSeriesDragging"/> property.       .
        /// </summary>
        internal static readonly DependencyProperty EnableSeriesDraggingProperty =
            DependencyProperty.Register(
                "EnableSeriesDragging",
                typeof(bool),
                typeof(XySeriesDraggingBase),
                new PropertyMetadata(false, OnEnableDraggingChanged));

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether to enable the series dragging. We can drag the series, if its <c>true</c>.
        /// </summary>
        internal bool EnableSeriesDragging
        {
            get { return (bool)GetValue(EnableSeriesDraggingProperty); }
            set { SetValue(EnableSeriesDraggingProperty, value); }
        }

        #endregion

        #region Internal Properties

        internal Ellipse? DraggingPointIndicator { get; set; }

        internal UIElement? PreviewSeries { get; set; }

        internal ChartSegment? DraggingSegment { get; set; }

        #endregion

        #endregion

        #region Methods

        #region Internal Virtual Methods

        internal virtual void UpdatePreivewSeriesDragging(Point mousePos)
        {
        }

        internal virtual void UpdatePreviewSegmentDragging(Point mousePos)
        {
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Method used to update the underlying model.
        /// </summary>
        /// <param name="path">path</param>
        /// <param name="updatedDatas">updated datas</param>
        internal void UpdateUnderLayingModel(string path, IList<double> updatedDatas)
        {
            if(ItemsSource is IEnumerable itemsSource)
            {
                var enumerator = itemsSource.GetEnumerator();
                PropertyInfo? yPropertyInfo;

                if (enumerator.MoveNext())
                {
                    yPropertyInfo = ChartDataUtils.GetPropertyInfo(enumerator.Current, path);
                    IPropertyAccessor? yPropertyAccessor = null;
                    if (yPropertyInfo != null)
                        yPropertyAccessor = FastReflectionCaches.PropertyAccessorCache.Get(yPropertyInfo);
                    int i = 0;
                    do
                    {
                        yPropertyAccessor?.SetValue(enumerator.Current, updatedDatas[i]);
                        i++;
                    }
                    while (enumerator.MoveNext());
                }
            }
        }

        #endregion

        #region Private Static Methods

        private static void OnEnableDraggingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //if ((bool)e.NewValue == false)
            //    ((XySeriesDraggingBase)d).ResetDraggingElements("OnPropertyChanged", false);
        }

        #endregion

        #endregion
    }

}

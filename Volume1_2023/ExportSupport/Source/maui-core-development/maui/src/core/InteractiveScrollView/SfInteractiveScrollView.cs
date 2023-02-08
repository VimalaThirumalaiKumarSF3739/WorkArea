using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using System;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Core.Internals
{
    /// <summary>
    /// Represents a scroll view with interactive zooming and panning capabilities.
    /// </summary>
    /// <example>
    /// # [Xaml](#tab/tabid-1)
    /// <code><![CDATA[
    /// <syncfusion:SfInteractiveScrollView>
    ///      <StackLayout>
    ///        <Label Text = "For the most wild, yet most homely narrative which I am about to pen, I neither expect nor solicit belief. Mad indeed would I be to expect it, in a case where my very senses reject their own evidence. Yet, mad am I not -- and very surely do I not dream. But to-morrow I die, and to-day I would unburthen my soul. My immediate purpose is to place before the world, plainly, succinctly, and without comment, a series of mere household events. In their consequences, these events have terrified -- have tortured -- have destroyed me. Yet I will not attempt to expound them. To me, they have presented little but Horror -- to many they will seem less terrible than barroques. Hereafter, perhaps, some intellect may be found which will reduce my phantasm to the common-place -- some intellect more calm, more logical, and far less excitable than my own, which will perceive, in the circumstances I detail with awe, nothing more than an ordinary succession of very natural causes and effects." />
    ///        < !--More Label objects go here -->
    ///      </StackLayout>
    /// </syncfusion:SfInteractiveScrollView>
    /// ]]>
    /// </code>
    /// # [C#](#tab/tabid-2)
    /// <code><![CDATA[
    /// SfInteractiveScrollView scrollView = new SfInteractiveScrollView();
    /// StackLayout stackLayout = new StackLayout();
    /// stackLayout.Add(new Label { Text = "FOR the most wild, yet most homely narrative which I am about to pen, I neither expect nor solicit belief. Mad indeed would I be to expect it, in a case where my very senses reject their own evidence. Yet, mad am I not -- and very surely do I not dream. But to-morrow I die, and to-day I would unburthen my soul. My immediate purpose is to place before the world, plainly, succinctly, and without comment, a series of mere household events. In their consequences, these events have terrified -- have tortured -- have destroyed me. Yet I will not attempt to expound them. To me, they have presented little but Horror -- to many they will seem less terrible than barroques. Hereafter, perhaps, some intellect may be found which will reduce my phantasm to the common-place -- some intellect more calm, more logical, and far less excitable than my own, which will perceive, in the circumstances I detail with awe, nothing more than an ordinary succession of very natural causes and effects." });
    /// //More Label objects go here
    /// scrollView.Content = stackLayout;
    /// ]]>
    /// </code>
    /// </example>
    [ContentProperty(nameof(Content))]
    public class SfInteractiveScrollView : View, ITapGestureListener
    {
        #region Variables
        internal PanZoomListener? m_panZoomListener;
        internal Size m_contentSize = Size.Zero;
        TaskCompletionSource<bool>? m_scrollCompletionSource;

        // The below backup values are used to compare with the resultant offset values and avoid repeated scroll requests. 
        double m_currentHorizontalOffset = 0;
        double m_currentVerticalOffset = 0;
        double m_currentHorizontalProportion = 0;
        double m_currentVerticalProportion = 0;

#if WINDOWS
        internal PanGestureManager? m_panGestureManager;
#elif IOS || MACCATALYST
        internal KeyboardGestureManager? m_keyboardGestureManager;
#endif
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="Syncfusion.Maui.Core.Internals.SfInteractiveScrollView"/> class.
        /// </summary>
        public SfInteractiveScrollView()
        {
            InitializeControl();
        }

        void InitializeControl()
        {
            m_panZoomListener = new PanZoomListener();
            m_panZoomListener.PanMode = PanMode.Vertical;
#if WINDOWS
            m_panZoomListener.MouseWheelSettings!.ZoomKeyModifier = KeyboardKey.Ctrl;
            m_panGestureManager = new PanGestureManager(this, m_panZoomListener);
#elif IOS || MACCATALYST
            //Restricting the mouse wheel zoom for MACCATALYST/IOS, due to the framework limitations mentioned by the team in the ticket https://support.syncfusion.com/agent/tickets/387564
            m_panZoomListener.AllowMouseWheelZoom = false;
            m_keyboardGestureManager = new KeyboardGestureManager(this);
            this.AddGestureListener(this);
#endif
            PresentedContent = new ContentPlaceHolder(m_panZoomListener);
            WireEvents();
        }
        #endregion

        #region Events
        /// <summary>
        /// Occurs when the zoom is changed.
        /// </summary>
        public event EventHandler<ZoomEventArgs>? ZoomChanged;

        /// <summary>
        /// Occurs when the zoom is started.
        /// </summary>
        public event EventHandler<ZoomEventArgs>? ZoomStarted;

        /// <summary>
        /// Occurs when the zoom is ended.
        /// </summary>
        public event EventHandler<ZoomEventArgs>? ZoomEnded;

        /// <summary>
        /// Occurs when the content is scrolled.
        /// </summary>
        public event EventHandler<ScrollChangedEventArgs>? ScrollChanged;
        #endregion

        #region Bindable properties
        static readonly BindablePropertyKey ZoomFactorPropertyKey = BindableProperty.CreateReadOnly(nameof(ZoomFactor), typeof(double), typeof(SfInteractiveScrollView), 1.0);
        /// <summary>
        /// Backing store for the <see cref="Syncfusion.Maui.Core.Internals.SfInteractiveScrollView.ZoomFactor"/> property.
        /// </summary>
        /// <remarks>
        /// This property is read-only.
        /// </remarks>
        public static readonly BindableProperty ZoomFactorProperty = ZoomFactorPropertyKey.BindableProperty;

        /// <summary>
        /// Backing store for the <see cref="Syncfusion.Maui.Core.Internals.SfInteractiveScrollView.MinZoomFactor"/> property.
        /// </summary>
        public static readonly BindableProperty MinZoomFactorProperty =
                BindableProperty.Create(nameof(MinZoomFactor), typeof(double), typeof(SfInteractiveScrollView), 0.1, coerceValue: CoerceMinZoom, propertyChanged: OnMinZoomChanged);

        /// <summary>
        /// Backing store for the <see cref="Syncfusion.Maui.Core.Internals.SfInteractiveScrollView.MaxZoomFactor"/> property.
        /// </summary>
        public static readonly BindableProperty MaxZoomFactorProperty =
                BindableProperty.Create(nameof(MaxZoomFactor), typeof(double), typeof(SfInteractiveScrollView), 10.0, coerceValue: CoerceMaxZoom, propertyChanged: OnMaxZoomChanged);

        private static readonly BindableProperty CanBecomeFirstResponderProperty =
                BindableProperty.Create(nameof(CanBecomeFirstResponder), typeof(bool), typeof(SfInteractiveScrollView), true);

        /// <summary>
        /// Backing store for the <see cref="Syncfusion.Maui.Core.Internals.SfInteractiveScrollView.AllowZoom"/> property.
        /// </summary>
        public static readonly BindableProperty AllowZoomProperty =
                BindableProperty.Create(nameof(AllowZoom), typeof(bool), typeof(SfInteractiveScrollView), false, propertyChanged: OnAllowZoomChanged);

        static readonly BindablePropertyKey ViewportHeightPropertyKey = BindableProperty.CreateReadOnly(nameof(ViewportHeight), typeof(double), typeof(SfInteractiveScrollView), 0.0);
        /// <summary>
        /// Backing store for the <see cref="Syncfusion.Maui.Core.Internals.SfInteractiveScrollView.ViewportHeight"/> property.
        /// </summary>
        /// <remarks>
        /// This property is read-only.
        /// </remarks>
        public static readonly BindableProperty ViewportHeightProperty = ViewportHeightPropertyKey.BindableProperty;

        static readonly BindablePropertyKey ViewportWidthPropertyKey = BindableProperty.CreateReadOnly(nameof(ViewportWidth), typeof(double), typeof(SfInteractiveScrollView), 0.0);
        /// <summary>
        /// Backing store for the <see cref="Syncfusion.Maui.Core.Internals.SfInteractiveScrollView.ViewportWidth"/> property.
        /// </summary>
        /// <remarks>
        /// This property is read-only.
        /// </remarks>
        public static readonly BindableProperty ViewportWidthProperty = ViewportWidthPropertyKey.BindableProperty;

        /// <summary>
        /// Backing store for the <see cref="Syncfusion.Maui.Core.Internals.SfInteractiveScrollView.Orientation"/> property.
        /// </summary>
        public static readonly BindableProperty OrientationProperty =
            BindableProperty.Create(nameof(Orientation), typeof(ScrollOrientation), typeof(SfInteractiveScrollView), ScrollOrientation.Vertical, propertyChanged: OnOrientationChanged);

        static readonly BindablePropertyKey ScrollYPropertyKey = BindableProperty.CreateReadOnly(nameof(ScrollY), typeof(double), typeof(SfInteractiveScrollView), 0d, propertyChanged: OnVerticalOffsetChanged);
        /// <summary>
        /// Backing store for the <see cref="Syncfusion.Maui.Core.Internals.SfInteractiveScrollView.ScrollY"/> property.
        /// </summary>
        /// <remarks>
        /// This property is read-only.
        /// </remarks>
        public static readonly BindableProperty ScrollYProperty = ScrollYPropertyKey.BindableProperty;

        static readonly BindablePropertyKey ScrollXPropertyKey = BindableProperty.CreateReadOnly(nameof(ScrollX), typeof(double), typeof(SfInteractiveScrollView), 0d, propertyChanged: OnHorizontalOffsetChanged);
        /// <summary>
        /// Backing store for the <see cref="Syncfusion.Maui.Core.Internals.SfInteractiveScrollView.ScrollX"/> property.
        /// </summary>
        /// <remarks>
        /// This property is read-only.
        /// </remarks>
        public static readonly BindableProperty ScrollXProperty = ScrollXPropertyKey.BindableProperty;

        /// <summary>
        /// Backing store for the <see cref="Syncfusion.Maui.Core.Internals.SfInteractiveScrollView.ScrollYProportion"/> property.
        /// </summary>
        public static readonly BindableProperty ScrollYProportionProperty =
            BindableProperty.Create(nameof(ScrollYProportion), typeof(double), typeof(SfInteractiveScrollView), 0d, propertyChanged: OnVerticalProportionalOffsetChanged);

        /// <summary>
        /// Backing store for the <see cref="Syncfusion.Maui.Core.Internals.SfInteractiveScrollView.ScrollXProportion"/> property.
        /// </summary>
        public static readonly BindableProperty ScrollXProportionProperty =
            BindableProperty.Create(nameof(ScrollXProportion), typeof(double), typeof(SfInteractiveScrollView), 0d, propertyChanged: OnHorizontalProportionalOffsetChanged);

        /// <summary>
        /// Backing store for the <see cref="Syncfusion.Maui.Core.Internals.SfInteractiveScrollView.VerticalScrollBarVisibility"/> property.
        /// </summary>
        public static readonly BindableProperty VerticalScrollBarVisibilityProperty =
            BindableProperty.Create(nameof(VerticalScrollBarVisibility), typeof(ScrollBarVisibility), typeof(SfInteractiveScrollView), ScrollBarVisibility.Default);

        /// <summary>
        /// Backing store for the <see cref="Syncfusion.Maui.Core.Internals.SfInteractiveScrollView.HorizontalScrollBarVisibility"/> property.
        /// </summary>
        public static readonly BindableProperty HorizontalScrollBarVisibilityProperty =
            BindableProperty.Create(nameof(HorizontalScrollBarVisibility), typeof(ScrollBarVisibility), typeof(SfInteractiveScrollView), ScrollBarVisibility.Default);

        /// <summary>
        /// Backing store for the <see cref="Syncfusion.Maui.Core.Internals.SfInteractiveScrollView.Content"/> property.
        /// </summary>
        public static readonly BindableProperty ContentProperty =
            BindableProperty.Create(nameof(Content), typeof(View), typeof(SfInteractiveScrollView), null, propertyChanged: OnContentChanged);

        /// <summary>
        /// Backing store for the <see cref="Syncfusion.Maui.Core.Internals.SfInteractiveScrollView.ContentSize"/> property.
        /// </summary>
        static readonly BindableProperty ContentSizeProperty =
            BindableProperty.Create(nameof(ContentSize), typeof(Size), typeof(ScrollView), default(Size), propertyChanged: OnExtentSizeChanged);

        /// <summary>
        /// Backing store for the <see cref="Syncfusion.Maui.Core.Internals.SfInteractiveScrollView.ExtentSize"/> property.
        /// </summary>
        public static readonly BindableProperty ExtentSizeProperty =
            BindableProperty.Create(nameof(ExtentSize), typeof(Size?), typeof(ScrollView), null, propertyChanged: OnExtentSizeRequestChanged);

        /// <summary>
        /// Backing store for the <see cref="Syncfusion.Maui.Core.Internals.SfInteractiveScrollView.ZoomLocation"/> property.
        /// </summary>
        public static readonly BindableProperty ZoomLocationProperty =
            BindableProperty.Create(nameof(ZoomLocation), typeof(ZoomLocation), typeof(ScrollView), ZoomLocation.Default);
        #endregion

        #region Properties
        /// <summary>
        /// Detects whether the scrolling is in progress, to prevent conflicts with other interactions like panning. It works for Windows only.
        /// </summary>
        internal bool IsScrolling { get; set; }

        /// <summary>
        /// It enables the scroll request to be delayed until the platform view size or load request is processed.
        /// </summary>
        internal bool IsContentLayoutRequested { get; set; }
        internal ContentPlaceHolder? PresentedContent { get; set; }

        internal bool CanBecomeFirstResponder
        {
            get => (bool)GetValue(CanBecomeFirstResponderProperty);
            set => SetValue(CanBecomeFirstResponderProperty, value);
        }

        /// <summary>
        /// Gets or sets the desired location at which the zoom should occur. 
        /// The default value is <see cref="ZoomLocation.Default"/> and it means that the zoom takes place at the pointer position. This is a bindable property.
        /// </summary>
        public ZoomLocation ZoomLocation
        {
            get => (ZoomLocation)GetValue(ZoomLocationProperty);
            set => SetValue(ZoomLocationProperty, value);
        }

        /// <summary>
        /// Gets or sets the vertically scrolled distance in proportional. The value ranges from 0 to 1. This is a bindable property.
        /// </summary>
        public double ScrollYProportion
        {
            get => (double)GetValue(ScrollYProportionProperty);
            set => SetValue(ScrollYProportionProperty, value);
        }

        /// <summary>
        /// Gets or sets the horizontally scrolled distance in proportional. The value ranges from 0 to 1. This is a bindable property.
        /// </summary>
        public double ScrollXProportion
        {
            get => (double)GetValue(ScrollXProportionProperty);
            set => SetValue(ScrollXProportionProperty, value);
        }

        /// <summary>
        /// Gets the vertical size of the viewport. This is a bindable property.
        /// </summary>
        public double ViewportHeight
        {
            get => (double)GetValue(ViewportHeightProperty);
            internal set => SetValue(ViewportHeightPropertyKey, value);
        }

        /// <summary>
        /// Gets the horizontal size of the viewport. This is a bindable property.
        /// </summary>
        public double ViewportWidth
        {
            get => (double)GetValue(ViewportWidthProperty);
            internal set => SetValue(ViewportWidthPropertyKey, value);
        }

        /// <summary>
        /// Gets or sets the value that indicates the minimum zoom factor permitted for the content. The default value is 0.1.
        /// </summary>
        /// <remarks>
        /// The value should not exceed the <see cref="Syncfusion.Maui.Core.Internals.SfInteractiveScrollView.MaxZoomFactor"/>.
        /// </remarks>
        /// <example>
        /// # [Xaml](#tab/tabid-1)
        /// <code><![CDATA[
        /// <syncfusion:SfInteractiveScrollView MinZoomFactor="0.5"/>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfInteractiveScrollView scrollView = new SfInteractiveScrollView();
        /// scrollView.MinZoomFactor = 0.5;
        /// ]]>
        /// </code>
        /// </example>
        public double MinZoomFactor
        {
            get => (double)GetValue(MinZoomFactorProperty);
            set => SetValue(MinZoomFactorProperty, value);
        }

        /// <summary>
        /// Gets or sets the maximum zoom factor permitted for the content. The default value is 10.
        /// </summary>
        /// <remarks>
        /// The value should not be lower than <see cref="Syncfusion.Maui.Core.Internals.SfInteractiveScrollView.MinZoomFactor"/>.
        /// </remarks>
        /// <example>
        /// # [Xaml](#tab/tabid-1)
        /// <code><![CDATA[
        /// <syncfusion:SfInteractiveScrollView MaxZoomFactor="4"/>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfInteractiveScrollView scrollView = new SfInteractiveScrollView();
        /// scrollView.MaxZoomFactor = 4;
        /// ]]>
        /// </code>
        /// </example>
        public double MaxZoomFactor
        {
            get => (double)GetValue(MaxZoomFactorProperty);
            set => SetValue(MaxZoomFactorProperty, value);
        }

        /// <summary>
        /// Gets the default double tap gesture settings used for zoom. It allows you to change the default options. 
        /// </summary>
        public DoubleTapSettings? DoubleTapSettings
        {
            get
            {
                if (m_panZoomListener != null)
                    return m_panZoomListener.DoubleTapSettings;
                return null;
            }
        }

        /// <summary>
        /// Gets or sets a value that determines whether the vertical scroll bar is visible. This is a bindable property.
        /// </summary>
        /// <example>
        /// # [Xaml](#tab/tabid-1)
        /// <code><![CDATA[
        /// <syncfusion:SfInteractiveScrollView VerticalScrollBarVisibility="Always"/>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfInteractiveScrollView scrollView = new SfInteractiveScrollView();
        /// scrollView.VerticalScrollBarVisibility = ScrollBarVisibility.Always;
        /// ]]>
        /// </code>
        /// </example>
        public ScrollBarVisibility VerticalScrollBarVisibility
        {
            get => (ScrollBarVisibility)GetValue(VerticalScrollBarVisibilityProperty);
            set => SetValue(VerticalScrollBarVisibilityProperty, value);
        }

        /// <summary>
        /// Gets or sets a value that determines whether the horizontal scroll bar is visible. This is a bindable property.
        /// </summary>
        /// <example>
        /// # [Xaml](#tab/tabid-1)
        /// <code><![CDATA[
        /// <syncfusion:SfInteractiveScrollView HorizontalScrollBarVisibility="Always"/>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfInteractiveScrollView scrollView = new SfInteractiveScrollView();
        /// scrollView.HorizontalScrollBarVisibility = ScrollBarVisibility.Always;
        /// ]]>
        /// </code>
        /// </example>
        public ScrollBarVisibility HorizontalScrollBarVisibility
        {
            get => (ScrollBarVisibility)GetValue(HorizontalScrollBarVisibilityProperty);
            set => SetValue(HorizontalScrollBarVisibilityProperty, value);
        }

        /// <summary>
        /// Gets or sets the scrolling direction of the scroll view. The default value is <see cref="ScrollOrientation.Vertical"/> and it allows scroll both vertically and horizontally. The scrolling can be disabled by setting the value to <see cref="ScrollOrientation.Neither"/>. This is a bindable property.
        /// </summary>
        /// <example>
        /// # [Xaml](#tab/tabid-1)
        /// <code><![CDATA[
        /// <syncfusion:SfInteractiveScrollView Orientation="Both"/>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfInteractiveScrollView scrollView = new SfInteractiveScrollView();
        /// scrollView.Orientation = ScrollOrientation.Both;
        /// ]]>
        /// </code>
        /// </example>
        public ScrollOrientation Orientation
        {
            get => (ScrollOrientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        /// <summary>
        /// Gets the current logical horizontal and vertical size of the scrollable area. The returned value is described in device-independent units.
        /// </summary>
        /// <remarks>
        /// This extent size is set during the layout phase.
        /// </remarks>
        internal Size ContentSize
        {
            get => (Size)GetValue(ContentSizeProperty);
            set => SetValue(ContentSizeProperty, value);
        }

        /// <summary>
        /// Gets or sets the overall logical horizontal and vertical size of the scrollable area. The value is described in device-independent units. This is a bindable property.
        /// </summary>
        /// <remarks>
        /// To prevent layout calls for each UI update during virtualization (or) an asynchronous process, it is advised to specify the total scrollable area using the <see cref="Syncfusion.Maui.Core.Internals.SfInteractiveScrollView.ExtentSize"/> property at the beginning itself.
        /// </remarks>
        public Size? ExtentSize
        {
            get => (Size?)GetValue(ExtentSizeProperty);
            set => SetValue(ExtentSizeProperty, value);
        }

        /// <summary>
        /// Gets the distance that the content has been scrolled vertically. The returned value is described in device-independent units. This is a bindable property.
        /// </summary>
        public double ScrollY
        {
            get => (double)GetValue(ScrollYProperty);
            internal set => SetValue(ScrollYPropertyKey, value);
        }

        /// <summary>
        /// Gets the distance that the content has been scrolled horizontally. The returned value is described in device-independent units. This is a bindable property.
        /// </summary>
        public double ScrollX
        {
            get => (double)GetValue(ScrollXProperty);
            internal set => SetValue(ScrollXPropertyKey, value);
        }

        /// <summary>
        /// Gets the current zoom factor. This is a bindable property. 
        /// The value 1 indicates that no additional scaling is applied. To modify the zoom value of the control, use <see cref="SfInteractiveScrollView.ZoomTo"/> method.
        /// </summary>
        public double ZoomFactor
        {
            get => (double)GetValue(ZoomFactorProperty);
            internal set => SetValue(ZoomFactorPropertyKey, value);
        }

        /// <summary>
        /// Gets or sets the view to display in the <see cref="Syncfusion.Maui.Core.Internals.SfInteractiveScrollView"/>. This is a bindable property.
        /// </summary>
        /// <example>
        /// # [Xaml](#tab/tabid-1)
        /// <code><![CDATA[
        /// <syncfusion:SfInteractiveScrollView>
        ///      <syncfusion:SfInteractiveScrollView>
        ///        <StackLayout>
        ///            <Label Text = "FOR the most wild, yet most homely narrative which I am about to pen, I neither expect nor solicit belief. Mad indeed would I be to expect it, in a case where my very senses reject their own evidence. Yet, mad am I not -- and very surely do I not dream. But to-morrow I die, and to-day I would unburthen my soul. My immediate purpose is to place before the world, plainly, succinctly, and without comment, a series of mere household events. In their consequences, these events have terrified -- have tortured -- have destroyed me. Yet I will not attempt to expound them. To me, they have presented little but Horror -- to many they will seem less terrible than barroques. Hereafter, perhaps, some intellect may be found which will reduce my phantasm to the common-place -- some intellect more calm, more logical, and far less excitable than my own, which will perceive, in the circumstances I detail with awe, nothing more than an ordinary succession of very natural causes and effects." />
        ///            < !--More Label objects go here -->
        ///        </StackLayout>
        ///      </syncfusion:SfInteractiveScrollView>
        /// </syncfusion:SfInteractiveScrollView>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfInteractiveScrollView scrollView = new SfInteractiveScrollView();
        /// StackLayout stackLayout = new StackLayout();
        /// stackLayout.Add(new Label { Text = "FOR the most wild, yet most homely narrative which I am about to pen, I neither expect nor solicit belief. Mad indeed would I be to expect it, in a case where my very senses reject their own evidence. Yet, mad am I not -- and very surely do I not dream. But to-morrow I die, and to-day I would unburthen my soul. My immediate purpose is to place before the world, plainly, succinctly, and without comment, a series of mere household events. In their consequences, these events have terrified -- have tortured -- have destroyed me. Yet I will not attempt to expound them. To me, they have presented little but Horror -- to many they will seem less terrible than barroques. Hereafter, perhaps, some intellect may be found which will reduce my phantasm to the common-place -- some intellect more calm, more logical, and far less excitable than my own, which will perceive, in the circumstances I detail with awe, nothing more than an ordinary succession of very natural causes and effects." });
        /// //More Label objects go here
        /// scrollView.Content = stackLayout;
        /// ]]>
        /// </code>
        /// </example>
        public View? Content
        {
            get => (View)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        /// <summary>
        /// Gets or sets whether to allow the content zooming. If false, content zooming will not be allowed. This is a bindable property. The default value is false. 
        /// </summary>
        /// <example>
        /// # [Xaml](#tab/tabid-1)
        /// <code><![CDATA[
        /// <syncfusion:SfInteractiveScrollView AllowZoom="True"/>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfInteractiveScrollView scrollView = new SfInteractiveScrollView();
        /// scrollView.AllowZoom = true;
        /// ]]>
        /// </code>
        /// </example>
        public bool AllowZoom
        {
            get => (bool)GetValue(AllowZoomProperty);
            set => SetValue(AllowZoomProperty, value);
        }

        Point CenteredOrigin
        {
            get
            {
                return new Point(ScrollX + (ViewportWidth / 2), ScrollY + (ViewportHeight / 2));
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Zooms the content by the specified factor at the given origin.
        /// </summary>
        /// <param name=" zoomFactor ">The factor by which the content to be zoomed. </param>
        /// <param name="zoomOrigin">The origin location from which the zoom must be performed. The default origin location is the left top of the content.</param>
        public void ZoomBy(double zoomFactor, Point? zoomOrigin = null)
        {
            ZoomTo(this.ZoomFactor * zoomFactor, zoomOrigin);
        }

        internal void ScrollTo(double x, double y, bool animated = true)
        {
            if (Handler != null)
            {
                ScrollToParameters? request = new ScrollToParameters(x, y, animated);
                Handler.Invoke(nameof(SfInteractiveScrollView.ScrollTo), request);
                request = null;
            }
            else
            {
                ScrollX = x;
                ScrollY = y;
            }
        }

        /// <summary>
        /// Returns a task that scrolls the scroll view to a position asynchronously.
        /// </summary>
        /// <param name="x">The distance to which the control scrolls horizontally. The value is measured in device-independent units. </param>
        /// <param name="y">The distance to which the control scrolls vertically. The value is measured in device-independent units. </param>
        /// <param name="animated">It indicates whether to animate the scroll. The default value is true. </param>
        public Task ScrollToAsync(double x, double y, bool animated = true)
        {
            switch (Orientation)
            {
                case ScrollOrientation.Neither:
                    return Task.FromResult(false);
                case ScrollOrientation.Vertical:
                    x = ScrollX;
                    break;
                case ScrollOrientation.Horizontal:
                    y = ScrollY;
                    break;
            }

            CheckTaskCompletionSource();
            ScrollTo(x, y, animated);
            return m_scrollCompletionSource!.Task;
        }

        /// <summary>
        /// Returns a task that scrolls the scroll view by a position asynchronously.
        /// </summary>
        /// <param name="x">The distance by which the control scrolls horizontally. The value is measured in device-independent units. </param>
        /// <param name="y">The distance by which the control scrolls vertically. The value is measured in device-independent units. </param>
        /// <param name="animated">It indicates whether to animate the scroll. The default value is true. </param>
        public Task ScrollByAsync(double x, double y, bool animated = true)
        {
            return ScrollToAsync(ScrollX + x, ScrollY + y, animated);
        }

        void CheckTaskCompletionSource()
        {
            if (m_scrollCompletionSource != null && m_scrollCompletionSource.Task.Status == TaskStatus.Running)
            {
                m_scrollCompletionSource.TrySetCanceled();
            }
            m_scrollCompletionSource = new TaskCompletionSource<bool>();
        }

        /// <summary>
        /// Scrolls the control horizontally to the specific offset value.
        /// </summary>
        /// <param name="x">The offset value to which the control scrolls horizontally.</param>
        /// <param name="animated">It is an optional parameter which indicates whether or not to animate the scroll. The default value is true.</param>
        internal void ScrollToX(double x, bool animated = true)
        {
            ScrollTo(x, ScrollY, animated);
            ScrollX = x;
        }

        /// <summary>
        /// Scrolls the control vertically to the specific offset value.
        /// </summary>
        /// <param name="y">The offset value to which the control scrolls vertically.</param>
        /// <param name="animated">It is an optional parameter which indicates whether or not to animate the scroll. The default value is true.</param>
        internal void ScrollToY(double y, bool animated = true)
        {
            ScrollTo(ScrollX, y, animated);
            ScrollY = y;
        }

        /// <summary>
        /// Updates the <see cref="Syncfusion.Maui.Core.Internals.SfInteractiveScrollView.ZoomFactor"/>  to the specified factor and triggers the zoom events.
        /// </summary>
        /// <param name="zoomFactor">The desired zoom factor.  The applicable value ranges from  <see cref="Syncfusion.Maui.Core.Internals.SfInteractiveScrollView.MinZoomFactor"/> to  <see cref="Syncfusion.Maui.Core.Internals.SfInteractiveScrollView.MaxZoomFactor"/>.</param>
        /// <param name="zoomOrigin">It is an optional parameter, specifies origin point in the control from which the zoom to be occurred. The default origin location is the left top of the content.</param>
        public void ZoomTo(double zoomFactor, Point? zoomOrigin = null)
        {
            if (Handler != null)
                m_panZoomListener?.ZoomTo(zoomFactor, zoomOrigin);
            else
                ZoomFactor = zoomFactor;
        }
        #endregion

        #region Interface implementations
        /// <summary>
        /// Implemented method from "ITapGestureListener" interface.
        /// </summary>
        /// <remarks>
        /// It is only applicable to iOS to provide focus on the element when on tapping, for listening to keyboard events in iOS/MAC.
        /// </remarks>
        void ITapGestureListener.OnTap(TapEventArgs e)
        {
#if IOS || MACCATALYST
            CanBecomeFirstResponder = true;
            Focus();
#endif
        }
        #endregion

        #region Helper methods
        void CheckIfZoomLocationRequested(ZoomEventArgs e)
        {
            if (ZoomLocation == ZoomLocation.Centered)
            {
                if (ViewportHeight > 0 && ViewportWidth > 0)
                    e.ZoomOrigin = CenteredOrigin;
            }
        }

        void AddOrRemoveZoomGestures(bool allowZoom)
        {
            if (allowZoom)
                PresentedContent?.AddZoomGestures();
            else
                PresentedContent?.RemoveZoomGestures();
        }

        internal void UpdateHorizontalProprotion(double horizontalOffset)
        {
            if (horizontalOffset == m_currentHorizontalOffset)
                return;
            if (ContentSize.Width > 0)
            {
                double desiredHorizontalProportion = horizontalOffset / ContentSize.Width;
                m_currentHorizontalProportion = desiredHorizontalProportion;
                ScrollXProportion = desiredHorizontalProportion;
            }
        }

        internal void UpdateVerticalProportion(double verticalOffset)
        {
            if (verticalOffset == m_currentVerticalOffset)
                return;
            if (ContentSize.Height > 0)
            {
                double desiredVerticalProportion = verticalOffset / ContentSize.Height;
                m_currentVerticalProportion = desiredVerticalProportion;
                ScrollYProportion = desiredVerticalProportion;
            }
        }

        void ScrollToVerticalOffsetProportionately(double offsetProportion)
        {
            if (ContentSize.Height > 0)
            {
                double desiredVerticalOffset = offsetProportion * ContentSize.Height;
                m_currentVerticalOffset = desiredVerticalOffset;
                ScrollToY(desiredVerticalOffset, false);
            }
        }

        void ScrollToHorizontalOffsetProportionately(double offsetProportion)
        {
            if (ContentSize.Width > 0)
            {
                double desiredHorizontalOffset = offsetProportion * ContentSize.Width;
                m_currentHorizontalOffset = desiredHorizontalOffset;
                ScrollToX(desiredHorizontalOffset, false);
            }
        }

        PanMode ConvertOrientationToPanMode(ScrollOrientation orientation)
        {
            switch (orientation)
            {
                case ScrollOrientation.Neither:
                    return PanMode.None;
                case ScrollOrientation.Horizontal:
                    return PanMode.Horizontal;
                case ScrollOrientation.Vertical:
                    return PanMode.Vertical;
                default:
                    return PanMode.Both;
            }
        }

        internal void SendScrollFinished()
        {
            if (m_scrollCompletionSource != null)
                m_scrollCompletionSource.TrySetResult(true);
        }

        void ApplyPresetProperties()
        {
            if (MinZoomFactor > ZoomFactor)
                ZoomTo(MinZoomFactor);
            else if (ZoomFactor != (double)ZoomFactorProperty.DefaultValue)
                ZoomTo(ZoomFactor);

            if (ScrollY != (double)ScrollYProperty.DefaultValue || ScrollX != (double)ScrollXProperty.DefaultValue)
            {
                IsContentLayoutRequested = true;
                ScrollTo(ScrollX, ScrollY, false);
            }
        }

        /// <summary>
        /// Allocates content width or height to the control, when the width or height value is zero.
        /// </summary>
        internal void RequestLayout(double controlWidth, double controlHeight)
        {
            if (controlWidth <= 0 || controlHeight <= 0)
            {
                if (controlWidth <= 0)
                    controlWidth = m_contentSize.Width;
                else if (controlHeight <= 0)
                    controlHeight = m_contentSize.Height;
            }
            this.Layout(new Rect(this.Bounds.X, this.Bounds.Y, controlWidth, controlHeight));
        }

        void UpdatePresentedContent(View content)
        {
            if (PresentedContent != null)
            {
                PresentedContent.Children.Add(content);
                PresentedContent.LayoutContent();
                ApplyPresetProperties();
            }
        }

        /// <summary>
        /// Updates the extent size based on the changes in the scale or size of the control. 
        /// </summary>
        void InvalidateExtentSize(Size extentSize)
        {
            extentSize.Width = Math.Max(extentSize.Width, this.Bounds.Width);
            extentSize.Height = Math.Max(extentSize.Height, this.Bounds.Height);
            ContentSize = extentSize;
        }

        void Reset()
        {
            this.ScrollTo(0, 0, false);
            m_panZoomListener!.CurrentZoomFactor = 1;
            if (ExtentSize == null)
                ContentSize = Size.Zero;
            PresentedContent?.Reset();
            ZoomFactor = 1;
            ScrollY = ScrollX = 0;
            m_contentSize = Size.Zero;
        }

        void WireZoomEvents()
        {
            if (m_panZoomListener != null)
            {
                m_panZoomListener.ZoomStarted += OnZoomStarted;
                m_panZoomListener.ZoomChanged += OnZoomChanged;
                m_panZoomListener.ZoomEnded += OnZoomEnded;
            }
        }

        void UnwireZoomEvents()
        {
            if (m_panZoomListener != null)
            {
                m_panZoomListener.ZoomStarted -= OnZoomStarted;
                m_panZoomListener.ZoomChanged -= OnZoomChanged;
                m_panZoomListener.ZoomEnded -= OnZoomEnded;
            }
        }

        void WireEvents()
        {
            this.PropertyChanging += OnPropertyChanging;
            this.PropertyChanged += OnPropertyChanged;
            WireZoomEvents();
        }

        void UnwireEvents()
        {
            this.PropertyChanging -= OnPropertyChanging;
            this.PropertyChanged -= OnPropertyChanged;
            UnwireZoomEvents();
        }
        #endregion

        #region Event handlers
        private void OnZoomStarted(object? sender, ZoomEventArgs e)
        {
            if (ZoomStarted != null)
            {
                CheckIfZoomLocationRequested(e);
                ZoomStarted(this, e);
            }
        }

        private void OnZoomChanged(object? sender, ZoomEventArgs e)
        {
            if (ZoomChanged != null)
            {
                CheckIfZoomLocationRequested(e);
                ZoomChanged(this, e);
            }
            ZoomFactor = e.ZoomFactor;
        }

        private void OnZoomEnded(object? sender, ZoomEventArgs e)
        {
            if (ZoomEnded != null)
            {
                CheckIfZoomLocationRequested(e);
                ZoomEnded(this, e);
            }
        }

        private void OnPropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Content):
                    if (Content != null)
                        Content.SizeChanged -= OnContentSizeChanged;
                    Reset();
                    break;
            }
        }

        private void OnPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Content):
                    if (Content != null)
                        Content.SizeChanged += OnContentSizeChanged;
                    InvalidateMeasure();
                    break;
                case nameof(BackgroundColor):
                    if (PresentedContent != null)
                        PresentedContent.BackgroundColor = this.BackgroundColor;
                    break;
#if !WINDOWS
                case nameof(IsEnabled):
                    if (PresentedContent != null)
                        PresentedContent.IsEnabled = IsEnabled;
                    break;
                //Since the flow direction - "MatchParent" is not working when RTL (Right To Left) applied in Android and iOS for the scroll view "content", forcing the effective flow direction. 
                case nameof(FlowDirection):
                    if (PresentedContent != null)
                    {
                        PresentedContent.FlowDirection = this.FlowDirection;
#if IOS || MACCATALYST
                        PresentedContent.LayoutContent();
#endif
                    }
                    break;
#endif
            }
        }

        internal void OnScrollChanged(ScrollChangedEventArgs scrolledEventArgs)
        {
            if (ScrollY == scrolledEventArgs.ScrollY && ScrollX == scrolledEventArgs.ScrollX)
                return;
            ScrollY = scrolledEventArgs.ScrollY;
            ScrollX = scrolledEventArgs.ScrollX;
            ScrollChanged?.Invoke(this, scrolledEventArgs);
        }
        #endregion

        #region Coerce value callbacks
        private static object CoerceMinZoom(BindableObject bindable, object value)
        {
            if (bindable is SfInteractiveScrollView scrollView)
            {
                if (double.TryParse(value.ToString(), out double minZoomFactor))
                {
                    if (minZoomFactor > scrollView.MaxZoomFactor)
                        value = scrollView.MaxZoomFactor;
                }
            }
            return value;
        }

        private static object CoerceMaxZoom(BindableObject bindable, object value)
        {
            if (bindable is SfInteractiveScrollView scrollView)
            {
                if (double.TryParse(value.ToString(), out double maxZoomFactor))
                {
                    if (maxZoomFactor < scrollView.MinZoomFactor)
                        value = scrollView.MinZoomFactor;
                }
            }
            return value;
        }
        #endregion

        #region Property change handlers
        private static void OnMinZoomChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfInteractiveScrollView scrollView)
            {
                double minZoomFactor = (double)newValue;
                scrollView.m_panZoomListener!.MinZoomFactor = minZoomFactor;
                if (scrollView.ZoomFactor < minZoomFactor)
                    scrollView.ZoomTo(minZoomFactor, new Point(scrollView.ScrollX, scrollView.ScrollY));
            }
        }

        private static void OnMaxZoomChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfInteractiveScrollView scrollView)
            {
                double maxZoomFactor = (double)newValue;
                scrollView.m_panZoomListener!.MaxZoomFactor = maxZoomFactor;
                if (scrollView.ZoomFactor > maxZoomFactor)
                    scrollView.ZoomTo(maxZoomFactor, new Point(scrollView.ScrollX, scrollView.ScrollY));
            }
        }

        private static void OnAllowZoomChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfInteractiveScrollView scrollView && scrollView.Handler != null)
            {
                bool allowZoom = (bool)newValue;
                scrollView.AddOrRemoveZoomGestures(allowZoom);
            }
        }

        private static void OnHorizontalOffsetChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfInteractiveScrollView scrollView && scrollView.Handler != null)
            {
                scrollView.UpdateHorizontalProprotion((double)newValue);
            }
        }

        private static void OnVerticalOffsetChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfInteractiveScrollView scrollView && scrollView.Handler != null)
            {
                scrollView.UpdateVerticalProportion((double)newValue);
            }
        }

        private static void OnOrientationChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfInteractiveScrollView scrollView)
            {
                ScrollOrientation orientation = (ScrollOrientation)newValue;
                scrollView.m_panZoomListener!.PanMode = scrollView.ConvertOrientationToPanMode(orientation);
            }
        }

        private static void OnVerticalProportionalOffsetChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfInteractiveScrollView scrollView && scrollView.Handler != null)
            {
                if (scrollView.m_currentVerticalProportion != (double)newValue)
                    scrollView.ScrollToVerticalOffsetProportionately((double)newValue);
            }
        }

        private static void OnHorizontalProportionalOffsetChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfInteractiveScrollView scrollView && scrollView.Handler != null)
            {
                if (scrollView.m_currentHorizontalProportion != (double)newValue)
                    scrollView.ScrollToHorizontalOffsetProportionately((double)newValue);
            }
        }

        private static void OnContentChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfInteractiveScrollView scrollView && scrollView.Handler != null)
            {
                scrollView.UpdatePresentedContent((View)newValue);
            }
        }

        private static void OnExtentSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfInteractiveScrollView scrollView)
            {
                Size extentSize = (Size)newValue;
                var presentedContent = scrollView.PresentedContent;
                if (presentedContent != null)
                {
                    presentedContent.RequestSize(extentSize.Width, extentSize.Height);
#if !WINDOWS
                    scrollView.IsContentLayoutRequested = true;
#endif
                }
            }
        }

        private static void OnExtentSizeRequestChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfInteractiveScrollView scrollView)
            {
                Size? extentSizeRequest = (Size?)newValue;
                if (extentSizeRequest != null)
                {
                    scrollView.InvalidateExtentSize(extentSizeRequest.Value);
                }
            }
        }

        private void OnContentSizeChanged(object? sender, EventArgs e)
        {
            m_contentSize = Content!.ComputeDesiredSize(double.PositiveInfinity, double.PositiveInfinity);

            if (ExtentSize == null)
            {
                if (!m_contentSize.IsZero)
                {
                    RequestLayout(this.Bounds.Width, this.Bounds.Height);
#if !ANDROID
                    InvalidateExtentSize(m_contentSize);
#endif
                }
            }
            UpdateVerticalProportion(ScrollY);
            UpdateHorizontalProprotion(ScrollX);
        }
        #endregion

        #region Overriden methods
        /// <summary>
        /// Overrides the handler changed occurrence.
        /// </summary>
        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();
            if (Handler != null)
            {
                AddOrRemoveZoomGestures(AllowZoom);
                if (Content != null)
                    UpdatePresentedContent(Content);
            }
            else
            {
                PresentedContent?.Unload();
            }
        }

        /// <summary>
        /// This method is called to handle when the size of the element is set during a layout cycle.
        /// </summary>
        /// <param name="width">The new width of the element.</param>
        /// <param name="height">The new height of the element.</param>
        protected override void OnSizeAllocated(double width, double height)
        {
            if (!m_contentSize.IsZero)
            {
                RequestLayout(width, height);
                // Invalidate extent size to maintain the content alignment on window size change as well as for pan gesture validation.
                if (ExtentSize == null)
                    InvalidateExtentSize(m_contentSize);
                else
                    InvalidateExtentSize(ExtentSize.Value);
            }
            base.OnSizeAllocated(this.Bounds.Width, this.Bounds.Height);
        }
        #endregion
    }
}
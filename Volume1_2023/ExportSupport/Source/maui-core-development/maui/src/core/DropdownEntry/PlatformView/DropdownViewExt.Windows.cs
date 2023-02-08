
using Microsoft.Maui.Platform;
using Microsoft.UI;
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation.Peers;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Grid = Microsoft.UI.Xaml.Controls.Grid;
using SolidColorBrush = Microsoft.UI.Xaml.Media.SolidColorBrush;

namespace Syncfusion.Maui.Core.Platform
{

    /// <summary>
    /// The DropdownViewExt class which is native class to access native properties and method of popup.
    /// </summary>
    public class DropdownViewExt : ContentPanel
    {

        #region Fields

        private double popupHeight = 400;
        private double popupWidth = 0;
        private int popupX = 0;
        private int popupY = 0;
        internal Grid? parentGrid;
        private FrameworkElement? anchorView;
        private bool isInitialLoad;

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        internal Popup? Popup { get; set; }

        /// <summary>
        /// 
        /// </summary>
        internal double PopupHeight
        {
            get
            {
                return popupHeight;
            }
            set
            {
                this.popupHeight = value;
                if (this.parentGrid != null && !Double.IsNaN(value))
                {
                    this.parentGrid.MaxHeight = value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal double PopupWidth
        {
            get
            {
                return popupWidth;
            }
            set
            {
                this.popupWidth = value;
                if (this.parentGrid != null && !Double.IsNaN(value) && value > 0)
                {
                    this.parentGrid.MaxWidth = value;
                }
            }
        }

        internal int PopupX
        {
            get
            {
                return popupX;
            }
            set
            {
                this.popupX = value;
                if(this.Popup != null)
                    this.Popup.HorizontalOffset= value;
            }
        }

        internal int PopupY
        {
            get
            {
                return popupY;
            }
            set
            {
                this.popupY = value;
                if (this.Popup != null)
                    this.Popup.VerticalOffset = value;
            }
        }

        internal FrameworkElement? AnchorView
        {
            get
            {
                return anchorView;
            }
            set
            {
                anchorView = value;
                if(this.AnchorView != null && this.Popup != null)
                {
                    this.Popup.PlacementTarget = this.AnchorView;
                }
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DropdownViewExt"/> class.
        /// </summary>
        public DropdownViewExt()
        {
            Initialize();
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        internal void Initialize()
        {
            parentGrid = new Grid
            {
                CornerRadius = new CornerRadius(4),
                Background = new SolidColorBrush(Colors.White),
                BorderBrush = new SolidColorBrush(Color.FromArgb(255, 220, 220, 220)),
                BorderThickness = new Thickness(1),
                Translation = new Vector3(0, 0, 32),
                Shadow = new ThemeShadow(),
                MaxHeight = this.PopupHeight
            };

            Popup = new Popup
            {
                Child = parentGrid,
                DesiredPlacement = PopupPlacementMode.BottomEdgeAlignedLeft,
                ShouldConstrainToRootBounds = false,
            };

            this.Children.Add(Popup);

            this.Loaded += PopupViewExt_Loaded;
        }

        private void PopupViewExt_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.XamlRoot != null && this.XamlRoot.Content != null)
            {
                if (Popup != null)
                {
                    if (Popup.XamlRoot == null)
                    {
                        Popup.XamlRoot = this.XamlRoot;
                    }
                    if (isInitialLoad)
                    {
                        Popup.IsOpen = true;
                    }
                }

                this.XamlRoot.Content.PointerPressed += Content_PointerPressed;
            }

            isInitialLoad = false;
        }


        private void Content_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (this.Popup != null && this.Popup.IsOpen)
            {
                this.Popup.IsOpen = false;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        internal void ShowPopup()
        {
            if (Popup != null && !Popup.IsOpen)
            {
                if (Popup.XamlRoot == null)
                {
                    Popup.XamlRoot = this.XamlRoot;
                    if (this.AnchorView != null && this.Popup.PlacementTarget == null)
                    {
                        Popup.PlacementTarget = this.AnchorView;
                    }
                }

                if (Popup.XamlRoot != null)
                {
                    Popup.IsOpen = true;
                }
                else 
                {
                    isInitialLoad = true;
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        internal void HidePopup()
        {
            if (Popup != null && Popup.IsOpen)
            {
                Popup.IsOpen = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        internal void UpdatePopupContent(FrameworkElement view)
        {
            if (parentGrid != null && !parentGrid.Children.Contains(view))
            {
                parentGrid.Children.Add(view);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal void Dispose()
        {
            this.Loaded -= PopupViewExt_Loaded;

            if (this.AnchorView != null)
            {
                this.AnchorView = null;
            }

            if (this.XamlRoot != null && this.XamlRoot.Content != null)
            {
                this.XamlRoot.Content.PointerPressed -= Content_PointerPressed;
            }
        }

        #endregion
    }
}

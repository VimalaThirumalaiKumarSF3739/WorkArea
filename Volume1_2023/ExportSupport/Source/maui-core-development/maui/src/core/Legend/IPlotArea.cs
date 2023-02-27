using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Core.Internals
{
    /// <summary>
    /// 
    /// </summary>
    internal interface IPlotArea
    {
        /// <summary>
        /// 
        /// </summary>
        public ILegend? Legend { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ReadOnlyObservableCollection<ILegendItem> LegendItems { get; }

        /// <summary>
        /// 
        /// </summary>
        public Rect PlotAreaBounds { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool ShouldPopulateLegendItems { get; set;}

        /// <summary>
        /// 
        /// </summary>
        public LegendHandler LegendItemToggleHandler
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<LegendItemEventArgs> LegendItemToggled
        {
            add
            {
                throw new NotImplementedException();
            }
            remove
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EventArgs> LegendItemsUpdated
        {
            add
            {
                throw new NotImplementedException();
            }
            remove
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateLegendItems()
        {

        }
    }
}

using Microsoft.Maui;
using Microsoft.Maui.Controls.PlatformConfiguration.GTKSpecific;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Core.Internals
{
    internal partial class OverlayContainerHandler
    {
        /// <summary>
        /// 
        /// </summary>
        public OverlayContainerHandler() : base(OverlayContainerHandler.ViewMapper)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="commandMapper"></param>
        public OverlayContainerHandler(PropertyMapper mapper, CommandMapper commandMapper) : base(mapper, commandMapper)
        {
        }
    }
}

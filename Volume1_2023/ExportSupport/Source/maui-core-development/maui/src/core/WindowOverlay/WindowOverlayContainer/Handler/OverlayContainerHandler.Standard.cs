using Microsoft.Maui;
using Microsoft.Maui.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Core.Internals
{
    internal partial class OverlayContainerHandler : ViewHandler<WindowOverlayContainer, object>
    {
        public OverlayContainerHandler(IPropertyMapper mapper, CommandMapper? commandMapper = null) : base(mapper, commandMapper)
        {
        }

        protected override object CreatePlatformView()
        {
            return new object();
        }
    }
}

using Microsoft.Maui;
using Microsoft.Maui.Handlers;
using Syncfusion.Maui.Core.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Core.Internals
{
    internal partial class OverlayContainerHandler : ViewHandler<WindowOverlayContainer, WindowOverlayStack>
    {
        internal OverlayContainerHandler(IPropertyMapper mapper, CommandMapper? commandMapper = null) : base(mapper, commandMapper)
        {
        }
      
        protected override WindowOverlayStack CreatePlatformView()
        {
            return new WindowOverlayStack(Context);
        }
    }
}

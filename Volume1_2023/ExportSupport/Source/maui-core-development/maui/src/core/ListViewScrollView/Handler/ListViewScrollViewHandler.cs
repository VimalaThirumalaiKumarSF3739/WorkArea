using Microsoft.Maui.Handlers;
using Syncfusion.Maui.Core.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Core.Internals
{
    /// <summary>
    /// ScrollViewHandler for <see cref="ListViewScrollViewExt"/>.
    /// </summary>
    // Todo - Reverting SfInteractiveScrollView - internal partial class ListViewScrollViewHandler : SfInteractiveScrollViewHandler
    internal partial class ListViewScrollViewHandler : ScrollViewHandler
    {
        /// <summary>
        /// Gets the <see cref="ListViewScrollViewExt"/>.
        /// </summary>
        private ListViewScrollViewExt? ScrollView
        {
            get { return this.VirtualView as ListViewScrollViewExt; }
        }
    }
}

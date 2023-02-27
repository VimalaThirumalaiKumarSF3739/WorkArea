using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Core.Internals
{
    internal class WindowOverlayStack : FrameLayout
    {
        public WindowOverlayStack(Context context)
            : base(context)
        {
        }

        public WindowOverlayStack(Context context, IAttributeSet? attrs)
            : base(context, attrs)
        {
        }

        public WindowOverlayStack(Context context, IAttributeSet? attrs, int defStyleAttr)
            : base(context, attrs, defStyleAttr)
        {
        }

        public WindowOverlayStack(Context context, IAttributeSet? attrs, int defStyleAttr, int defStyleRes)
            : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        protected WindowOverlayStack(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }
    }

}

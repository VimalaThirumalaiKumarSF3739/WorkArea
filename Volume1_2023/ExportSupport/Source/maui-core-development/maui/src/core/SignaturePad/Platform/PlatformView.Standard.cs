using System;

namespace Syncfusion.Maui.Graphics.Internals
{
    public partial class PlatformSignaturePad
    {
        internal Microsoft.Maui.Controls.ImageSource ToImageSource()
        {
            throw new NotImplementedException();
        }

        internal void Connect(ISignaturePad mauiView)
        {
        }

        internal void Disconnect()
        {
        }

        internal void Clear()
        {
        }

        private void AddBezier(Bezier curve, float startWidth, float endWidth)
        {
        }

        private void DrawPoint(float x, float y, float width)
        {
        }

        private void WipeOut()
        {
        }

        private void Invalidate()
        {
        }
    }
}

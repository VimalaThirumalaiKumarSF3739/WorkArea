﻿using Microsoft.Maui.Graphics.Win2D;
using Microsoft.Maui.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Graphics.Internals
{
	public partial class SfDrawableViewHandler : ViewHandler<IDrawableView, W2DGraphicsView>
	{
		#region Methods

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected override W2DGraphicsView CreatePlatformView()
		{
			var nativeGraphicsView = new W2DGraphicsView();
			nativeGraphicsView.Drawable = VirtualView;
			nativeGraphicsView.UseSystemFocusVisuals = true;
			nativeGraphicsView.IsTabStop = true;
			return nativeGraphicsView;
		}

		/// <summary>
		/// 
		/// </summary>
		public void Invalidate()
		{
			this.PlatformView?.Invalidate();
		}

		#endregion
	}
}

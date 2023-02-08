using Microsoft.Maui;
using Microsoft.Maui.Handlers;
using System.Runtime.Versioning;

namespace Syncfusion.Maui.Graphics.Internals
{
	/// <summary>
	/// 
	/// </summary>
    public partial class SfDrawableViewHandler
    {
		/// <summary>
		/// 
		/// </summary>
		public SfDrawableViewHandler() : base(SfDrawableViewHandler.ViewMapper)
		{

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="mapper"></param>
		public SfDrawableViewHandler(PropertyMapper mapper) : base(mapper)
		{
		}
	}
}

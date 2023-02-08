
using Microsoft.Maui;
using Microsoft.Maui.Handlers;
using System;

namespace Syncfusion.Maui.Core
{
	/// <summary>
	/// 
	/// </summary>
	public partial class SfViewHandler
	{
		/// <summary>
		/// 
		/// </summary>
		public SfViewHandler() : base(SfViewHandler.ViewMapper)
		{

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="mapper"></param>
		public SfViewHandler(PropertyMapper mapper) : base(mapper)
		{
		}
	}
}
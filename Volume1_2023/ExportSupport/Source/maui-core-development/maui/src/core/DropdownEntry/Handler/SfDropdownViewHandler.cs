
using Microsoft.Maui;
using Microsoft.Maui.Handlers;
using System;

namespace Syncfusion.Maui.Core
{
    /// <summary>
    /// Handler class for popup view.
    /// </summary>
    public partial class SfDropdownViewHandler : ContentViewHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SfDropdownViewHandler"/> class.
        /// </summary>
        public SfDropdownViewHandler()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SfDropdownViewHandler"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        public SfDropdownViewHandler(PropertyMapper mapper) : base(mapper)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SfDropdownViewHandler"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        public SfDropdownViewHandler(IPropertyMapper? mapper = null) : base(mapper)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SfDropdownViewHandler"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="commandMapper">The command mapper.</param>
        protected SfDropdownViewHandler(IPropertyMapper mapper, CommandMapper? commandMapper = null) : base(mapper, commandMapper)
        {
        }
    }
}
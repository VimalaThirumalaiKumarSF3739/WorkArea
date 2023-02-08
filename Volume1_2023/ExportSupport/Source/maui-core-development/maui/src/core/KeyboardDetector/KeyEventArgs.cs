namespace Syncfusion.Maui.Core.Internals
{

	/// <summary>
	/// This class serves as an event data for the key down action on the view.
	/// </summary>
	public class KeyEventArgs
	{
		/// <summary>
		/// Initializes when <see cref="KeyEventArgs"/>.
		/// </summary>
		/// <param name="keyboardKey"></param>
		public KeyEventArgs(KeyboardKey keyboardKey)
		{
			this.Key = keyboardKey;
		}

		/// <summary>
		/// Returns key pressed.
		/// </summary>
		public KeyboardKey Key { get; }

		/// <summary>
		/// Gets or sets a value that marks the routed event as handled. A <b>true</b> value for <b>Handled</b> to restrict the event to be routed to parent.
		/// </summary>
		public bool Handled
		{
			get;
			set;
		}

		/// <summary>
		/// Returns the pressed state of <b>Shift</b> key
		/// </summary>
		public bool IsShiftKeyPressed
		{
			get;
			internal set;
		}

		/// <summary>
		/// Returns the pressed state of <b>Control</b> key
		/// </summary>
		public bool IsCtrlKeyPressed
		{
			get;
			internal set;
		}

		/// <summary>
		/// Returns the pressed state of<b>Alt</b> key.
		/// </summary>
		public bool IsAltKeyPressed
		{
			get;
			internal set;
		}

        /// <summary>
        /// Returns the pressed state of <b>Command</b> key.
        /// </summary>
        /// <remarks>This property is only applicable to the iOS platform.</remarks>
        public bool IsCommandKeyPressed
        {
            get;
            internal set;
        }

    }
}

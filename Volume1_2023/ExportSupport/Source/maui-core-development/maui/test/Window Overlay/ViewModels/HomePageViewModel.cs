using System.ComponentModel;
using System.Windows.Input;

namespace WindowOverlay.Samples
{
    public class HomePageViewModel : INotifyPropertyChanged
    {
        private object? selectedItem;

        public event PropertyChangedEventHandler? PropertyChanged;

        public HomePageViewModel()
        {
            SelectionChangedCommand = new Command<WindowOverlayPositions>(OnSelectionChanged);
        }

        public ICommand SelectionChangedCommand { get; set; }

        public object? SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        public Array Source
        {
            get => Enum.GetValues(typeof(WindowOverlayPositions));
        }

        protected void OnPropertyChanged(string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnSelectionChanged(WindowOverlayPositions item)
        {
            switch (item)
            {
                case WindowOverlayPositions.Overlay:
                    Application.Current?.MainPage?.Navigation.PushAsync(new OverlayPage());
                    break;

                case WindowOverlayPositions.Absolute:
                    Application.Current?.MainPage?.Navigation.PushAsync(new AbsoluteOverlayPage());
                    break;

                case WindowOverlayPositions.RelativePosition:
                    Application.Current?.MainPage?.Navigation.PushAsync(new RelativePositioningPage());
                    break;

                case WindowOverlayPositions.Fill:
                    Application.Current?.MainPage?.Navigation.PushAsync(new FillOverlayPage());
                    break;

                case WindowOverlayPositions.Top:
                    Application.Current?.MainPage?.Navigation.PushAsync(new TopOverlayPage());
                    break;

                case WindowOverlayPositions.Framework:
                    Application.Current?.MainPage?.Navigation.PushAsync(new FrameworkWindowOverlaySample());
                    break;
            }
        }
    }
}

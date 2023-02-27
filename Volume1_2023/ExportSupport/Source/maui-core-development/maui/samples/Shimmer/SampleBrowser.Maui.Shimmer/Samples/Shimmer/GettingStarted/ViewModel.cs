using Syncfusion.Maui.Shimmer;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SampleBrowser.Maui.Shimmer
{
    public class ViewModel : INotifyPropertyChanged
    {
        private int waveWidth = 200;

        private Color waveColor = Color.FromArgb("#D1D1D1");

        private Color shimmerColor = Color.FromArgb("#EBEBEB");

        private int duration = 1000;

        public int WaveWidth
        {
            get { return waveWidth; }
            set
            {
                waveWidth = value;
                RaisePropertyChanged("WaveWidth");
            }
        }

        public int Duration
        {
            get { return duration; }
            set
            {
                duration = value;
                RaisePropertyChanged("Duration");
            }
        }

        public Color WaveColor
        {
            get { return waveColor; }
            set
            {
                waveColor = value;
                RaisePropertyChanged("WaveColor");
            }
        }

        public Color ShimmerColor
        {
            get { return shimmerColor; }
            set
            {
                shimmerColor = value;
                RaisePropertyChanged("ShimmerColor");
            }
        }

        public ObservableCollection<Color> ShimmerColors { get; set; }

        public List<Color> WaveColors { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public Array ShimmerTypes
        {
            get
            {
                return Enum.GetValues(typeof(ShimmerType));
            }
        }

        public Array WaveDirectionTypes
        {
            get
            {
                return Enum.GetValues(typeof(ShimmerWaveDirection));
            }
        }

        private void RaisePropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }

        public ViewModel()
        {
            ShimmerColors = new ObservableCollection<Color>
            {
                Color.FromArgb("#EBEBEB"),
                Color.FromArgb("#E7E7F9"),
                Color.FromArgb("#E1EFFF"),
                Color.FromArgb("#F4E2EE"),
                Color.FromArgb("#F6F6DB"),
            };

            WaveColors = new List<Color>
            {
                Color.FromArgb("#D1D1D1"),
                Color.FromArgb("#CECEEF"),
                Color.FromArgb("#B8D4F2"),
                Color.FromArgb("#DFBFD5"),
                Color.FromArgb("#E7E7B2"),
            };
        }
    }
}

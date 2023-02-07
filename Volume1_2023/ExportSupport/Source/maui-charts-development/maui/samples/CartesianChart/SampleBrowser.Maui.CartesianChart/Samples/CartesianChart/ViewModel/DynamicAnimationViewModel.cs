﻿using System.Collections.ObjectModel;
using Microsoft.Maui.Dispatching;

namespace SampleBrowser.Maui.CartesianChart.SfCartesianChart
{
    internal class DynamicAnimationViewModel : BaseViewModel
    {
        private ObservableCollection<ChartDataModel> motionAnimation = new ObservableCollection<ChartDataModel>();
        public ObservableCollection<ChartDataModel> MotionAnimation
        {
            get { return motionAnimation; }
            set
            {
                motionAnimation = value;
                OnPropertyChanged("MotionAnimation");
            }
        }

        private bool canStopTimer;

        public DynamicAnimationViewModel()
        {
            var r = new Random();
            MotionAnimation = new ObservableCollection<ChartDataModel>();
            for (int i = 0; i < 7; i++)
            {
                MotionAnimation.Add(new ChartDataModel(i, r.Next(5, 90)));
            }
        }

        public void StopTimer()
        {
            canStopTimer = true;
        }

        public async void StartTimer()
        {
            await Task.Delay(500);
            if (Application.Current != null)
                Application.Current.Dispatcher.StartTimer(new TimeSpan(0, 0, 0, 2, 500), UpdateData);

            canStopTimer = false;
        }

        private bool UpdateData()
        {
            if (canStopTimer) return false;

            var r = new Random();
            var data = new ObservableCollection<ChartDataModel>();
            for (int i = 0; i < 7; i++)
            {
                data.Add(new ChartDataModel(i, r.Next(5, 90)));
            }

            MotionAnimation = data;

            return true;
        }
    }
}

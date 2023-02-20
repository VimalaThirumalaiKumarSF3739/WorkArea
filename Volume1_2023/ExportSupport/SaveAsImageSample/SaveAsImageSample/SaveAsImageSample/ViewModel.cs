using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveAsImageSample
{
    internal class ViewModel
    {
        public ObservableCollection<Model> Data { get; set; }
        public ObservableCollection<Model> Data1 { get; set; }

        public ViewModel()
        {
            Data = new ObservableCollection<Model>()
            {
                new Model { Name = "David", Height = 170 },
                new Model { Name = "Michael", Height = 96 },
                new Model { Name = "Steve", Height = 65 },
                new Model { Name = "Joel", Height = 182 },
                new Model { Name = "Bob", Height = 200 }
            };
        }
    }

    public class Model
    {
        public string Name { get; set; }
        public double Height { get; set; }
    }
}

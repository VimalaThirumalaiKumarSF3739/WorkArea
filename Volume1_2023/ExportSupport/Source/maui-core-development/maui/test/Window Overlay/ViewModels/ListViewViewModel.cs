namespace WindowOverlay.Samples
{
    public class ListViewModel
    {
        public ListViewModel()
        {
            Source = new List<int>();

            for (int i = 1; i <= 100; i++)
            {
                Source.Add(i);
            }
        }

        public List<int> Source { get; set; }
    }
}

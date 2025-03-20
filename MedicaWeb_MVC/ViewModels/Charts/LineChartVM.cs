namespace MedicaWeb_MVC.ViewModels.Charts
{
    public class LineChartVM
    {
        public List<string> Dates { get; set; }
        public List<SeriesVM> Series { get; set; }
    }

    public class SeriesVM
    {
        public string Name { get; set; }
        public List<int> Data { get; set; } = new();
    }
}

namespace PlantsWpf.ListViewExtensions
{
    public class MatrixColumn
    {
        public string Name { get; set; }
        public string StringFormat { get; set; }

        public MatrixColumn(string name, string stringFormat)
        {
            Name = name;
            StringFormat = stringFormat;
        }
    }
}
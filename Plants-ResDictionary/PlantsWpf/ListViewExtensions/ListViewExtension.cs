using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace PlantsWpf.ListViewExtensions
{
    public static class ListViewExtension
    {
        public static readonly DependencyProperty MatrixSourceProperty =
            DependencyProperty.RegisterAttached("MatrixSource",
                typeof (DataMatrix), typeof (ListView),
                new FrameworkPropertyMetadata(null,
                    OnMatrixSourceChanged));

        public static DataMatrix GetMatrixSource(this DependencyObject d)
        {
            return (DataMatrix) d.GetValue(MatrixSourceProperty);
        }

        public static void SetMatrixSource(this DependencyObject d, DataMatrix value)
        {
            d.SetValue(MatrixSourceProperty, value);
        }

        private static void OnMatrixSourceChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            ListView listView = d as ListView;
            DataMatrix dataMatrix = e.NewValue as DataMatrix;

            if (listView != null)
            {
                listView.ItemsSource = dataMatrix;
                GridView gridView = listView.View as GridView;
                int count = 0;
                if (gridView != null)
                {
                    gridView.Columns.Clear();
                    if (dataMatrix != null)
                    {
                        foreach (MatrixColumn col in dataMatrix.Columns)
                        {
                            gridView.Columns.Add(
                                new GridViewColumn
                                {
                                    Header = col.Name,
                                    DisplayMemberBinding = new Binding(string.Format("[{0}]", count))
                                });
                            count++;
                        }
                    }
                }
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;

namespace PlantsWpf.ListViewExtensions
{
    public class DataMatrix : IEnumerable
    {
        public List<MatrixColumn> Columns { get; set; }
        public List<object[]> Rows { get; set; }

        public DataMatrix(List<MatrixColumn> columns, List<object[]> rows)
        {
            Columns = columns;
            Rows = rows;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new GenericEnumerator(Rows.ToArray());
        }
    }
}
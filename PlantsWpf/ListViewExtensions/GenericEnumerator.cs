using System;
using System.Collections;

namespace PlantsWpf.ListViewExtensions
{
    internal class GenericEnumerator : IEnumerator
    {
        private readonly object[] _list;
        private int _position = -1;

        public GenericEnumerator(object[][] list)
        {
            _list = list;
        }

        public bool MoveNext()
        {
            _position++;
            return (_position < _list.Length);
        }

        public void Reset()
        {
            _position = -1;
        }

        public object Current
        {
            get
            {
                try
                {
                    return _list[_position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}
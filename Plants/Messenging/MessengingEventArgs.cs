using System;

namespace Planting.Messenging
{
    public class MessengingEventArgs<T> : EventArgs where T: class
    {
        public MessengingEventArgs(T obj)
        {
            Object = obj;
        }

        public T Object { get; set; }
    }
}

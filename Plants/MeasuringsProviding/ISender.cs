using System;

namespace Planting.MeasuringsProviding
{
    public interface ISender<in T> where T: class 
    {
        event EventHandler MessageSending;    
        void OnMessageSending(T message);
    }
}
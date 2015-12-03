using System;

namespace PlantingLib.MeasuringsProviders
{
    public interface ISender<in T> where T: class 
    {
        event EventHandler MessageSending;    
        void OnMessageSending(T message);
    }
}
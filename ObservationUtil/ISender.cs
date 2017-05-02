using System;

namespace ObservationUtil
{
    public interface ISender<in T> where T : class
    {
        event EventHandler MessageSending;
        void OnMessageSending(T message);
    }
}
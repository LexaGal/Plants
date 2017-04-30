using System;

namespace ObservationUtil
{
    public interface IReciever
    {
        void RecieveMessage(object sender, EventArgs eventArgs);
    }
}
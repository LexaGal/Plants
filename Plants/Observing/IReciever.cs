using System;

namespace Planting.Observing
{
    public interface IReciever
    {
        void RecieveMessage(object sender, EventArgs eventArgs);
    }
}
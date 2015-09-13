using System;

namespace Planting.Observation
{
    public interface IReciever
    {
        void RecieveMessage(object sender, EventArgs eventArgs);
    }
}
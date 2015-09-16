using System;

namespace PlantingLib.Observation
{
    public interface IReciever
    {
        void RecieveMessage(object sender, EventArgs eventArgs);
    }
}
using System;

namespace PlantsWpf.ControlsBuilders
{
    public interface IControlsRefresher
    {
        event EventHandler RefreshControl;
        void OnRefreshControls();
    }
}
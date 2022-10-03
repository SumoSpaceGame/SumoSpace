using System;

namespace Game.Common.Settings
{
    [Serializable]
    public struct NetworkSettings
    {
        public bool isServer;
        /*
        public ulong _updateInterval;
        public ulong updateInterval
        {
            get
            {
                return _updateInterval;
            }

            set
            {
                _updateInterval = value;
                OnUpdateIntervalChange?.Invoke(value);
            }
        }


        public delegate void UpdateIntervalChangeEvent(ulong amount);
        public UpdateIntervalChangeEvent OnUpdateIntervalChange;
        */
    }
}

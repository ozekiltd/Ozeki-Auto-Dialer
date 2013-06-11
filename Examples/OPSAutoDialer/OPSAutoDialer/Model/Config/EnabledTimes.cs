using System;
using OzCommon.Model;

namespace OPSAutoDialer.Model.Config
{
    public class EnabledTimes : BaseModel, ICloneable
    {
        #region Properties

        private bool timeIntervalEnabled;
        private DateTime earliestTime;
        private DateTime latestTime;

        #region Properties => OnPropertyChanged
        public bool TimeIntervalEnabled
        {
            get { return timeIntervalEnabled; }
            set { timeIntervalEnabled = value; Raise(()=>TimeIntervalEnabled); }
        }

        public DateTime LatestTime
        {
            get { return latestTime; }
            set { latestTime = value; Raise(()=>LatestTime); }
        }

        public DateTime EarliestTime
        {
            get { return earliestTime; }
            set { earliestTime = value; Raise(()=>EarliestTime); }
        }
        #endregion

        #endregion

        public bool IsValid
        {
            get
            {
                return true;
            }
        }

        public object Clone()
        {
            return new EnabledTimes
            {
                TimeIntervalEnabled = this.TimeIntervalEnabled,
                EarliestTime = this.EarliestTime,
                LatestTime = this.LatestTime
            };
        }
    }
}

using System;
using System.ComponentModel;
using OzCommon.Model;

namespace OPSAutoDialer.Model.Config
{
    public class AutoDialerConfig : BaseModel, ICloneable, IDataErrorInfo
    {
        #region Properties

        public EnabledDays EnabledDays { get; set; }
        public EnabledTimes EnabledTimes { get; set; }
        public IVRSettings IVRSettings { get; set; }
       
        private string extensionId;
        private Int32 ringingTime;
        private Int32 concurrentWorks;

        #region Properties => OnPropertyChanged

        public string ExtensionId
        {
            get { return extensionId; }
            set { extensionId = value; Raise(()=> ExtensionId); }
        }

        public Int32 RingingTime
        {
            get { return ringingTime; }
            set { ringingTime = value; Raise(()=>RingingTime); }
        }

        public Int32 ConcurrentWorks
        {
            get { return concurrentWorks; }
            set { concurrentWorks = value; Raise(() => ConcurrentWorks); }
        }  

        #endregion

        #endregion

        public AutoDialerConfig()
        {
            ConcurrentWorks = 1;
            RingingTime = 30;
            
            EnabledDays = new EnabledDays();
            EnabledTimes = new EnabledTimes();
            IVRSettings = new IVRSettings();
        }

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "RingingTime":
                        if (!(RingingTime >= 30 && RingingTime <= 600))
                            return "Ringing time must be between 30 and 600";
                        break;
                }

                return null;
            }
        }

        public string Error { get; set; }

        public bool IsValid
        {
            get
            {
                return (RingingTime >= 30 && RingingTime <= 600) && (ConcurrentWorks > 0 && EnabledDays.IsValid && EnabledTimes.IsValid && IVRSettings.IsValid);
            }
        }

        public object Clone()
        {
            return new AutoDialerConfig
            {
                EnabledDays = (EnabledDays)EnabledDays.Clone(),
                EnabledTimes = (EnabledTimes)EnabledTimes.Clone(),
                IVRSettings = (IVRSettings)IVRSettings.Clone(),

                ConcurrentWorks = this.ConcurrentWorks,
                RingingTime = this.RingingTime,
                ExtensionId =  ExtensionId,
            };
        }
    }
}

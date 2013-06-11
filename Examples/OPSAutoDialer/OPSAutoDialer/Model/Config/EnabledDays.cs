using System;
using System.ComponentModel;
using OzCommon.Model;

namespace OPSAutoDialer.Model.Config
{
    public class EnabledDays : BaseModel, ICloneable, IDataErrorInfo
    {
        #region Properties

        private bool daysIntervalEnabled;
        private bool mondayEnabled;
        private bool thursdayEnabled;
        private bool tuesdayEnabled;
        private bool fridayEnabled;
        private bool sundayEnabled;
        private bool wednesdayEnabled;
        private bool saturdayEnabled;

        #region Properties => OnPropertyChanged

        public bool DaysIntervalEnabled
        {
            get { return daysIntervalEnabled; }
            set { daysIntervalEnabled = value; Raise(()=>DaysIntervalEnabled); }
        }

        public bool MondayEnabled
        {
            get { return mondayEnabled; }
            set { mondayEnabled = value; Raise(()=>MondayEnabled); }
        }

        public bool TuesdayEnabled
        {
            get { return tuesdayEnabled; }
            set { tuesdayEnabled = value; Raise(()=>TuesdayEnabled); }
        }

        public bool WednesdayEnabled
        {
            get { return wednesdayEnabled; }
            set { wednesdayEnabled = value; Raise(()=>WednesdayEnabled); }
        }

        public bool ThursdayEnabled
        {
            get { return thursdayEnabled; }
            set { thursdayEnabled = value; Raise(()=>ThursdayEnabled); }
        }

        public bool FridayEnabled
        {
            get { return fridayEnabled; }
            set { fridayEnabled = value; Raise(()=>FridayEnabled); }
        }

        public bool SaturdayEnabled
        {
            get { return saturdayEnabled; }
            set { saturdayEnabled = value; Raise(()=>SaturdayEnabled); }
        }

        public bool SundayEnabled
        {
            get { return sundayEnabled; }
            set { sundayEnabled = value; Raise(()=>SundayEnabled); }
        }
        #endregion

        #endregion

        public bool IsValid
        {
            get
            {
                if (!DaysIntervalEnabled) return true;
                else return ( MondayEnabled || TuesdayEnabled || WednesdayEnabled || ThursdayEnabled || FridayEnabled || SaturdayEnabled || SundayEnabled );
            }
        }

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    default:
                        if (DaysIntervalEnabled && !MondayEnabled && !TuesdayEnabled && !WednesdayEnabled && !ThursdayEnabled && !FridayEnabled && !SaturdayEnabled && !SundayEnabled)
                            return "You did't choose enabled day.";
                        else return "";

                }
            }
        }

        public string Error { get; set; }

        public object Clone()
        {
            return new EnabledDays {
                                       DaysIntervalEnabled = this.DaysIntervalEnabled,
                                       MondayEnabled = this.MondayEnabled,
                                       ThursdayEnabled = this.ThursdayEnabled,
                                       TuesdayEnabled = this.TuesdayEnabled,
                                       FridayEnabled = this.FridayEnabled,
                                       SundayEnabled = this.SundayEnabled,
                                       WednesdayEnabled = this.WednesdayEnabled,
                                       SaturdayEnabled = this.SaturdayEnabled
                                   };
        }

    }
}

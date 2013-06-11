using System.ComponentModel;
using OzCommon.Model;
using OzCommonBroadcasts.Model;

namespace OPSAutoDialer.Model
{
    public class DialerEntry : BaseModel, ICompletedWork, IDataErrorInfo
    {
        #region Properties

        string dialedNumber;
        WorkState state;
        bool isCompleted;
        string soundPath;
        int _progress;
        int _pressedNumber;

        #region Properties with OnPropertyChanged


        public string DialedNumber
        {
            get
            {
                return dialedNumber;
            }
            set
            {
                dialedNumber = value;
                Raise(() => DialedNumber);
            }
        }

        public string SoundPath
        {
            get { return soundPath; }
            set
            {
                soundPath = value;
                Raise(() => SoundPath);
            }
        }

        [ReadOnlyProperty]
        public int Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                Raise(()=> Progress);
            }
        }

        [ReadOnlyProperty]
        public int PressedNumber
        {
            get { return _pressedNumber; }
            set { 
                  _pressedNumber = value;
                  Raise(()=> PressedNumber); 
                }
        }

        [ReadOnlyProperty]
        [ExportIgnoreProperty]
        public WorkState State
        {
            get { return state; }
            set
            {
                state = value;
                Raise(() => State);
            }
        }

        [InvisibleProperty]
        [ExportIgnoreProperty]
        public bool IsCompleted
        {
            get { return isCompleted; }
            set
            {
                isCompleted = value;
                Raise(() => IsCompleted);
            }
        }

        [InvisibleProperty]
        [ExportIgnoreProperty]
        public bool IsValid {

            get { return !string.IsNullOrWhiteSpace(DialedNumber) && !string.IsNullOrWhiteSpace(SoundPath); }
        }

        #endregion

        [InvisibleProperty]
        [ExportIgnoreProperty]
        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "DialedNumber":
                        if (string.IsNullOrWhiteSpace(DialedNumber))
                            return "DialedNumber cannot be empty";
                        break;
                    case "SoundPath":
                        if (string.IsNullOrWhiteSpace(SoundPath))
                            return "SoundPath cannot be empty";
                        break;
                }
                return null;
            }
        }

        [InvisibleProperty]
        [ExportIgnoreProperty]
        public string Error { get; private set; }

        #endregion

        public DialerEntry()
        {
            state = WorkState.Init;
        }


    }
}

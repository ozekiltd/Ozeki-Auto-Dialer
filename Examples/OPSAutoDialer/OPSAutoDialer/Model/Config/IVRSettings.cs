using System;
using OzCommon.Model;

namespace OPSAutoDialer.Model.Config
{
    public class IVRSettings : BaseModel, ICloneable
    {
        #region Properties

        private bool ivrEnabled;
        private string zeroRedirection;
        private string oneRedirection;
        private string twoRedirection;
        private string threeRedirection;
        private string fourRedirection;
        private string fiveRedirection;
        private string sixRedirection;
        private string sevenRedirection;
        private string eightRedirection;
        private string nineRedirection;
        private string hashRedirection;
        private string starRedirection;

        #region Properties => OnPropertyChanged
        public Boolean IVREnabled
        {
            get { return ivrEnabled; }
            set { ivrEnabled = value; Raise(()=>IVREnabled); }
        }

        public String ZeroRedirection
        {
            get { return zeroRedirection; }
            set { zeroRedirection = value; Raise(()=>ZeroRedirection); }
        }

        public String OneRedirection
        {
            get { return oneRedirection; }
            set { oneRedirection = value; Raise(()=>OneRedirection); }
        }

        public String TwoRedirection
        {
            get { return twoRedirection; }
            set { twoRedirection = value; Raise(()=>TwoRedirection); }
        }

        public String ThreeRedirection
        {
            get { return threeRedirection; }
            set { threeRedirection = value; Raise(()=>ThreeRedirection); }
        }

        public String FourRedirection
        {
            get { return fourRedirection; }
            set { fourRedirection = value; Raise(()=>FourRedirection); }
        }

        public String FiveRedirection
        {
            get { return fiveRedirection; }
            set { fiveRedirection = value; Raise(()=>FiveRedirection); }
        }

        public String SixRedirection
        {
            get { return sixRedirection; }
            set { sixRedirection = value; Raise(()=>SixRedirection); }
        }

        public String SevenRedirection
        {
            get { return sevenRedirection; }
            set { sevenRedirection = value; Raise(()=>SevenRedirection); }
        }

        public String EightRedirection
        {
            get { return eightRedirection; }
            set { eightRedirection = value; Raise(()=>EightRedirection); }
        }

        public String NineRedirection
        {
            get { return nineRedirection; }
            set { nineRedirection = value; Raise(()=>NineRedirection); }
        }

        public String StarRedirection
        {
            get { return starRedirection; }
            set { starRedirection = value; Raise(()=>StarRedirection); }
        }

        public String HashRedirection
        {
            get { return hashRedirection; }
            set { hashRedirection = value; Raise(()=>HashRedirection); }
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
            return new IVRSettings {
                                       ivrEnabled = this.IVREnabled,
                                       zeroRedirection = this.ZeroRedirection,
                                       oneRedirection = this.OneRedirection,
                                       twoRedirection = this.TwoRedirection,
                                       threeRedirection = this.ThreeRedirection,
                                       fourRedirection = this.FourRedirection,
                                       fiveRedirection = this.FiveRedirection,
                                       sixRedirection = this.SixRedirection,
                                       sevenRedirection = this.SevenRedirection,
                                       eightRedirection = this.EightRedirection,
                                       nineRedirection = this.NineRedirection,
                                       hashRedirection = this.HashRedirection,
                                       starRedirection = this.StarRedirection
                                   };
        }

    }
}

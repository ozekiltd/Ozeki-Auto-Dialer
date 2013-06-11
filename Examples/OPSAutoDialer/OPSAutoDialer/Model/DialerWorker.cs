using System;
using System.Threading;
using GalaSoft.MvvmLight.Ioc;
using OPSAutoDialer.Model.Config;
using OPSSDK;
using OPSSDKCommon.Model.Call;
using OzCommon.Model;
using OzCommon.Utils.Schedule;
using Ozeki.Media.MediaHandlers;
using Timer = System.Timers.Timer;

namespace OPSAutoDialer.Model
{
    class DialerWorker : IWorker
    {
        DialerEntry entry;
        IAPIExtension extension;
        ICall call;
        AutoDialerConfig config;
        WaveStreamPlayback waveStreamPlayback;
        Timer progressTimer;
        Timer ringingTimer;

        public DialerWorker(DialerEntry entry, IAPIExtension extension)
        {
            this.entry = entry;
            this.extension = extension;


            config = SimpleIoc.Default.GetInstance<IGenericSettingsRepository<AutoDialerConfig>>().GetSettings() ?? new AutoDialerConfig();

            ringingTimer = new Timer(config.RingingTime*1000);
            ringingTimer.Elapsed += ringingTimer_Elapsed;

            progressTimer = new Timer(1000);
            progressTimer.AutoReset = true;
            progressTimer.Elapsed += progressTimer_Elapsed;

        }

        public event EventHandler<WorkResult> WorkCompleted;

        private bool started;
        public void StartWork()
        {
            if (entry.State == WorkState.Success || entry.State == WorkState.HungUpByRemote)
            {
                OnWorkCompleted(entry.State);
                return;
            }

            if (!entry.IsValid)
            {
                OnWorkCompleted(entry.State);
                return;
            }

            try
            {
                started = true;
                WaitForValidDateEndTime();

                entry.Progress = 0;
                entry.State = WorkState.Init;
                
                ringingTimer.Start();
                call = extension.CreateCall(entry.DialedNumber);

                call.CallStateChanged += call_CallStateChanged;
                call.DtmfStarted += call_DtmfStarted;
                call.CallErrorOccurred += call_CallErrorOccurred;


                call.Start();
            }
            catch (Exception ex)
            {
                OnWorkCompleted(WorkState.CallError);
            }

        }

        public void CancelWork()
        {
            started = false;
            OnWorkCompleted(WorkState.Cancelled);
        }

        void ringingTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            OnWorkCompleted(WorkState.RingingTimeExpired);
        }


        void call_CallErrorOccurred(object sender, Ozeki.VoIP.VoIPEventArgs<CallError> e)
        {
            if (e.Item == CallError.NotFound)
                OnWorkCompleted(WorkState.NotFound);
            else
                OnWorkCompleted(WorkState.CallError);

       
        }

        void call_DtmfStarted(object sender, Ozeki.VoIP.VoIPEventArgs<DtmfSignal> e)
        {
            string target = null;

            if(config.IVRSettings == null)
                return;

            var ivr = config.IVRSettings;

            switch (e.Item.Signal)
            {
                case DtmfNamedEvents.Dtmf0:
                    target = ivr.ZeroRedirection;
                    break;
                case DtmfNamedEvents.Dtmf1:
                    target = ivr.OneRedirection;
                    break;
                case DtmfNamedEvents.Dtmf2:
                    target = ivr.TwoRedirection;
                    break;
                case DtmfNamedEvents.Dtmf3:
                    target = ivr.ThreeRedirection;
                    break;
                case DtmfNamedEvents.Dtmf4:
                    target = ivr.FourRedirection;
                    break;
                case DtmfNamedEvents.Dtmf5:
                    target = ivr.FiveRedirection;
                    break;
                case DtmfNamedEvents.Dtmf6:
                    target = ivr.SixRedirection;
                    break;
                case DtmfNamedEvents.Dtmf7:
                    target = ivr.SevenRedirection;
                    break;
                case DtmfNamedEvents.Dtmf8:
                    target = ivr.EightRedirection;
                    break;
                case DtmfNamedEvents.Dtmf9:
                    target = ivr.NineRedirection;
                    break;
            }

            entry.PressedNumber = (int)e.Item.Signal;

            if(!string.IsNullOrEmpty(target))
                call.BlindTransfer(target);
        }

        private bool firstInCall = true;
        void call_CallStateChanged(object sender, Ozeki.VoIP.VoIPEventArgs<CallState> e)
        {
            if(e.Item.IsInCall())
            {
                if(firstInCall)
                {
                    try
                    {
                        entry.State = WorkState.InProgress;
                        firstInCall = false;
                        ringingTimer.Elapsed -= ringingTimer_Elapsed;
                        waveStreamPlayback = new WaveStreamPlayback(entry.SoundPath);
                        call.ConnectAudioSender(waveStreamPlayback);

                        waveStreamPlayback.Stopped += waveStreamPlayback_Stopped;

                        waveStreamPlayback.StartStreaming();
                        progressTimer.Start();
                    }
                    catch (Exception ex)
                    {
                        OnWorkCompleted(WorkState.FileError);   
                    }
                }
            }
            else if(e.Item.IsCallEnded())
            {
                OnWorkCompleted(WorkState.HungUpByRemote);
            }
       
        }

        void progressTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                entry.Progress = (int)((double)waveStreamPlayback.Stream.Position / waveStreamPlayback.Stream.Length * 100);
            }
            catch
            {
                
            }
        }

        void waveStreamPlayback_Stopped(object sender, EventArgs e)
        {
            entry.Progress = 100;
            OnWorkCompleted(WorkState.Success);
        }

        void WaitForValidDateEndTime()
        {
            if (config.EnabledDays.DaysIntervalEnabled || config.EnabledTimes.TimeIntervalEnabled)
                entry.State = WorkState.WaitForValidDate;
            else
                return;

            int dayOfWeek = 0;
            int currentTimeInMinute = 0;
            int earliestTimeInMinute = 0;
            int latestTimeInMinute = 0;

            var currentDay = (int)DateTime.Now.DayOfWeek;

            bool validDate = false;
            bool validTime = false;

            while (started)
            {

                if (config.EnabledDays.DaysIntervalEnabled)
                {
                    validDate = false;
                    dayOfWeek = (int)DateTime.Now.DayOfWeek;

                    switch (dayOfWeek)
                    {
                        case 0:
                            if (config.EnabledDays.SundayEnabled && currentDay == 0)
                                validDate = true;
                            break;
                        case 1:
                            if (config.EnabledDays.MondayEnabled && currentDay == 1)
                                validDate = true;
                            break;
                        case 2:
                            if (config.EnabledDays.TuesdayEnabled && currentDay == 2)
                                validDate = true;
                            break;
                        case 3:
                            if (config.EnabledDays.WednesdayEnabled && currentDay == 3)
                                validDate = true;
                            break;
                        case 4:
                            if (config.EnabledDays.ThursdayEnabled && currentDay == 4)
                                validDate = true;
                            break;
                        case 5:
                            if (config.EnabledDays.FridayEnabled && currentDay == 5)
                                validDate = true;
                            break;
                        case 6:
                            if (config.EnabledDays.SaturdayEnabled && currentDay == 6)
                                validDate = true;
                            break;
                    }
                }
                else
                    validDate = true;

                if (config.EnabledTimes.TimeIntervalEnabled)
                {
                    validTime = false;

                    var currentTime = DateTime.Now;

                    var earliestTime = config.EnabledTimes.EarliestTime;
                    var latestTime = config.EnabledTimes.LatestTime;

                    earliestTimeInMinute = 60 * earliestTime.Hour +
                                           earliestTime.Minute;

                    if (earliestTime.Hour > latestTime.Hour
                        ||
                        (earliestTime.Hour == latestTime.Hour && earliestTime.Minute > latestTime.Minute))
                            latestTimeInMinute = 60 * latestTime.Hour + latestTime.Minute + 1440;
                    else
                        latestTimeInMinute = 60 * latestTime.Hour + latestTime.Minute;

                    if (earliestTime.Hour > currentTime.Hour || earliestTime.Hour == currentTime.Hour &&
                        earliestTime.Hour > currentTime.Hour)
                        currentTimeInMinute = 60 * currentTime.Hour + currentTime.Minute + 1440;
                    else
                        currentTimeInMinute = 60 * currentTime.Hour + currentTime.Minute;

                    if (earliestTimeInMinute <= currentTimeInMinute && currentTimeInMinute <= latestTimeInMinute)
                        validTime = true;
                }
                else
                    validTime = true;

                if (validDate && validTime)
                    break;

                Thread.Sleep(50);

            }
        }

        int completed;
        void OnWorkCompleted(WorkState e)
        {
            if(Interlocked.Exchange(ref completed, 1) != 0)
                return;

            entry.State = e;
            entry.IsCompleted = e == WorkState.Success || e == WorkState.HungUpByRemote;
            
            var handler = WorkCompleted;
            if (handler != null)
                handler(this, new WorkResult() { IsSuccess = entry.IsCompleted });

            CleanUp();
        }

        void CleanUp()
        {
            try
            {
                ringingTimer.Dispose();

                progressTimer.Elapsed -= progressTimer_Elapsed;
                progressTimer.Dispose();
             
                if(waveStreamPlayback != null)
                {
                    waveStreamPlayback.Stopped -= waveStreamPlayback_Stopped;
                    waveStreamPlayback.Dispose();
                }
                  
                if(call != null)
                    call.HangUp();
            }
            catch (Exception)
            {
                
            }
        }



    }
}

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using OzCommon.Model;
using OzCommon.ViewModel;
using OPSAutoDialer.Model.Config;


namespace OPSAutoDialer.ViewModel
{
    class SettingsViewModel : ViewModelBase
    {
        public RelayCommand CancelCommand { get; protected set; }
        public RelayCommand SaveCommand { get; protected set; }
        IGenericSettingsRepository<AutoDialerConfig> settingsRepository;

        public AutoDialerConfig AutoDialerConfig { get; set; }

        public SettingsViewModel()
        {
            InitCommands();
            settingsRepository = SimpleIoc.Default.GetInstance<IGenericSettingsRepository<AutoDialerConfig>>();
            GetSettings();
        }

        private void GetSettings()
        {
            AutoDialerConfig = new AutoDialerConfig();

            var config = settingsRepository.GetSettings();
            if (config != null)
                AutoDialerConfig = config.Clone() as AutoDialerConfig;
        }

        public void InitCommands()
        {
            CancelCommand = new RelayCommand(() => Messenger.Default.Send(new NotificationMessage(Messages.DismissSettingsWindow)));
            SaveCommand = new RelayCommand(() => { 
                                                    //TODO gomb frissítés
                                                    settingsRepository.SetSettings(AutoDialerConfig);
                                                    Messenger.Default.Send(new NotificationMessage(Messages.DismissSettingsWindow)); 
                                                 }, () => AutoDialerConfig.IsValid);
        }
    }
}

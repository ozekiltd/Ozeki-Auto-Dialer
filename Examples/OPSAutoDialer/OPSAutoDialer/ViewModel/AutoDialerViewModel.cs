using GalaSoft.MvvmLight.Ioc;
using OPSAutoDialer.Model;
using OPSAutoDialer.Model.Config;
using OPSSDK;
using OzCommon.Model;
using OzCommon.Utils;
using OzCommonBroadcasts.ViewModel;

namespace OPSAutoDialer.ViewModel
{
    public class AutoDialerViewModel : BroadcastMainViewModel<DialerEntry>
    {
        private readonly IGenericSettingsRepository<AutoDialerConfig> settingsRepository;

        private IClient client;
        public AutoDialerViewModel()
        {
            settingsRepository = SimpleIoc.Default.GetInstance<IGenericSettingsRepository<AutoDialerConfig>>();
        }

        protected override object GetSettingsViewModel()
        {
            return new SettingsViewModel();
        }

        protected override int GetMaxConcurrentWorkers()
        {
            var config = settingsRepository.GetSettings();
            return config != null ? config.ConcurrentWorks : 1; //Default value is 1
        }

        protected override string GetApiExtensionID()
        {
            var settings = settingsRepository.GetSettings();
            return settings == null ? "" : settings.ExtensionId;
        }
    }
}

using GalaSoft.MvvmLight.Ioc;
using OPSSDK;
using OzCommon.Utils.Schedule;
using OzCommonBroadcasts.Model;

namespace OPSAutoDialer.Model
{
    public class AutoDialerWorkerFactory : IWorkerFactory<DialerEntry>
    {
        private IExtensionContainer extensionContainer;
        public AutoDialerWorkerFactory()
        {
            extensionContainer = SimpleIoc.Default.GetInstance<IExtensionContainer>();
        }
        public IWorker CreateWorker(DialerEntry work)
        {
            return new DialerWorker(work, extensionContainer.GetExtension());
        }
    }
}   

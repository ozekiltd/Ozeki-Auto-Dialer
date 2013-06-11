using System;
using System.Windows;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using OPSAutoDialer.Model;
using OPSAutoDialer.Model.Config;
using OPSAutoDialer.ViewModel;
using OzCommon.Model;
using OzCommon.Model.Mock;
using OzCommon.Utils;
using OzCommon.Utils.DialogService;
using OzCommon.Utils.Schedule;
using OzCommon.View;
using OzCommon.ViewModel;
using OzCommonBroadcasts.Model;
using OzCommonBroadcasts.Model.Csv;
using OzCommonBroadcasts.View;
using OzCommonBroadcasts.ViewModel;

namespace OPSAutoDialer
{

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        readonly SingletonApp singletonApp;

        public App()
        {
            singletonApp = new SingletonApp("OPSAutoDialer");

            InitDependencies();
        }

        void InitDependencies()
        {
            SimpleIoc.Default.Register<IDialogService>(() => new DialogService());
            SimpleIoc.Default.Register<IBroadcastMainViewModel>(() => new AutoDialerViewModel());
            SimpleIoc.Default.Register<ICsvImporter<DialerEntry>>(() => new CsvImporter<DialerEntry>());
            SimpleIoc.Default.Register<ICsvExporter<DialerEntry>>(() => new CsvExporter<DialerEntry>());
            SimpleIoc.Default.Register<IGenericSettingsRepository<AutoDialerConfig>>(() => new GenericSettingsRepository<AutoDialerConfig>());
            SimpleIoc.Default.Register<IWorkerFactory<DialerEntry>>(() => new AutoDialerWorkerFactory());
            SimpleIoc.Default.Register<IExtensionContainer>(() => new ExtensionContainer());
            SimpleIoc.Default.Register<IScheduler<DialerEntry>>(() => new Scheduler<DialerEntry>(SimpleIoc.Default.GetInstance<IWorkerFactory<DialerEntry>>()));
            SimpleIoc.Default.Register(() => new ApplicationInformation("Auto Dialer"));
            SimpleIoc.Default.Register<IUserInfoSettingsRepository>(() => new UserInfoSettingsRepository());
            SimpleIoc.Default.Register<IClient>(()=> new Client());
        }


        protected override void OnStartup(StartupEventArgs e)
        {
            Messenger.Default.Register<NotificationMessage>(this, MessageReceived);
            singletonApp.OnStartup(e);

            MainWindow = new LoginWindow();
            MainWindow.Show();
            base.OnStartup(e);
        }
        

        protected override void OnExit(ExitEventArgs e)
        {
            Messenger.Default.Unregister<NotificationMessage>(this, MessageReceived);
            base.OnExit(e);
        }

        private void MessageReceived(NotificationMessage notificationMessage)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (notificationMessage.Notification == Messages.NavigateToMainWindow)
                {
                    var mainWindow = new BroadcastMainWindow();
                    mainWindow.Show();

                    Current.MainWindow = mainWindow;
                }
            }));
        }
    }
}

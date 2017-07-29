using System;
using System.Linq;
using System.Reactive.Disposables;
using Windows.ApplicationModel.Core;
using Windows.Devices.Enumeration;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AxelSmash.Listeners;
using AxelSmash.SmashSources;

#pragma warning disable 4014

namespace AxelSmash
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ISmashSource source;
        private readonly CompositeDisposable subscriptions = new CompositeDisposable();

        public MainPage()
        {
            InitializeComponent();

            this.Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            source = new CompositeSmashSource(
                new ControllerSmashSource(),
                new CoreWindowKeysSmashSource(CoreWindow.GetForCurrentThread()));

            subscriptions.Add(source.Subscribe(new AudioSmashListener()));
            subscriptions.Add(source.Subscribe(new DrawingsSmashListener(FiguresCanvas)));
        }


        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            subscriptions?.Dispose();

            source?.Dispose();
            source = null;
        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var devices = await DeviceInformation.FindAllAsync(ProjectionManager.GetDeviceSelector());
            var dl = devices.ToList();
            //ProjectionManager.RequestStartProjectingAsync(dl.First().)

            //ApplicationView.GetForCurrentView().TryEnterFullScreenMode();

            var currentAV = ApplicationView.GetForCurrentView();
            var xw = Window.Current;
            var device = dl.ElementAt(0);
            
            var coreApplicationView = CoreApplication.CreateNewView();
            await coreApplicationView.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal,
                async () =>
                {
                    var newWindow = Window.Current;
                    var newAppView = ApplicationView.GetForCurrentView();
                    newAppView.Title = "New window " + device.Name;

                    var frame = new Frame();
                    frame.Navigate(typeof(MainPage), null);
                    newWindow.Content = frame;
                    await xw.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => xw.Activate());
                    await ApplicationViewSwitcher.TryShowAsStandaloneAsync(
                        newAppView.Id,
                        ViewSizePreference.UseMinimum,
                        currentAV.Id,
                        ViewSizePreference.UseMinimum);
                });
        }

        private async void ButtonBase2_OnClick(object sender, RoutedEventArgs e)
        {
            var devices = await DeviceInformation.FindAllAsync(ProjectionManager.GetDeviceSelector());
            var dl = devices.ToList();
            //ProjectionManager.RequestStartProjectingAsync(dl.First().)

            //ApplicationView.GetForCurrentView().TryEnterFullScreenMode();

            var currentAV = ApplicationView.GetForCurrentView();
            var xw = Window.Current;

            var previousId = currentAV.Id;
            int i = 0;
            foreach (var device in dl)
            {
                var coreApplicationView = CoreApplication.CreateNewView();
                await coreApplicationView.Dispatcher.RunAsync(
                    CoreDispatcherPriority.Normal,
                    async () =>
                    {
                        var newAppView = ApplicationView.GetForCurrentView();
                        newAppView.Title = "New window " + device.Name +" " + i++;

                        var frame = new Frame();
                        frame.Navigate(typeof(MainPage), null);
                        Window.Current.Content = frame;
                        //await xw.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => xw.Activate());


                        await ProjectionManager.StartProjectingAsync(newAppView.Id, previousId, device);
                        previousId = newAppView.Id;
                    });
                return;
            }
        }
    }
}

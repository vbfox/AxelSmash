using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Devices.Enumeration;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AxelSmash.Shapes;
#pragma warning disable 4014

namespace AxelSmash
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ControllerPolling controllerPolling;
        public MainPage()
        {
            InitializeComponent();

            CoreWindow.GetForCurrentThread().KeyDown += MainPage_KeyDown;
            controllerPolling = new ControllerPolling();
            controllerPolling.Smash +=
                smash => Dispatcher.TryRunAsync(CoreDispatcherPriority.Normal, () => OnSmash(smash));
        }

        private async Task OnSmash(IBabySmash smash)
        {
            var x = _random.Next(0, (int)ActualWidth);
            var y = _random.Next(0, (int)ActualHeight);
            var cool = new CoolStar();
            FiguresCanvas.Children.Add(cool);
            Canvas.SetLeft(cool, x);
            Canvas.SetTop(cool, y);

            try
            {
                await Audio.PlayWavResource(GetRandomSoundFile());
            }
            catch (Exception ex)
            {
                Debugger.Break();
            }
        }


        private static readonly string[] sounds = {
            "giggle.wav",
            "babylaugh.wav",
            "babygigl2.wav",
            "ccgiggle.wav",
            "laughingmice.wav",
            "scooby2.wav",
        };

        private static readonly Random _random = new Random();

        public static string GetRandomSoundFile()
        {
            return sounds[_random.Next(0, sounds.Length)];
        }

        private void MainPage_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            if (args.VirtualKey >= VirtualKey.GamepadA && args.VirtualKey <= VirtualKey.GamepadRightThumbstickLeft)
            {
                // Handled separately
                return;
            }
        
            Debug.WriteLine($"KBD {args.VirtualKey}");
            OnSmash(new KeyboarSmash(args.VirtualKey));
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

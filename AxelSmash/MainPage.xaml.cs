using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Graphics.Display.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AxelSmash.Shapes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AxelSmash
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            CoreWindow.GetForCurrentThread().KeyUp += MainPage_KeyDown;
        }

        private void MainPage_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            var x = new Random().Next(0, (int)ActualWidth);
            var y = new Random().Next(0, (int)ActualHeight);
            var cool = new CoolStar();
            FiguresCanvas.Children.Add(cool);
            Canvas.SetLeft(cool, x);
            Canvas.SetTop(cool, y);
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
                        frame.Navigate(typeof(SmashPage), null);
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

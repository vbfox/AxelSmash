using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using JetBrains.Annotations;

namespace AxelSmash.Shapes
{
    public sealed partial class CoolStar : INotifyPropertyChanged
    {
        public CoolStar(Brush brush)
        {
            InitializeComponent();

            Body.Fill = brush;

            Loaded += CoolStar_Loaded;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void CoolStar_Loaded(object sender, RoutedEventArgs e)
        {
            EyesStoryboard.Begin();
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
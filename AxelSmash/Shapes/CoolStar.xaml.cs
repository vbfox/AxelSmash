using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI;
using Windows.UI.Xaml;
using JetBrains.Annotations;

namespace AxelSmash.Shapes
{
    public sealed partial class CoolStar : INotifyPropertyChanged
    {
        private Color gradientEnd;
        private Color gradientStart;

        public CoolStar()
        {
            InitializeComponent();

            GradientStart = Color.FromArgb(255, 0, 255, 0);
            GradientEnd = Color.FromArgb(255, 0, 103, 54);

            Loaded += CoolStar_Loaded;
        }

        public Color GradientEnd
        {
            get => gradientEnd;
            set
            {
                if (value.Equals(gradientEnd)) return;
                gradientEnd = value;
                OnPropertyChanged();
            }
        }

        public Color GradientStart
        {
            get => gradientStart;
            set
            {
                if (value.Equals(gradientStart)) return;
                gradientStart = value;
                OnPropertyChanged();
            }
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
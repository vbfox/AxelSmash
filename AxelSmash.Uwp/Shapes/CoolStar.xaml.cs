using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace AxelSmash.Uwp.Shapes
{
    public sealed partial class CoolStar
    {
        public CoolStar(Brush brush)
        {
            InitializeComponent();

            Body.Fill = brush;

            Loaded += CoolStar_Loaded;
        }

        private void CoolStar_Loaded(object sender, RoutedEventArgs e)
        {
            EyesStoryboard.Begin();
        }
    }
}
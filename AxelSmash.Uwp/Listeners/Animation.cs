using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace AxelSmash.Uwp.Listeners
{
    static class Animation
    {
        public static Storyboard CreateDoubleAnimation(DependencyObject target,
            string propertyPath, Duration duration, double from, double to)
        {
            var storyboard = new Storyboard();
            
            var animation = new DoubleAnimation
            {
                From = from,
                To = to,
                Duration = duration,
                AutoReverse = false
            };
            
            storyboard.Children.Add(animation);
            
            Storyboard.SetTarget(animation, target);
            Storyboard.SetTargetProperty(animation, propertyPath);

            return storyboard;
        }
    }
}
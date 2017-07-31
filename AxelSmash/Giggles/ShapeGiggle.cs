using AxelSmash.Colors;

namespace AxelSmash.Giggles
{
    public class ShapeGiggle : IGiggle
    {
        public Shape Shape { get; }
        public Hsl Color { get; }

        public ShapeGiggle(Shape shape, Hsl color)
        {
            Shape = shape;
            Color = color;
        }
    }
}
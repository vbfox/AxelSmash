using AxelSmash.Colors;

namespace AxelSmash.Giggles
{
    public class ShapeGiggle : IGiggle
    {
        public Shape Shape { get; }
        public Rgb Color { get; }

        public ShapeGiggle(Shape shape, Rgb color)
        {
            Shape = shape;
            Color = color;
        }
    }
}
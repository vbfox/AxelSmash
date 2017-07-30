using Spectrum;

namespace AxelSmash.Giggles
{
    class ShapeGiggle : IGiggle
    {
        public Shape Shape { get; }
        public Color.RGB Color { get; }

        public ShapeGiggle(Shape shape, Color.RGB color)
        {
            Shape = shape;
            Color = color;
        }
    }
}
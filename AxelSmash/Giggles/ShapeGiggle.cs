using AxelSmash.Colors;

namespace AxelSmash.Giggles
{
    public class ShapeGiggle : IGiggle
    {
        public Shape Shape { get; }
        public Hsl Color { get; }
        public char? Letter { get; }

        public ShapeGiggle(Shape shape, Hsl color, char? letter)
        {
            Shape = shape;
            Color = color;
            Letter = letter;
        }
    }
}
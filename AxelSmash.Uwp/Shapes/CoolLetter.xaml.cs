using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation.Collections;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Xaml;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AxelSmash.Uwp.Shapes
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CoolLetter : Page
    {
        public CoolLetter(Brush brush, char character)
        {
            this.InitializeComponent();

            Body.Fill = brush;
            Body.Data = MakeCharacterGeometry(character);
        }

        public static Geometry MakeCharacterGeometry(char character)
        {
            var g = CanvasGeometry.CreateText(
                new CanvasTextLayout(new CanvasDevice(),
                character.ToString(),
                new CanvasTextFormat() { FontFamily = "Arial", FontSize = 200, FontWeight = FontWeights.Bold }, 250, 250));
            return Win2DGeometryToUwp.Convert(g);
        }
    }
}

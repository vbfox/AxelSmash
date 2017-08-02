using System.Collections.Immutable;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using AxelSmash.SmashSources;
using AxelSmash.Uwp.Listeners;
using AxelSmash.Uwp.SmashSources;

namespace AxelSmash.Uwp
{
    class UwpController : Controller
    {
        private readonly Canvas canvas;

        public UwpController(Canvas canvas)
        {
            this.canvas = canvas;

            SmashSources = new ISmashSource[]
            {
                new ControllerSmashSource(),
                new CoreWindowGestureSmashSource(CoreWindow.GetForCurrentThread()),
                new CoreWindowKeyboardSmashSource(CoreWindow.GetForCurrentThread()
            }.ToImmutableArray();

            var soundGigglePlayer = new SoundGigglePlayer();
            GigglePlayers = new GigglePlayers(
                soundGigglePlayer,
                new ShapesGigglePlayer(canvas),
                soundGigglePlayer);

        }

        protected override ImmutableArray<ISmashSource> SmashSources { get; }
        protected override GigglePlayers GigglePlayers { get; }
    }
}
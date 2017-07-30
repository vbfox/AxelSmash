using System.Collections.Immutable;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using AxelSmash.Listeners;
using AxelSmash.SmashSources;

namespace AxelSmash
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
                new CoreWindowKeysSmashSource(CoreWindow.GetForCurrentThread())
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
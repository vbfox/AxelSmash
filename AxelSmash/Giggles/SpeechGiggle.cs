namespace AxelSmash.Giggles
{
    class SpeechGiggle : IGiggle
    {
        public string Text { get; }

        public SpeechGiggle(string text)
        {
            Text = text;
        }
    }
}
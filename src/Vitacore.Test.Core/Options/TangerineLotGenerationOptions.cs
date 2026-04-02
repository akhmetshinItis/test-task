namespace Vitacore.Test.Core.Options
{
    public class TangerineLotGenerationOptions
    {
        public const string SectionName = "TangerineLotGeneration";

        public int MaxGenerateCount { get; set; } = 20;
        public int JobGenerateCount { get; set; } = 1;
        public int StartPriceMin { get; set; } = 80;
        public int StartPriceMax { get; set; } = 240;
        public decimal BuyoutMultiplierMin { get; set; } = 2.2m;
        public decimal BuyoutMultiplierMax { get; set; } = 3.0m;
        public int AuctionDurationHoursMin { get; set; } = 6;
        public int AuctionDurationHoursMax { get; set; } = 24;
        public int ExpirationDaysMin { get; set; } = 1;
        public int ExpirationDaysMax { get; set; } = 3;
        public string[] TitlePrefixes { get; set; } =
        [
            "Classic",
            "Golden",
            "Forest",
            "Ruby",
            "Sunny",
            "Honey",
            "Velvet",
            "Sparkling"
        ];

        public string[] DescriptionTemplates { get; set; } =
        [
            "Fresh tangerine illustration with bright citrus tones.",
            "Juicy tangerine artwork prepared for the next auction.",
            "Glossy mandarin image with rich orange palette.",
            "Neat tangerine picture with soft highlights and vivid peel."
        ];

        public string SvgBackgroundStartColor { get; set; } = "#fff7e6";
        public string SvgBackgroundEndColor { get; set; } = "#ffd08a";
        public string SvgMandarinColor { get; set; } = "#f58a1f";
        public string SvgHighlightColor { get; set; } = "#ffb458";
        public string SvgLeafColor { get; set; } = "#5f8f2f";
        public string SvgTitleColor { get; set; } = "#7a4a14";
        public string SvgFontFamily { get; set; } = "Verdana";
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlossyBook
{
    public struct UITheme
    {
        public string MainColor {  get; set; }
        public string BackgroundColor { get; set; }
        public string PrimaryTextColor { get; set; }
        public string SecondaryTextColor { get; set; }
        public string ButtonTextColor { get; set; }

        public UITheme(string mainColor, string backgroundColor, string primaryTextColor, string secondaryTextColor, string buttonTextColor)
        {
            MainColor = mainColor;
            BackgroundColor = backgroundColor;
            PrimaryTextColor = primaryTextColor;
            SecondaryTextColor = secondaryTextColor;
            ButtonTextColor = buttonTextColor;
        }

        public static UITheme Default()
        {
            return new UITheme(
                "#404452",
                "#565b6e",
                "#ebce86",
                "#86b5eb",
                "#dbdbdb");
        }
    }
}

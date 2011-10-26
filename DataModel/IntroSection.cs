namespace Quantae.DataModel
{
    using System.Collections.Generic;

    public class IntroSection : StaticSection
    {
        public IntroSection()
        {
            this.Pages = new List<StaticPage>();
        }
    }
}
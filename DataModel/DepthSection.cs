namespace Quantae.DataModel
{
    using System.Collections.Generic;

    public class DepthSection : StaticSection
    {
        public DepthSection()
        {
            this.Pages = new List<StaticPage>();
        }
    }
}
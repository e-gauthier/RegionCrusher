namespace RegionCrusher.Tags
{

    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Text.Tagging;
    public class RegionTag : ClassificationTag
    {
        public RegionTag(IClassificationType type)
            : base(type)
        {
        }
    }
}

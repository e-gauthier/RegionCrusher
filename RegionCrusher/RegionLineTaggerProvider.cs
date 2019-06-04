//-----------------------------------------------------------------------
// <copyright file="RegionLineTaggerProvider.cs" company="Xpertdoc Technologies Inc.">
//     Missing Copyright information from a valid stylecop.json file.
// </copyright>
//-----------------------------------------------------------------------

namespace RegionCrusher
{
    using System.ComponentModel.Composition;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Text.Tagging;
    using Microsoft.VisualStudio.Utilities;
    using RegionCrusher.DialogPages;
    using RegionCrusher.Tags;

    [Export(typeof(IViewTaggerProvider))]
    [ContentType("CSharp")]
    [ContentType("Basic")]
    [TagType(typeof(RegionTag))]
    public class RegionLineTaggerProvider : IViewTaggerProvider
    {
        [Import]
        internal IClassificationTypeRegistryService ClassificationTypeRegistryService { get; set; }

        public ITagger<T> CreateTagger<T>(ITextView textView, ITextBuffer buffer) where T : ITag
        {
            if (textView.TextBuffer != buffer)
                return null;
            if (!RegionCrusherOptionPage.Instance.StyleRegionLinesOption)
                return null;
            return new RegionLineTagger(textView, buffer, this.ClassificationTypeRegistryService) as ITagger<T>;
        }
    }
}

//-----------------------------------------------------------------------
// <copyright file="TextViewCreationListener.cs" company="Xpertdoc Technologies Inc.">
//     Missing Copyright information from a valid stylecop.json file.
// </copyright>
//-----------------------------------------------------------------------

namespace RegionCrusher
{
    using System.ComponentModel.Composition;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Text.Outlining;
    using Microsoft.VisualStudio.Utilities;

    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType("CSharp")]
    [ContentType("Basic")]
    [TextViewRole("DOCUMENT")]
    public class TextViewCreationListener : IWpfTextViewCreationListener
    {
        [Import(typeof(IOutliningManagerService))]
        public IOutliningManagerService OutliningManagerService { get; set; }


        public void TextViewCreated(IWpfTextView textView)
        {
            if (textView == null || OutliningManagerService == null)
            {
                return;
            }
            TextViewHandler textViewHandler = new TextViewHandler(textView, this.OutliningManagerService);

        }
    }
}

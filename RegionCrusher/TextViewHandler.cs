//-----------------------------------------------------------------------
// <copyright file="TextViewHandler.cs" company="Xpertdoc Technologies Inc.">
//     Missing Copyright information from a valid stylecop.json file.
// </copyright>
//-----------------------------------------------------------------------

namespace RegionCrusher
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Text.Outlining;
    using RegionCrusher.DialogPages;

    public class TextViewHandler
    {
        private IWpfTextView _textView;
        private IOutliningManager _outliningManager;
        private bool _disableCollapse { get; }
        private bool _expandOnLoad { get; }

        public TextViewHandler(IWpfTextView textView, IOutliningManagerService outliningManagerService)
        {
            if (RegionCrusherOptionPage.Instance == null)
                return;
            this._textView = textView;
            this._disableCollapse = RegionCrusherOptionPage.Instance.DisableRegionCollapsingOption;
            this._expandOnLoad = RegionCrusherOptionPage.Instance.ExpandRegionOnLoad;
            this._outliningManager = outliningManagerService.GetOutliningManager((ITextView)textView);
            if (this._outliningManager == null || !this._disableCollapse && !this._expandOnLoad)
                return;
            this._textView.Closed += this.TextViewHandler_Closed;
            this._outliningManager.RegionsCollapsed += this.ClassifierProvider_RegionsCollapsed;
        }

        private void TextViewHandler_Closed(object sender, EventArgs e)
        {
            if (this._textView != null)
                this._textView.Closed -= this.TextViewHandler_Closed;
            if (this._outliningManager != null)
                this._outliningManager.RegionsCollapsed -= this.ClassifierProvider_RegionsCollapsed;
            this._outliningManager = null;
            this._textView = null;
        }

        private void ClassifierProvider_RegionsCollapsed(object sender, RegionsCollapsedEventArgs e)
        {
            using (IEnumerator<ICollapsed> enumerator = e.CollapsedRegions.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    ICollapsed current = enumerator.Current;
                    if (current.Extent.TextBuffer == this._textView.TextBuffer)
                    {
                        string text = current.Extent.GetText(current.Extent.TextBuffer.CurrentSnapshot);
                        if (current.IsCollapsed)
                        {
                            if (text.TrimStart().ToLower().StartsWith("#region"))
                            {
                                try
                                {
                                    ((IOutliningManager)sender).Expand(current);
                                }
                                catch (InvalidOperationException ex)
                                {
                                }
                            }
                        }
                    }
                }
            }
            if (!this._expandOnLoad || this._disableCollapse)
                return;
            ((IOutliningManager)sender).RegionsCollapsed -= this.ClassifierProvider_RegionsCollapsed;
        }
    }
}

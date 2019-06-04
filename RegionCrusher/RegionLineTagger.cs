//-----------------------------------------------------------------------
// <copyright file="RegionLineTagger.cs" company="Xpertdoc Technologies Inc.">
//     Missing Copyright information from a valid stylecop.json file.
// </copyright>
//-----------------------------------------------------------------------

namespace RegionCrusher
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Text.Tagging;
    using RegionCrusher.Tags;

    public class RegionLineTagger : ITagger<RegionTag>
    {
        private readonly object _updateLock = new object();
        private int _oldLineNumber = -1;
        private SnapshotSpan _oldSnapshotSpan;

        public RegionLineTagger(
          ITextView view,
          ITextBuffer sourceBuffer,
          IClassificationTypeRegistryService classificationTypeRegistryService)
        {
            this.View = view;
            this.SourceBuffer = sourceBuffer;
            this.ClassificationTypeRegistryService = classificationTypeRegistryService;
            if (!DialogPages.RegionCrusherOptionPage.Instance.StyleRegionLinesOption)
                return;
            view.Caret.PositionChanged += this.CaretPositionChanged;
            view.LayoutChanged += this.ViewLayoutChanged;
        }

        private ITextView View { get; set; }

        private ITextBuffer SourceBuffer { get; set; }

        private IClassificationTypeRegistryService ClassificationTypeRegistryService { get; set; }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

        //public IEnumerable<ITagSpan<RegionTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        //{
        //    //return (IEnumerable<ITagSpan<RegionTag>>)new RegionLineTagger.<GetTags>d__19(-2)
        //    // {
        //    //   <>4__this = this,
        //    //   <>3__spans = spans
        //    // };

        //    return new List<ITagSpan<RegionTag>>()
        //    {
        //        this,
        //        spans
        //    };
        //}

        public IEnumerable<ITagSpan<RegionTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            if (!DialogPages.RegionCrusherOptionPage.Instance.StyleRegionLinesOption || spans.Count == 0)
            {
                return Enumerable.Empty<ITagSpan<RegionTag>>();
            }

            List<ITagSpan<RegionTag>> regionTags = new List<ITagSpan<RegionTag>>();

            foreach (var span in spans)
            {
                if (!RegionLineTagger.IsRegionOrEndRegion(span))
                {
                    continue;
                }

                int numberFromPosition = span.Snapshot.GetLineNumberFromPosition(span.Start);
                CaretPosition position1 = this.View.Caret.Position;
                IMappingPoint point1 = position1.Point;
                ITextBuffer sourceBuffer = this.SourceBuffer;
                CaretPosition position2 = this.View.Caret.Position;
                PositionAffinity affinity = position2.Affinity;
                SnapshotPoint? point2 = point1.GetPoint(sourceBuffer, affinity);

                int num1 = point2.HasValue ? span.Snapshot.GetLineNumberFromPosition(point2.Value) : -1;
                int num2 = span.GetText().IndexOf('#');

                SnapshotSpan regionLineSnapshot = new SnapshotSpan(span.Snapshot, span.Start + num2, span.Length - num2);
                int num3 = num1;
                if (numberFromPosition != num3)
                {
                    regionTags.Add(new TagSpan<RegionTag>(regionLineSnapshot, new RegionTag(this.ClassificationTypeRegistryService.GetClassificationType("inactiveRegionLine"))));
                }
                else
                {
                    regionTags.Add(new TagSpan<RegionTag>(regionLineSnapshot, new RegionTag(this.ClassificationTypeRegistryService.GetClassificationType("activeRegionLine"))));
                }
            }

            return regionTags;
        }

        private void ViewLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            if (e.NewViewState.EditSnapshot == e.OldViewState.EditSnapshot)
                return;
            this.UpdateAtCaretPosition(this.View.Caret.Position);
        }

        private void CaretPositionChanged(object sender, CaretPositionChangedEventArgs e)
        {
            this.UpdateAtCaretPosition(e.NewPosition);
        }

        private void UpdateAtCaretPosition(CaretPosition newPosition)
        {
            SnapshotPoint? point = newPosition.Point.GetPoint(this.SourceBuffer, newPosition.Affinity);
            if (!point.HasValue)
                return;
            SnapshotPoint snapshotPoint1 = point.Value;
            ITextSnapshot snapshot1 = snapshotPoint1.Snapshot;
            SnapshotPoint snapshotPoint2 = point.Value;
            int position1 = snapshotPoint2.Position;
            ITextSnapshotLine lineFromPosition = snapshot1.GetLineFromPosition(position1);
            lock (this._updateLock)
            {
                if (lineFromPosition.LineNumber == this._oldLineNumber)
                    return;

                ITextSnapshot snapshot2 = lineFromPosition.Snapshot;
                SnapshotPoint start = lineFromPosition.Start;
                int position2 = start.Position;
                int length = lineFromPosition.Length;

                SnapshotSpan local = new SnapshotSpan(snapshot2, position2, length);

                bool flag = RegionLineTagger.IsRegionOrEndRegion(local);
                EventHandler<SnapshotSpanEventArgs> tagsChanged = this.TagsChanged;
                if (tagsChanged != null)
                {
                    if (flag)
                    {
                        tagsChanged(this, new SnapshotSpanEventArgs(local));
                    }

                    if (!this._oldSnapshotSpan.IsEmpty)
                    {
                        tagsChanged((object)this, new SnapshotSpanEventArgs(this._oldSnapshotSpan));
                    }
                }
                this._oldSnapshotSpan = local;
                this._oldLineNumber = lineFromPosition.LineNumber;
            }
        }

        private static bool IsRegionOrEndRegion(SnapshotSpan line)
        {
            return Regex.IsMatch(line.GetText().Trim().ToLower(), "^#\\W*(end\\W*)*region");
        }
    }
}

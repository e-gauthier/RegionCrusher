//-----------------------------------------------------------------------
// <copyright file="ActiveRegionLineClassification.cs" company="Xpertdoc Technologies Inc.">
//     Missing Copyright information from a valid stylecop.json file.
// </copyright>
//-----------------------------------------------------------------------

namespace RegionCrusher.Tags
{
    using System.ComponentModel.Composition;
    using System.Windows.Media;
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Utilities;

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "activeRegionLine")]
    [Name("Active Region Line")]
    [DisplayName("Active Region Line")]
    [UserVisible(true)]
    [Order(After = "Default Priority", Before = "High Priority")]
    internal sealed class ActiveRegionLineClassification : ClassificationFormatDefinition
    {
        public ActiveRegionLineClassification()
            : base()
        {
            this.ForegroundColor = new Color?(Colors.DarkGray);
        }
    }
}

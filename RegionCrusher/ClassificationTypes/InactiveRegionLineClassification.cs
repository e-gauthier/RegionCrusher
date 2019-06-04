//-----------------------------------------------------------------------
// <copyright file="InactiveRegionLineClassification.cs" company="Xpertdoc Technologies Inc.">
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
    [ClassificationType(ClassificationTypeNames = "inactiveRegionLine")]
    [Name("Inactive Region Line")]
    [DisplayName("Inactive Region Line")]
    [UserVisible(true)]
    [Order(After = "Default Priority", Before = "High Priority")]
    internal sealed class InactiveRegionLineClassification : ClassificationFormatDefinition
    {
        public InactiveRegionLineClassification()
            : base()
        {
            this.ForegroundColor = new Color?(Colors.Gray);
            this.FontRenderingSize = new double?(10.0);
            this.ForegroundOpacity = new double?(0.5);
        }
    }
}

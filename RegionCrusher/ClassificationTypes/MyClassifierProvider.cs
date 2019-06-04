//-----------------------------------------------------------------------
// <copyright file="MyClassifierProvider.cs" company="Xpertdoc Technologies Inc.">
//     Missing Copyright information from a valid stylecop.json file.
// </copyright>
//-----------------------------------------------------------------------

using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace RegionCrusher.Tags
{
    internal sealed class MyClassifierProvider
    {
        [Export]
        [Name("activeRegionLine")]
        internal static ClassificationTypeDefinition ActiveRegionLineDefinition;
        [Export]
        [Name("inactiveRegionLine")]
        internal static ClassificationTypeDefinition InactiveRegionLineDefinition;

        public MyClassifierProvider()
        {
        }
    }
}

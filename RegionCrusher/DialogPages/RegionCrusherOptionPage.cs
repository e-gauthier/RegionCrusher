//-----------------------------------------------------------------------
// <copyright file="RegionCrusherOptionPage.cs" company="Xpertdoc Technologies Inc.">
//     Missing Copyright information from a valid stylecop.json file.
// </copyright>
//-----------------------------------------------------------------------

namespace RegionCrusher.DialogPages
{
    using System.ComponentModel;
    using Microsoft.VisualStudio.Shell;

    public class RegionCrusherOptionPage : DialogPage
    {
        public RegionCrusherOptionPage()
        {
            this.ExpandRegionOnLoad = true;
        }

        [Category("Options")]
        public bool ExpandRegionOnLoad { get; set; }

        [Category("Options")]
        public bool StyleRegionLinesOption { get; set; }

        [Category("Options")]
        public bool DisableRegionCollapsingOption { get; set; }

        public static RegionCrusherOptionPage Instance { get; set; }
    }
}

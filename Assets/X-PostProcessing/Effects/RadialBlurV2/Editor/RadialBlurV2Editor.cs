
//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// created by QianMo @ 2020
//----------------------------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using UnityEditor.Rendering.PostProcessing;
using UnityEngine.Rendering.PostProcessing;

namespace XPostProcessing
{
    [PostProcessEditor(typeof(RadialBlurV2))]
    public sealed class RadialBlurV2Editor : PostProcessEffectEditor<RadialBlurV2>
    {
        SerializedParameterOverride qualityLevel;
        SerializedParameterOverride blurRadius;
        SerializedParameterOverride radialCenterX;
        SerializedParameterOverride radialCenterY;



        public override void OnEnable()
        {
            qualityLevel = FindParameterOverride(x => x.qualityLevel);
            blurRadius = FindParameterOverride(x => x.blurRadius);
            radialCenterX = FindParameterOverride(x => x.radialCenterX);
            radialCenterY = FindParameterOverride(x => x.radialCenterY);
        }

        public override string GetDisplayTitle()
        {
            return XPostProcessingEditorUtility.DISPLAY_TITLE_PREFIX + base.GetDisplayTitle();
        }

        public override void OnInspectorGUI()
        {

            PropertyField(qualityLevel);
            PropertyField(blurRadius);

            EditorUtilities.DrawHeaderLabel("Radial Center");
            PropertyField(radialCenterX);
            PropertyField(radialCenterY);
        }

    }
}
        


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
        SerializedParameterOverride QualityLevel;
        SerializedParameterOverride BlurRadius;
        SerializedParameterOverride RadialCenterX;
        SerializedParameterOverride RadialCenterY;



        public override void OnEnable()
        {
            QualityLevel = FindParameterOverride(x => x.QualityLevel);
            BlurRadius = FindParameterOverride(x => x.BlurRadius);
            RadialCenterX = FindParameterOverride(x => x.RadialCenterX);
            RadialCenterY = FindParameterOverride(x => x.RadialCenterY);
        }

        public override string GetDisplayTitle()
        {
            return XPostProcessingEditorUtility.DISPLAY_TITLE_PREFIX + base.GetDisplayTitle();
        }

        public override void OnInspectorGUI()
        {

            PropertyField(QualityLevel);
            PropertyField(BlurRadius);

            EditorUtilities.DrawHeaderLabel("Radial Center");
            PropertyField(RadialCenterX);
            PropertyField(RadialCenterY);
        }

    }
}
        

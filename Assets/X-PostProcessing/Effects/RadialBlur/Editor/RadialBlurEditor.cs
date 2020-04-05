
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
    [PostProcessEditor(typeof(RadialBlur))]
    public sealed class RadialBlurEditor : PostProcessEffectEditor<RadialBlur>
    {

        SerializedParameterOverride blurRadius;
        SerializedParameterOverride iteration;
        SerializedParameterOverride radialCenterX;
        SerializedParameterOverride radialCenterY;
        

        public override void OnEnable()
        {
            blurRadius = FindParameterOverride(x => x.blurRadius);
            iteration = FindParameterOverride(x => x.iteration);
            radialCenterX = FindParameterOverride(x => x.radialCenterX);
            radialCenterY = FindParameterOverride(x => x.radialCenterY);
        }

        public override string GetDisplayTitle()
        {
            return XPostProcessingEditorUtility.DISPLAY_TITLE_PREFIX + base.GetDisplayTitle();
        }

        public override void OnInspectorGUI()
        {
            EditorUtilities.DrawHeaderLabel("Core Property");
            PropertyField(blurRadius);
            PropertyField(iteration);

            EditorUtilities.DrawHeaderLabel("Radial Center");
            PropertyField(radialCenterX);
            PropertyField(radialCenterY);
        }

    }
}
        

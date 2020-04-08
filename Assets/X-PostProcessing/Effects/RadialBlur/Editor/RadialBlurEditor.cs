
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

        SerializedParameterOverride BlurRadius;
        SerializedParameterOverride Iteration;
        SerializedParameterOverride RadialCenterX;
        SerializedParameterOverride RadialCenterY;
        

        public override void OnEnable()
        {
            BlurRadius = FindParameterOverride(x => x.BlurRadius);
            Iteration = FindParameterOverride(x => x.Iteration);
            RadialCenterX = FindParameterOverride(x => x.RadialCenterX);
            RadialCenterY = FindParameterOverride(x => x.RadialCenterY);
        }

        public override string GetDisplayTitle()
        {
            return XPostProcessingEditorUtility.DISPLAY_TITLE_PREFIX + base.GetDisplayTitle();
        }

        public override void OnInspectorGUI()
        {
            EditorUtilities.DrawHeaderLabel("Core Property");
            PropertyField(BlurRadius);
            PropertyField(Iteration);

            EditorUtilities.DrawHeaderLabel("Radial Center");
            PropertyField(RadialCenterX);
            PropertyField(RadialCenterY);
        }

    }
}
        

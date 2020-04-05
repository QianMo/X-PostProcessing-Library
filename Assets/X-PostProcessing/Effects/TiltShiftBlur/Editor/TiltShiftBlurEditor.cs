
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
    [PostProcessEditor(typeof(TiltShiftBlur))]
    public sealed class TiltShiftBlurEditor : PostProcessEffectEditor<TiltShiftBlur>
    {

        SerializedParameterOverride qualityLevel;
        SerializedParameterOverride areaSize;
        SerializedParameterOverride blurRadius;
        SerializedParameterOverride Iteration;
        SerializedParameterOverride RTDownScaling;

        public override void OnEnable()
        {
            qualityLevel = FindParameterOverride(x => x.qualityLevel);
            areaSize = FindParameterOverride(x => x.areaSize);
            blurRadius = FindParameterOverride(x => x.blurRadius);
            Iteration = FindParameterOverride(x => x.Iteration);
            RTDownScaling = FindParameterOverride(x => x.RTDownScaling);
        }

        public override string GetDisplayTitle()
        {
            return XPostProcessingEditorUtility.DISPLAY_TITLE_PREFIX + base.GetDisplayTitle();
        }

        public override void OnInspectorGUI()
        {
            PropertyField(qualityLevel);
            PropertyField(areaSize);
            PropertyField(blurRadius);
            PropertyField(Iteration);
            PropertyField(RTDownScaling);
        }

    }
}
        


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
    [PostProcessEditor(typeof(BokehBlur))]
    public sealed class BokehBlurEditor : PostProcessEffectEditor<BokehBlur>
    {

        SerializedParameterOverride blurRadius;
        SerializedParameterOverride iteration;
        SerializedParameterOverride RTDownScaling;


        public override void OnEnable()
        {
            blurRadius = FindParameterOverride(x => x.blurRadius);
            iteration = FindParameterOverride(x => x.iteration);
            RTDownScaling = FindParameterOverride(x => x.RTDownScaling);
        }

        public override string GetDisplayTitle()
        {
            return XPostProcessingEditorUtility.DISPLAY_TITLE_PREFIX + base.GetDisplayTitle();
        }

        public override void OnInspectorGUI()
        {
            PropertyField(blurRadius);
            PropertyField(iteration);
            PropertyField(RTDownScaling);
        }

    }
}
        

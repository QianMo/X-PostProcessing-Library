
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
    [PostProcessEditor(typeof(TiltShiftBlurV2))]
    public sealed class TiltShiftBlurV2Editor : PostProcessEffectEditor<TiltShiftBlurV2>
    {

        SerializedParameterOverride Iteration;
        SerializedParameterOverride BlurRadius;
        SerializedParameterOverride centerOffset;
        SerializedParameterOverride AreaSize;
        SerializedParameterOverride areaSmooth;
        SerializedParameterOverride showPreview;


        public override void OnEnable()
        {
            showPreview = FindParameterOverride(x => x.showPreview);
            centerOffset = FindParameterOverride(x => x.centerOffset);
            AreaSize = FindParameterOverride(x => x.AreaSize);
            areaSmooth = FindParameterOverride(x => x.areaSmooth);
            Iteration = FindParameterOverride(x => x.Iteration);
             BlurRadius = FindParameterOverride(x => x.BlurRadius);
        }

        public override string GetDisplayTitle()
        {
            return XPostProcessingEditorUtility.DISPLAY_TITLE_PREFIX + base.GetDisplayTitle();
        }

        public override void OnInspectorGUI()
        {

            EditorUtilities.DrawHeaderLabel("Blur Property");
            PropertyField(BlurRadius);
            PropertyField(Iteration);

            EditorUtilities.DrawHeaderLabel("Area Property");
            PropertyField(centerOffset);
            PropertyField(AreaSize);
            PropertyField(areaSmooth);

            EditorUtilities.DrawHeaderLabel("Debug");
            PropertyField(showPreview);

        }

    }
}
        

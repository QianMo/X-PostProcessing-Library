
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

        SerializedParameterOverride iteration;
        SerializedParameterOverride blurRadius;
        SerializedParameterOverride centerOffset;
        SerializedParameterOverride areaSize;
        SerializedParameterOverride areaSmooth;
        SerializedParameterOverride showPreview;


        public override void OnEnable()
        {
            showPreview = FindParameterOverride(x => x.showPreview);
            centerOffset = FindParameterOverride(x => x.centerOffset);
            areaSize = FindParameterOverride(x => x.areaSize);
            areaSmooth = FindParameterOverride(x => x.areaSmooth);
            iteration = FindParameterOverride(x => x.iteration);
             blurRadius = FindParameterOverride(x => x.blurRadius);
        }

        public override string GetDisplayTitle()
        {
            return XPostProcessingEditorUtility.DISPLAY_TITLE_PREFIX + base.GetDisplayTitle();
        }

        public override void OnInspectorGUI()
        {

            EditorUtilities.DrawHeaderLabel("Blur Property");
            PropertyField(blurRadius);
            PropertyField(iteration);

            EditorUtilities.DrawHeaderLabel("Area Property");
            PropertyField(centerOffset);
            PropertyField(areaSize);
            PropertyField(areaSmooth);

            EditorUtilities.DrawHeaderLabel("Debug");
            PropertyField(showPreview);

        }

    }
}
        

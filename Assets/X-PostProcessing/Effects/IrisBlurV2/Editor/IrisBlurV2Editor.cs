
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
    [PostProcessEditor(typeof(IrisBlurV2))]
    public sealed class IrisBlurV2Editor : PostProcessEffectEditor<IrisBlurV2>
    {

        SerializedParameterOverride centerOffsetX;
        SerializedParameterOverride centerOffsetY;
        SerializedParameterOverride areaSize;
        SerializedParameterOverride iteration;
        SerializedParameterOverride blurRadius;
        SerializedParameterOverride showPreview;


        public override void OnEnable()
        {
            showPreview = FindParameterOverride(x => x.showPreview);
            centerOffsetX = FindParameterOverride(x => x.centerOffsetX);
            centerOffsetY = FindParameterOverride(x => x.centerOffsetY);
            areaSize = FindParameterOverride(x => x.areaSize);
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
            PropertyField(areaSize);
            PropertyField(centerOffsetX);
            PropertyField(centerOffsetY);

            EditorUtilities.DrawHeaderLabel("Debug");
            PropertyField(showPreview);
        }

    }
}
        

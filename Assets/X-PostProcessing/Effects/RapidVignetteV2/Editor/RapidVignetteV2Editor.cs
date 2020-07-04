using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using UnityEditor.Rendering.PostProcessing;
using UnityEngine.Rendering.PostProcessing;

namespace XPostProcessing
{
    [PostProcessEditor(typeof(RapidVignetteV2))]
    public sealed class RapidVignetteV2Editor : PostProcessEffectEditor<RapidVignetteV2>
    {

        SerializedParameterOverride vignetteType;
        SerializedParameterOverride vignetteIndensity;
        SerializedParameterOverride vignetteSharpness;
        SerializedParameterOverride vignetteCenter;
        SerializedParameterOverride vignetteColor;
        
        public override void OnEnable()
        {
            vignetteType = FindParameterOverride(x => x.vignetteType);
            vignetteIndensity = FindParameterOverride(x => x.vignetteIndensity);
            vignetteSharpness = FindParameterOverride(x => x.vignetteSharpness);
            vignetteCenter = FindParameterOverride(x => x.vignetteCenter);
            vignetteColor = FindParameterOverride(x => x.vignetteColor);
        }

        public override string GetDisplayTitle()
        {
            return XPostProcessingEditorUtility.DISPLAY_TITLE_PREFIX + base.GetDisplayTitle();
        }

        public override void OnInspectorGUI()
        {

            PropertyField(vignetteType);
            PropertyField(vignetteIndensity);
            PropertyField(vignetteSharpness);
            PropertyField(vignetteCenter);

            if (vignetteType.value.enumValueIndex == 1)
            {
                PropertyField(vignetteColor);
            }


        }

    }
}
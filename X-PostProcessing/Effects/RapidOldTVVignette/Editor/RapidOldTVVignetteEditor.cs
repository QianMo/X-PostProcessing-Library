using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using UnityEditor.Rendering.PostProcessing;
using UnityEngine.Rendering.PostProcessing;

namespace XPostProcessing
{
    [PostProcessEditor(typeof(RapidOldTVVignette))]
    public sealed class RapidOldTVVignetteEditor : PostProcessEffectEditor<RapidOldTVVignette>
    {

        SerializedParameterOverride vignetteType;
        SerializedParameterOverride vignetteIndensity;
        SerializedParameterOverride vignetteCenter;
        SerializedParameterOverride vignetteColor;

        public override void OnEnable()
        {
            vignetteType = FindParameterOverride(x => x.vignetteType);
            vignetteIndensity = FindParameterOverride(x => x.vignetteIndensity);
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
            PropertyField(vignetteCenter);

            if (vignetteType.value.enumValueIndex == 1)
            {
                PropertyField(vignetteColor);
            }
        }

    }
}
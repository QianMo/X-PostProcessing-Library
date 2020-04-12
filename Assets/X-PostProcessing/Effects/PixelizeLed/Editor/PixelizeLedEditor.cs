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
    [PostProcessEditor(typeof(PixelizeLed))]
    public sealed class PixelizeLedEditor : PostProcessEffectEditor<PixelizeLed>
    {

        SerializedParameterOverride pixelSize;
        SerializedParameterOverride useAutoScreenRatio;
        SerializedParameterOverride pixelRatio;
        SerializedParameterOverride BackgroundColor;
        SerializedParameterOverride ledRadius;

        public override void OnEnable()
        {
            pixelSize = FindParameterOverride(x => x.pixelSize);
            ledRadius = FindParameterOverride(x => x.ledRadius);
            useAutoScreenRatio = FindParameterOverride(x => x.useAutoScreenRatio);
            pixelRatio = FindParameterOverride(x => x.pixelRatio);
            BackgroundColor = FindParameterOverride(x => x.BackgroundColor);

        }

        public override string GetDisplayTitle()
        {
            return XPostProcessingEditorUtility.DISPLAY_TITLE_PREFIX + base.GetDisplayTitle();
        }

        public override void OnInspectorGUI()
        {
            EditorUtilities.DrawHeaderLabel("Core Property");
            PropertyField(pixelSize);
            PropertyField(ledRadius);
            PropertyField(useAutoScreenRatio);

            if (useAutoScreenRatio.value.boolValue == false)
            {
                PropertyField(pixelRatio);
            }


            EditorUtilities.DrawHeaderLabel("Pixel Scale");
            PropertyField(BackgroundColor);
        }


    }
}
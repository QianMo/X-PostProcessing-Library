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
    [PostProcessEditor(typeof(PixelizeQuad))]
    public sealed class PixelizeQuadEditor : PostProcessEffectEditor<PixelizeQuad>
    {

        SerializedParameterOverride pixelSize;
        SerializedParameterOverride useAutoScreenRatio;
        SerializedParameterOverride pixelRatio;
        SerializedParameterOverride pixelScaleX;
        SerializedParameterOverride pixelScaleY;

        public override void OnEnable()
        {
            pixelSize = FindParameterOverride(x => x.pixelSize);
            useAutoScreenRatio = FindParameterOverride(x => x.useAutoScreenRatio);
            pixelRatio = FindParameterOverride(x => x.pixelRatio);
            pixelScaleX = FindParameterOverride(x => x.pixelScaleX);
            pixelScaleY = FindParameterOverride(x => x.pixelScaleY);
        }

        public override string GetDisplayTitle()
        {
            return XPostProcessingEditorUtility.DISPLAY_TITLE_PREFIX + base.GetDisplayTitle();
        }

        public override void OnInspectorGUI()
        {
            EditorUtilities.DrawHeaderLabel("Core Property");
            PropertyField(pixelSize);
            PropertyField(useAutoScreenRatio);

            if (useAutoScreenRatio.value.boolValue == false)
            {
                PropertyField(pixelRatio);
            }


            EditorUtilities.DrawHeaderLabel("Pixel Scale");
            PropertyField(pixelScaleX);
            PropertyField(pixelScaleY);
        }


    }
}
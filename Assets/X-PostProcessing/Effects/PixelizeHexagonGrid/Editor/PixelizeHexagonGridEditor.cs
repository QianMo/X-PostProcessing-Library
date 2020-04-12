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
    [PostProcessEditor(typeof(PixelizeHexagonGrid))]
    public sealed class PixelizeHexagonGridEditor : PostProcessEffectEditor<PixelizeHexagonGrid>
    {

        SerializedParameterOverride pixelSize;
        SerializedParameterOverride useAutoScreenRatio;
        SerializedParameterOverride pixelRatio;
        SerializedParameterOverride pixelScaleX;
        SerializedParameterOverride pixelScaleY;
        SerializedParameterOverride gridWidth;

        

        public override void OnEnable()
        {
            pixelSize = FindParameterOverride(x => x.pixelSize);
            useAutoScreenRatio = FindParameterOverride(x => x.useAutoScreenRatio);
            pixelRatio = FindParameterOverride(x => x.pixelRatio);
            pixelScaleX = FindParameterOverride(x => x.pixelScaleX);
            pixelScaleY = FindParameterOverride(x => x.pixelScaleY);
            gridWidth = FindParameterOverride(x => x.gridWidth);
        }

        public override string GetDisplayTitle()
        {
            return XPostProcessingEditorUtility.DISPLAY_TITLE_PREFIX + base.GetDisplayTitle();
        }

        public override void OnInspectorGUI()
        {
            EditorUtilities.DrawHeaderLabel("Core Property");
            PropertyField(pixelSize);
            PropertyField(gridWidth);
        }


    }
}
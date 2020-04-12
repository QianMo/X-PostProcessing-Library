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
    [PostProcessEditor(typeof(PixelizeDiamond))]
    public sealed class PixelizeDiamondEditor : PostProcessEffectEditor<PixelizeDiamond>
    {

        SerializedParameterOverride pixelSize;


        public override void OnEnable()
        {
            pixelSize = FindParameterOverride(x => x.pixelSize);
        }

        public override string GetDisplayTitle()
        {
            return XPostProcessingEditorUtility.DISPLAY_TITLE_PREFIX + base.GetDisplayTitle();
        }

        public override void OnInspectorGUI()
        {
            PropertyField(pixelSize);
        }

    }
}
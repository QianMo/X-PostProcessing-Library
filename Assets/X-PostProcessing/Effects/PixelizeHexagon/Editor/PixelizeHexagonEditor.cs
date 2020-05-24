//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License 
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using UnityEditor.Rendering.PostProcessing;
using UnityEngine.Rendering.PostProcessing;

namespace XPostProcessing
{
    [PostProcessEditor(typeof(PixelizeHexagon))]
    public sealed class PixelizeHexagonEditor : PostProcessEffectEditor<PixelizeHexagon>
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
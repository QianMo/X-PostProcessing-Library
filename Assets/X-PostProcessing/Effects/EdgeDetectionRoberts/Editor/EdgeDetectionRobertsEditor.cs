
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
    [PostProcessEditor(typeof(EdgeDetectionRoberts))]
    public sealed class EdgeDetectionRobertsEditor : PostProcessEffectEditor<EdgeDetectionRoberts>
    {

        SerializedParameterOverride edgeWidth;
        SerializedParameterOverride backgroundFade;
        SerializedParameterOverride edgeColor;
        SerializedParameterOverride backgroundColor;


        public override void OnEnable()
        {
            edgeWidth = FindParameterOverride(x => x.edgeWidth);
            backgroundFade = FindParameterOverride(x => x.backgroundFade);
            edgeColor = FindParameterOverride(x => x.edgeColor);
            backgroundColor = FindParameterOverride(x => x.backgroundColor);
        }

        public override string GetDisplayTitle()
        {
            return XPostProcessingEditorUtility.DISPLAY_TITLE_PREFIX + base.GetDisplayTitle();
        }

        public override void OnInspectorGUI()
        {
            EditorUtilities.DrawHeaderLabel("Edge Property");
            PropertyField(edgeWidth);
            PropertyField(edgeColor, new GUIContent("Edge Color"));
            EditorUtilities.DrawHeaderLabel("Background Property");
            PropertyField(backgroundFade);
            PropertyField(backgroundColor);

        }

    }
}
        

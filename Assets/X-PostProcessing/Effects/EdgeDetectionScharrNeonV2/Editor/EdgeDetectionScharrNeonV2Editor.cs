
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
    [PostProcessEditor(typeof(EdgeDetectionScharrNeonV2))]
    public sealed class EdgeDetectionScharrNeonV2Editor : PostProcessEffectEditor<EdgeDetectionScharrNeonV2>
    {

        SerializedParameterOverride EdgeWidth;
        SerializedParameterOverride EdgeNeonFade;
        SerializedParameterOverride Brigtness;
        SerializedParameterOverride BackgroundFade;
        SerializedParameterOverride BackgroundColor;

        public override void OnEnable()
        {
            EdgeWidth = FindParameterOverride(x => x.EdgeWidth);
            EdgeNeonFade = FindParameterOverride(x => x.EdgeNeonFade);
            Brigtness = FindParameterOverride(x => x.Brigtness);
            BackgroundFade = FindParameterOverride(x => x.BackgroundFade);
            BackgroundColor = FindParameterOverride(x => x.BackgroundColor);
        }

        public override string GetDisplayTitle()
        {
            return XPostProcessingEditorUtility.DISPLAY_TITLE_PREFIX + base.GetDisplayTitle();
        }

        public override void OnInspectorGUI()
        {
            EditorUtilities.DrawHeaderLabel("Edge Property");
            PropertyField(EdgeWidth);
            PropertyField(EdgeNeonFade);

            EditorUtilities.DrawHeaderLabel("Background Property( For Edge Neon Fade <1 )");
            PropertyField(BackgroundFade);
            PropertyField(BackgroundColor);

            EditorUtilities.DrawHeaderLabel("Edge Property");
            PropertyField(Brigtness);

        }
    }
}
        

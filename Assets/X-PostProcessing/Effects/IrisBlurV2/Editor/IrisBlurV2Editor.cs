
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
    [PostProcessEditor(typeof(IrisBlurV2))]
    public sealed class IrisBlurV2Editor : PostProcessEffectEditor<IrisBlurV2>
    {

        SerializedParameterOverride centerOffsetX;
        SerializedParameterOverride centerOffsetY;
        SerializedParameterOverride AreaSize;
        SerializedParameterOverride Iteration;
        SerializedParameterOverride BlurRadius;
        SerializedParameterOverride showPreview;


        public override void OnEnable()
        {
            showPreview = FindParameterOverride(x => x.showPreview);
            centerOffsetX = FindParameterOverride(x => x.centerOffsetX);
            centerOffsetY = FindParameterOverride(x => x.centerOffsetY);
            AreaSize = FindParameterOverride(x => x.AreaSize);
            Iteration = FindParameterOverride(x => x.Iteration);
            BlurRadius = FindParameterOverride(x => x.BlurRadius);
        }

        public override string GetDisplayTitle()
        {
            return XPostProcessingEditorUtility.DISPLAY_TITLE_PREFIX + base.GetDisplayTitle();
        }

        public override void OnInspectorGUI()
        {
            EditorUtilities.DrawHeaderLabel("Blur Property");
            PropertyField(BlurRadius);
            PropertyField(Iteration);

            EditorUtilities.DrawHeaderLabel("Area Property");
            PropertyField(AreaSize);
            PropertyField(centerOffsetX);
            PropertyField(centerOffsetY);

            EditorUtilities.DrawHeaderLabel("Debug");
            PropertyField(showPreview);
        }

    }
}
        

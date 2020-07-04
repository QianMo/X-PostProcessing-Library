
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
    [PostProcessEditor(typeof(RapidVignette))]
    public sealed class RapidVignetteEditor : PostProcessEffectEditor<RapidVignette>
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
        


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
    [PostProcessEditor(typeof(AuroraVignette))]
    public sealed class AuroraVignetteEditor : PostProcessEffectEditor<AuroraVignette>
    {

        SerializedParameterOverride vignetteArea;
        SerializedParameterOverride vignetteSmothness;
        SerializedParameterOverride vignetteFading;
        SerializedParameterOverride colorChange;
        SerializedParameterOverride colorFactorR;
        SerializedParameterOverride colorFactorG;
        SerializedParameterOverride colorFactorB;
        SerializedParameterOverride flowSpeed;


        public override void OnEnable()
        {
            vignetteArea = FindParameterOverride(x => x.vignetteArea);
            vignetteSmothness = FindParameterOverride(x => x.vignetteSmothness);
            vignetteFading = FindParameterOverride(x => x.vignetteFading);
            colorChange = FindParameterOverride(x => x.colorChange);
            colorFactorR = FindParameterOverride(x => x.colorFactorR);
            colorFactorG = FindParameterOverride(x => x.colorFactorG);
            colorFactorB = FindParameterOverride(x => x.colorFactorB);
            flowSpeed = FindParameterOverride(x => x.flowSpeed);
        }

        public override string GetDisplayTitle()
        {
            return XPostProcessingEditorUtility.DISPLAY_TITLE_PREFIX + base.GetDisplayTitle();
        }

        public override void OnInspectorGUI()
        {
            EditorUtilities.DrawHeaderLabel("Vignette");
            PropertyField(vignetteFading);
            PropertyField(vignetteArea);
            PropertyField(vignetteSmothness);

            EditorUtilities.DrawHeaderLabel("Speed");
            PropertyField(flowSpeed);

            EditorUtilities.DrawHeaderLabel("Color Adjustment");
            PropertyField(colorChange);
            PropertyField(colorFactorR);
            PropertyField(colorFactorG);
            PropertyField(colorFactorB);
        }

    }
}
        

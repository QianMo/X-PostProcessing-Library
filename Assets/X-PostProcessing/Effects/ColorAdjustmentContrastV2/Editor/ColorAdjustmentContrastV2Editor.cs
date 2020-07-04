
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
    [PostProcessEditor(typeof(ColorAdjustmentContrastV2))]
    public sealed class ColorAdjustmentContrastV2Editor : PostProcessEffectEditor<ColorAdjustmentContrastV2>
    {

        SerializedParameterOverride Contrast;
        SerializedParameterOverride ContrastFactorR;
        SerializedParameterOverride ContrastFactorG;
        SerializedParameterOverride ContrastFactorB;


        public override void OnEnable()
        {

            Contrast = FindParameterOverride(x => x.Contrast);
            ContrastFactorR = FindParameterOverride(x => x.ContrastFactorR);
            ContrastFactorG = FindParameterOverride(x => x.ContrastFactorG);
            ContrastFactorB = FindParameterOverride(x => x.ContrastFactorB);
        }

        public override string GetDisplayTitle()
        {
            return XPostProcessingEditorUtility.DISPLAY_TITLE_PREFIX + base.GetDisplayTitle();
        }

        public override void OnInspectorGUI()
        {
            EditorUtilities.DrawHeaderLabel("Core Property");
            PropertyField(Contrast);

            EditorUtilities.DrawHeaderLabel("RGB Channel");

            PropertyField(ContrastFactorR);
            PropertyField(ContrastFactorG);
            PropertyField(ContrastFactorB);

        }

    }
}
        

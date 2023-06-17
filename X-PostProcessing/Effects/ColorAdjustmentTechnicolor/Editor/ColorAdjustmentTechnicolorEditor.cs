
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
    [PostProcessEditor(typeof(ColorAdjustmentTechnicolor))]
    public sealed class ColorAdjustmentTechnicolorEditor : PostProcessEffectEditor<ColorAdjustmentTechnicolor>
    {

        SerializedParameterOverride exposure;
        SerializedParameterOverride colorBalanceR;
        SerializedParameterOverride colorBalanceG;
        SerializedParameterOverride colorBalanceB;
        SerializedParameterOverride indensity;


        public override void OnEnable()
        {
            exposure = FindParameterOverride(x => x.exposure);
            colorBalanceR = FindParameterOverride(x => x.colorBalanceR);
            colorBalanceG = FindParameterOverride(x => x.colorBalanceG);
            colorBalanceB = FindParameterOverride(x => x.colorBalanceB);
            indensity = FindParameterOverride(x => x.indensity);
        }

        public override string GetDisplayTitle()
        {
            return XPostProcessingEditorUtility.DISPLAY_TITLE_PREFIX + base.GetDisplayTitle();
        }

        public override void OnInspectorGUI()
        {
            EditorUtilities.DrawHeaderLabel("Core Property");
            PropertyField(exposure);
            PropertyField(indensity);
            EditorUtilities.DrawHeaderLabel("Color Balance");
            PropertyField(colorBalanceR);
            PropertyField(colorBalanceG);
            PropertyField(colorBalanceB);

        }

    }
}
        

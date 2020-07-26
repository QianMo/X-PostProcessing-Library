
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
    [PostProcessEditor(typeof(ColorReplaceV2))]
    public sealed class ColorReplaceV2Editor : PostProcessEffectEditor<ColorReplaceV2>
    {
        SerializedParameterOverride FromGradientColor;
        SerializedParameterOverride ToGradientColor;
        SerializedParameterOverride gridentSpeed;
        SerializedParameterOverride Range;
        SerializedParameterOverride Fuzziness;


        public override void OnEnable()
        {
            FromGradientColor = FindParameterOverride(x => x.FromGradientColor);
            ToGradientColor = FindParameterOverride(x => x.ToGradientColor);
            gridentSpeed = FindParameterOverride(x => x.gridentSpeed);
            Range = FindParameterOverride(x => x.Range);
            Fuzziness = FindParameterOverride(x => x.Fuzziness);
        }

        public override string GetDisplayTitle()
        {
            return XPostProcessingEditorUtility.DISPLAY_TITLE_PREFIX + base.GetDisplayTitle();
        }

        public override void OnInspectorGUI()
        {
            EditorUtilities.DrawHeaderLabel("From-To Color");
            PropertyField(FromGradientColor);
            PropertyField(ToGradientColor);
            PropertyField(gridentSpeed);

            EditorUtilities.DrawHeaderLabel("Color Precision");
            PropertyField(Range);
            PropertyField(Fuzziness);
        }

    }
}
        

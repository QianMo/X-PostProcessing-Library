
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
    [PostProcessEditor(typeof(ColorReplace))]
    public sealed class ColorReplaceEditor : PostProcessEffectEditor<ColorReplace>
    {

        SerializedParameterOverride FromColor;
        SerializedParameterOverride ToColor;
        SerializedParameterOverride Range;
        SerializedParameterOverride Fuzziness;


        public override void OnEnable()
        {
            FromColor = FindParameterOverride(x => x.FromColor);
            ToColor = FindParameterOverride(x => x.ToColor);
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
            PropertyField(FromColor);
            PropertyField(ToColor);

            EditorUtilities.DrawHeaderLabel("Color Precision");
            PropertyField(Range);
            PropertyField(Fuzziness);
        }

    }
}
        

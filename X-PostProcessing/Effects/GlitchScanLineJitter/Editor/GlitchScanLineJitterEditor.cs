
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
    [PostProcessEditor(typeof(GlitchScanLineJitter))]
    public sealed class GlitchScanLineJitterEditor : PostProcessEffectEditor<GlitchScanLineJitter>
    {

        SerializedParameterOverride JitterDirection;
        SerializedParameterOverride JitterIndensity;
        SerializedParameterOverride intervalType;
        SerializedParameterOverride frequency;

        public override void OnEnable()
        {
            JitterDirection = FindParameterOverride(x => x.JitterDirection);
            JitterIndensity = FindParameterOverride(x => x.JitterIndensity);
            intervalType = FindParameterOverride(x => x.intervalType);
            frequency = FindParameterOverride(x => x.frequency);
        }

        public override string GetDisplayTitle()
        {
            return XPostProcessingEditorUtility.DISPLAY_TITLE_PREFIX + base.GetDisplayTitle();
        }

        public override void OnInspectorGUI()
        {
            EditorUtilities.DrawHeaderLabel("Jitter Direction");
            PropertyField(JitterDirection);
            EditorUtilities.DrawHeaderLabel("Interval Frequency");
            PropertyField(intervalType);
            PropertyField(frequency);
            EditorUtilities.DrawHeaderLabel("Jitter Property");
            PropertyField(JitterIndensity);
        }

    }
}
        

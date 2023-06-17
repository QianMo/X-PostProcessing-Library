
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
    [PostProcessEditor(typeof(GlitchAnalogNoise))]
    public sealed class GlitchAnalogNoiseEditor : PostProcessEffectEditor<GlitchAnalogNoise>
    {

        SerializedParameterOverride NoiseSpeed;
        SerializedParameterOverride NoiseFading;
        SerializedParameterOverride LuminanceJitterThreshold;


        public override void OnEnable()
        {
            NoiseSpeed = FindParameterOverride(x => x.NoiseSpeed);
            NoiseFading = FindParameterOverride(x => x.NoiseFading);
            LuminanceJitterThreshold = FindParameterOverride(x => x.LuminanceJitterThreshold);
        }

        public override string GetDisplayTitle()
        {
            return XPostProcessingEditorUtility.DISPLAY_TITLE_PREFIX + base.GetDisplayTitle();
        }

        public override void OnInspectorGUI()
        {
            EditorUtilities.DrawHeaderLabel("Core Property");
            PropertyField(NoiseSpeed);
            PropertyField(NoiseFading);
            PropertyField(LuminanceJitterThreshold);
        }

    }
}
        

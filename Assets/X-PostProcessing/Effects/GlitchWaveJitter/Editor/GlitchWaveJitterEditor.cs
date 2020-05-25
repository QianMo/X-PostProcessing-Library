
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
    [PostProcessEditor(typeof(GlitchWaveJitter))]
    public sealed class GlitchWaveJitterEditor : PostProcessEffectEditor<GlitchWaveJitter>
    {

        SerializedParameterOverride jitterDirection;
        SerializedParameterOverride intervalType;
        SerializedParameterOverride frequency;
        SerializedParameterOverride RGBSplit;
        SerializedParameterOverride speed;
        SerializedParameterOverride amount;
        SerializedParameterOverride customResolution;
        SerializedParameterOverride resolution;


        public override void OnEnable()
        {
            jitterDirection = FindParameterOverride(x => x.jitterDirection);
            intervalType = FindParameterOverride(x => x.intervalType);
            frequency = FindParameterOverride(x => x.frequency);
            RGBSplit = FindParameterOverride(x => x.RGBSplit);
            speed = FindParameterOverride(x => x.speed);
            amount = FindParameterOverride(x => x.amount);
            customResolution = FindParameterOverride(x => x.customResolution);
            resolution = FindParameterOverride(x => x.resolution);
        }

        public override string GetDisplayTitle()
        {
            return XPostProcessingEditorUtility.DISPLAY_TITLE_PREFIX + base.GetDisplayTitle();
        }

        public override void OnInspectorGUI()
        {

            EditorUtilities.DrawHeaderLabel("Jitter Direction");
            PropertyField(jitterDirection);

            EditorUtilities.DrawHeaderLabel("Interval Frequency");
            PropertyField(intervalType);

            if (intervalType.value.enumValueIndex != (int)IntervalType.Infinite)
            {
                PropertyField(frequency);
            }

            EditorUtilities.DrawHeaderLabel("Core Property");
            PropertyField(RGBSplit);
            PropertyField(speed);
            PropertyField(amount);

            EditorUtilities.DrawHeaderLabel("Custom Jitter Resolution");
            PropertyField(customResolution);
            PropertyField(resolution);


        }

    }
}
        

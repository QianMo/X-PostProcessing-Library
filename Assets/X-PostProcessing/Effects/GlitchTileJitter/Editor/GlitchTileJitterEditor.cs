
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
    [PostProcessEditor(typeof(GlitchTileJitter))]
    public sealed class GlitchTileJitterEditor : PostProcessEffectEditor<GlitchTileJitter>
    {

        SerializedParameterOverride jitterDirection;
        SerializedParameterOverride intervalType;
        SerializedParameterOverride frequency;
        SerializedParameterOverride splittingDirection;
        SerializedParameterOverride splittingNumber;
        SerializedParameterOverride amount;
        SerializedParameterOverride speed;

        public override void OnEnable()
        {

            jitterDirection = FindParameterOverride(x => x.jitterDirection);
            intervalType = FindParameterOverride(x => x.intervalType);
            frequency = FindParameterOverride(x => x.frequency);
            splittingNumber = FindParameterOverride(x => x.splittingNumber);
            splittingDirection = FindParameterOverride(x => x.splittingDirection);
            amount = FindParameterOverride(x => x.amount);
            speed = FindParameterOverride(x => x.speed);
        }

        public override string GetDisplayTitle()
        {
            return XPostProcessingEditorUtility.DISPLAY_TITLE_PREFIX + base.GetDisplayTitle();
        }

        public override void OnInspectorGUI()
        {
            EditorUtilities.DrawHeaderLabel("Splitting Property");
            PropertyField(splittingDirection);
            PropertyField(splittingNumber);

            EditorUtilities.DrawHeaderLabel("Interval Frequency");
            PropertyField(intervalType);
            if (intervalType.value.enumValueIndex != (int)IntervalType.Infinite)
            {
                PropertyField(frequency);
            }

            EditorUtilities.DrawHeaderLabel("Jitter Property");
            PropertyField(jitterDirection);
            PropertyField(amount);
            PropertyField(speed);

        }

    }
}
        

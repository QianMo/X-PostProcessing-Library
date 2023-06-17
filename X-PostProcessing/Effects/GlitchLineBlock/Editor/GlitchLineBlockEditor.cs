
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
    [PostProcessEditor(typeof(GlitchLineBlock))]
    public sealed class GlitchLineBlockEditor : PostProcessEffectEditor<GlitchLineBlock>
    {
        SerializedParameterOverride blockDirection;
        SerializedParameterOverride intervalType;
        SerializedParameterOverride frequency;
        SerializedParameterOverride Amount;
        SerializedParameterOverride LinesWidth;
        SerializedParameterOverride Speed;
        SerializedParameterOverride Offset;
        SerializedParameterOverride Alpha;


        public override void OnEnable()
        {
            blockDirection = FindParameterOverride(x => x.blockDirection);
            intervalType = FindParameterOverride(x => x.intervalType);
            frequency = FindParameterOverride(x => x.frequency);
            Amount = FindParameterOverride(x => x.Amount);
            LinesWidth = FindParameterOverride(x => x.LinesWidth);
            Speed = FindParameterOverride(x => x.Speed);
            Offset = FindParameterOverride(x => x.Offset);
            Alpha = FindParameterOverride(x => x.Alpha);
        }

        public override string GetDisplayTitle()
        {
            return XPostProcessingEditorUtility.DISPLAY_TITLE_PREFIX + base.GetDisplayTitle();
        }

        public override void OnInspectorGUI()
        {
            EditorUtilities.DrawHeaderLabel("Block Direction");
            PropertyField(blockDirection);

            EditorUtilities.DrawHeaderLabel("Interval Frequency");
            PropertyField(intervalType);

            if (intervalType.value.enumValueIndex != (int)IntervalType.Infinite)
            {
                PropertyField(frequency);
            }

            EditorUtilities.DrawHeaderLabel("Core Property");
            PropertyField(Amount);
            PropertyField(LinesWidth);
            PropertyField(Speed);
            PropertyField(Offset);
            PropertyField(Alpha);
        }

    }
}
        

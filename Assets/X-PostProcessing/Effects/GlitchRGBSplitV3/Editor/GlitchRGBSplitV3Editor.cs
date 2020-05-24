
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
    [PostProcessEditor(typeof(GlitchRGBSplitV3))]
    public sealed class GlitchRGBSplitV3Editor : PostProcessEffectEditor<GlitchRGBSplitV3>
    {

        SerializedParameterOverride SplitDirection;
        SerializedParameterOverride intervalType;
        SerializedParameterOverride Frequency;
        SerializedParameterOverride Amount;
        SerializedParameterOverride Speed;


        public override void OnEnable()
        {
            SplitDirection = FindParameterOverride(x => x.SplitDirection);
            intervalType = FindParameterOverride(x => x.intervalType);
            Frequency = FindParameterOverride(x => x.Frequency);
            Amount = FindParameterOverride(x => x.Amount);
            Speed = FindParameterOverride(x => x.Speed);
        }

        public override string GetDisplayTitle()
        {
            return XPostProcessingEditorUtility.DISPLAY_TITLE_PREFIX + base.GetDisplayTitle();
        }

        public override void OnInspectorGUI()
        {

            EditorUtilities.DrawHeaderLabel("split Direction");
            PropertyField(SplitDirection);

            EditorUtilities.DrawHeaderLabel("Interval Frequency");
            PropertyField(intervalType);
            if (intervalType.value.enumValueIndex != (int)IntervalType.Infinite)
            {
                PropertyField(Frequency);
            }

            EditorUtilities.DrawHeaderLabel("Core Property");
            PropertyField(Amount);
            PropertyField(Speed);

        }

    }
}
        

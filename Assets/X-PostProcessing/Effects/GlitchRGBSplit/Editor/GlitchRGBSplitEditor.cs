
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
    [PostProcessEditor(typeof(GlitchRGBSplit))]
    public sealed class GlitchRGBSplitEditor : PostProcessEffectEditor<GlitchRGBSplit>
    {

        SerializedParameterOverride SplitDirection;
        SerializedParameterOverride Fading;
        SerializedParameterOverride Amount;
        SerializedParameterOverride Speed;
        SerializedParameterOverride AmountR;
        SerializedParameterOverride AmountB;
        SerializedParameterOverride CenterFading;
        

        public override void OnEnable()
        {
            SplitDirection = FindParameterOverride(x => x.SplitDirection);
            Fading = FindParameterOverride(x => x.Fading);
            Amount = FindParameterOverride(x => x.Amount);
            Speed = FindParameterOverride(x => x.Speed);
            AmountR = FindParameterOverride(x => x.AmountR);
            AmountB = FindParameterOverride(x => x.AmountB);
            CenterFading = FindParameterOverride(x => x.CenterFading);
        }

        public override string GetDisplayTitle()
        {
            return XPostProcessingEditorUtility.DISPLAY_TITLE_PREFIX + base.GetDisplayTitle();
        }

        public override void OnInspectorGUI()
        {
            EditorUtilities.DrawHeaderLabel("Split Direction");
            PropertyField(SplitDirection);

            EditorUtilities.DrawHeaderLabel("Core Property");
            PropertyField(Amount);
            PropertyField(Speed);
            PropertyField(Fading);
            PropertyField(CenterFading);

            EditorUtilities.DrawHeaderLabel("RGB Channel Amount");
            PropertyField(AmountR);
            PropertyField(AmountB);
        }

    }
}
        

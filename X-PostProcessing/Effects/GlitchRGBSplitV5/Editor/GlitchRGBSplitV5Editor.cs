
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
    [PostProcessEditor(typeof(GlitchRGBSplitV5))]
    public sealed class GlitchRGBSplitV5Editor : PostProcessEffectEditor<GlitchRGBSplitV5>
    {

        SerializedParameterOverride Amplitude;
        SerializedParameterOverride Speed;


        public override void OnEnable()
        {
            Amplitude = FindParameterOverride(x => x.Amplitude);
            Speed = FindParameterOverride(x => x.Speed);
        }

        public override string GetDisplayTitle()
        {
            return XPostProcessingEditorUtility.DISPLAY_TITLE_PREFIX + base.GetDisplayTitle();
        }

        public override void OnInspectorGUI()
        {
            EditorUtilities.DrawHeaderLabel("Core Property");
            PropertyField(Amplitude);
            PropertyField(Speed);
        }

    }
}
        

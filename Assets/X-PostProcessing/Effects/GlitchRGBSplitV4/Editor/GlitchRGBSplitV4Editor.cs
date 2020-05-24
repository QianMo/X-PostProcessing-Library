
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
    [PostProcessEditor(typeof(GlitchRGBSplitV4))]
    public sealed class GlitchRGBSplitV4Editor : PostProcessEffectEditor<GlitchRGBSplitV4>
    {
        SerializedParameterOverride splitDirection;
        SerializedParameterOverride indensity;
        SerializedParameterOverride speed;


        public override void OnEnable()
        {
            splitDirection = FindParameterOverride(x => x.splitDirection);
            indensity = FindParameterOverride(x => x.indensity);
            speed = FindParameterOverride(x => x.speed);

        }

        public override string GetDisplayTitle()
        {
            return XPostProcessingEditorUtility.DISPLAY_TITLE_PREFIX + base.GetDisplayTitle();
        }

        public override void OnInspectorGUI()
        {
            EditorUtilities.DrawHeaderLabel("Split Direction");
            PropertyField(splitDirection);
            EditorUtilities.DrawHeaderLabel("Core Property");
            PropertyField(indensity);
            PropertyField(speed);
        }
    }
}
        

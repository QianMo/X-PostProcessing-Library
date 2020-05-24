
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
    [PostProcessEditor(typeof(GlitchImageBlockV4))]
    public sealed class GlitchImageBlockV4Editor : PostProcessEffectEditor<GlitchImageBlockV4>
    {

        SerializedParameterOverride Speed;
        SerializedParameterOverride BlockSize;
        SerializedParameterOverride MaxOffsetX;
        SerializedParameterOverride MaxOffsetY;



        public override void OnEnable()
        {
            Speed = FindParameterOverride(x => x.Speed);
            BlockSize = FindParameterOverride(x => x.BlockSize);
            MaxOffsetX = FindParameterOverride(x => x.MaxOffsetX);
            MaxOffsetY = FindParameterOverride(x => x.MaxOffsetY);
        }

        public override string GetDisplayTitle()
        {
            return XPostProcessingEditorUtility.DISPLAY_TITLE_PREFIX + base.GetDisplayTitle();
        }

        public override void OnInspectorGUI()
        {
            EditorUtilities.DrawHeaderLabel("Core Property");
            PropertyField(Speed);
            PropertyField(BlockSize);
            PropertyField(MaxOffsetX);
            PropertyField(MaxOffsetY);
        }

    }
}
        

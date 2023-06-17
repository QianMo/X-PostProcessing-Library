
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
    [PostProcessEditor(typeof(GlitchImageBlock))]
    public sealed class GlitchImageBlockEditor : PostProcessEffectEditor<GlitchImageBlock>
    {


        SerializedParameterOverride Fade;
        SerializedParameterOverride Speed;
        SerializedParameterOverride Amount;
        SerializedParameterOverride BlockLayer1_U;
        SerializedParameterOverride BlockLayer1_V;
        SerializedParameterOverride BlockLayer2_U;
        SerializedParameterOverride BlockLayer2_V;
        SerializedParameterOverride BlockLayer1_Indensity;
        SerializedParameterOverride BlockLayer2_Indensity;
        SerializedParameterOverride RGBSplitIndensity;
        SerializedParameterOverride BlockVisualizeDebug;

        

        //[Range(0.0f, 50.0f)]
        //public FloatParameter BlockLayer1_Indensity = new FloatParameter { value = 8f };

        //[Range(0.0f, 50.0f)]
        //public FloatParameter BlockLayer2_Indensity = new FloatParameter { value = 4f };

        public override void OnEnable()
        {
            Fade = FindParameterOverride(x => x.Fade);
            Speed = FindParameterOverride(x => x.Speed);
            Amount = FindParameterOverride(x => x.Amount);
            BlockLayer1_U = FindParameterOverride(x => x.BlockLayer1_U);
            BlockLayer1_V = FindParameterOverride(x => x.BlockLayer1_V);
            BlockLayer2_U = FindParameterOverride(x => x.BlockLayer2_U);
            BlockLayer2_V = FindParameterOverride(x => x.BlockLayer2_V);
            BlockLayer1_Indensity = FindParameterOverride(x => x.BlockLayer1_Indensity);
            BlockLayer2_Indensity = FindParameterOverride(x => x.BlockLayer2_Indensity);
            RGBSplitIndensity = FindParameterOverride(x => x.RGBSplitIndensity);
            BlockVisualizeDebug = FindParameterOverride(x => x.BlockVisualizeDebug);

        }

        public override string GetDisplayTitle()
        {
            return XPostProcessingEditorUtility.DISPLAY_TITLE_PREFIX + base.GetDisplayTitle();
        }

        public override void OnInspectorGUI()
        {
            EditorUtilities.DrawHeaderLabel("Core Property");
            PropertyField(Fade);
            PropertyField(Speed);
            PropertyField(Amount);
            EditorUtilities.DrawHeaderLabel("Block Noise Size");
            PropertyField(BlockLayer1_U);
            PropertyField(BlockLayer1_V);

            EditorGUILayout.Space();
            PropertyField(BlockLayer2_U);
            PropertyField(BlockLayer2_V);
            EditorUtilities.DrawHeaderLabel("Block Indensity");
            PropertyField(BlockLayer1_Indensity);
            PropertyField(BlockLayer2_Indensity);
            PropertyField(RGBSplitIndensity);

            
            EditorUtilities.DrawHeaderLabel("Block Visualize Debug");
            PropertyField(BlockVisualizeDebug);
        }

    }
}
        


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
    [PostProcessEditor(typeof(GlitchDigitalStripe))]
    public sealed class GlitchDigitalStripeEditor : PostProcessEffectEditor<GlitchDigitalStripe>
    {

        SerializedParameterOverride intensity;
        SerializedParameterOverride frequncy;
        SerializedParameterOverride stripeLength;               
        SerializedParameterOverride noiseTextureWidth;
        SerializedParameterOverride noiseTextureHeight;
        SerializedParameterOverride needStripColorAdjust;
        SerializedParameterOverride StripColorAdjustIndensity;
        SerializedParameterOverride StripColorAdjustColor;




        public override void OnEnable()
        {
            intensity = FindParameterOverride(x => x.intensity);
            frequncy = FindParameterOverride(x => x.frequncy);
            stripeLength = FindParameterOverride(x => x.stripeLength);
            noiseTextureHeight = FindParameterOverride(x => x.noiseTextureHeight);
            noiseTextureWidth = FindParameterOverride(x => x.noiseTextureWidth);
            needStripColorAdjust = FindParameterOverride(x => x.needStripColorAdjust);
            StripColorAdjustIndensity = FindParameterOverride(x => x.StripColorAdjustIndensity);
            StripColorAdjustColor = FindParameterOverride(x => x.StripColorAdjustColor);
        }

        public override string GetDisplayTitle()
        {
            return XPostProcessingEditorUtility.DISPLAY_TITLE_PREFIX + base.GetDisplayTitle();
        }

        public override void OnInspectorGUI()
        {
            EditorUtilities.DrawHeaderLabel("Core Property");
            PropertyField(intensity);
            PropertyField(frequncy);


            EditorUtilities.DrawHeaderLabel("Stripe Generate");
            PropertyField(stripeLength);

            EditorUtilities.DrawHeaderLabel("Noise Texture Size");
            PropertyField(noiseTextureWidth);
            PropertyField(noiseTextureHeight);

            EditorUtilities.DrawHeaderLabel("Strip Color Adjust");
            PropertyField(needStripColorAdjust);
            PropertyField(StripColorAdjustIndensity);
            PropertyField(StripColorAdjustColor);


        }

    }
}
        

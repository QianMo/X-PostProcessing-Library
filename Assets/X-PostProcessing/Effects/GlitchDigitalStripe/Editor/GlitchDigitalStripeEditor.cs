
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
        SerializedParameterOverride stripeWidth;


        public override void OnEnable()
        {
            intensity = FindParameterOverride(x => x.intensity);
            frequncy = FindParameterOverride(x => x.frequncy);
            stripeLength = FindParameterOverride(x => x.stripeLength);
            stripeWidth = FindParameterOverride(x => x.stripeWidth);
        }

        public override string GetDisplayTitle()
        {
            return XPostProcessingEditorUtility.DISPLAY_TITLE_PREFIX + base.GetDisplayTitle();
        }

        public override void OnInspectorGUI()
        {
            PropertyField(intensity);
            PropertyField(frequncy);
            PropertyField(stripeLength);
            PropertyField(stripeWidth);

        }

    }
}
        

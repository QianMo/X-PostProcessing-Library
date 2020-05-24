
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
    [PostProcessEditor(typeof(GlitchDigitalStripeV2))]
    public sealed class GlitchDigitalStripeV2Editor : PostProcessEffectEditor<GlitchDigitalStripeV2>
    {

        SerializedParameterOverride speed;
        SerializedParameterOverride intensity;
        SerializedParameterOverride resolutionMultiplier;
        SerializedParameterOverride stretchMultiplier;
        //SerializedParameterOverride colorProperty1;
        //SerializedParameterOverride bumpMap;


        public override void OnEnable()
        {
            speed = FindParameterOverride(x => x.speed);
            intensity = FindParameterOverride(x => x.intensity);
            resolutionMultiplier = FindParameterOverride(x => x.resolutionMultiplier);
            stretchMultiplier = FindParameterOverride(x => x.stretchMultiplier);
            //colorProperty1 = FindParameterOverride(x => x.colorProperty1);
            //bumpMap = FindParameterOverride(x => x.bumpMap);
        }

        public override string GetDisplayTitle()
        {
            return XPostProcessingEditorUtility.DISPLAY_TITLE_PREFIX + base.GetDisplayTitle();
        }

        public override void OnInspectorGUI()
        {
            EditorUtilities.DrawHeaderLabel("Core Property");
            PropertyField(speed);
            PropertyField(intensity);
            PropertyField(resolutionMultiplier);
            PropertyField(stretchMultiplier);
            //PropertyField(colorProperty1);

            //PropertyField(bumpMap);

            //if (speed.value.floatValue == 1)
            //{
            //    PropertyField(bumpMap);
            //}
        }

    }
}
        


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
    [PostProcessEditor(typeof(RadialBlurV2))]
    public sealed class RadialBlurV2Editor : PostProcessEffectEditor<RadialBlurV2>
    {
        SerializedParameterOverride QualityLevel;
        SerializedParameterOverride BlurRadius;
        SerializedParameterOverride RadialCenterX;
        SerializedParameterOverride RadialCenterY;



        public override void OnEnable()
        {
            QualityLevel = FindParameterOverride(x => x.QualityLevel);
            BlurRadius = FindParameterOverride(x => x.BlurRadius);
            RadialCenterX = FindParameterOverride(x => x.RadialCenterX);
            RadialCenterY = FindParameterOverride(x => x.RadialCenterY);
        }

        public override string GetDisplayTitle()
        {
            return XPostProcessingEditorUtility.DISPLAY_TITLE_PREFIX + base.GetDisplayTitle();
        }

        public override void OnInspectorGUI()
        {

            PropertyField(QualityLevel);
            PropertyField(BlurRadius);

            EditorUtilities.DrawHeaderLabel("Radial Center");
            PropertyField(RadialCenterX);
            PropertyField(RadialCenterY);
        }

    }
}
        

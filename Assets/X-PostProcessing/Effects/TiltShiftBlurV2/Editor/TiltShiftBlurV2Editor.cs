
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
    [PostProcessEditor(typeof(TiltShiftBlurV2))]
    public sealed class TiltShiftBlurV2Editor : PostProcessEffectEditor<TiltShiftBlurV2>
    {

        SerializedParameterOverride Iteration;
        SerializedParameterOverride BlurRadius;
        SerializedParameterOverride centerOffset;
        SerializedParameterOverride AreaSize;
        SerializedParameterOverride areaSmooth;
        SerializedParameterOverride showPreview;


        public override void OnEnable()
        {
            showPreview = FindParameterOverride(x => x.showPreview);
            centerOffset = FindParameterOverride(x => x.centerOffset);
            AreaSize = FindParameterOverride(x => x.AreaSize);
            areaSmooth = FindParameterOverride(x => x.areaSmooth);
            Iteration = FindParameterOverride(x => x.Iteration);
             BlurRadius = FindParameterOverride(x => x.BlurRadius);
        }

        public override string GetDisplayTitle()
        {
            return XPostProcessingEditorUtility.DISPLAY_TITLE_PREFIX + base.GetDisplayTitle();
        }

        public override void OnInspectorGUI()
        {

            EditorUtilities.DrawHeaderLabel("Blur Property");
            PropertyField(BlurRadius);
            PropertyField(Iteration);

            EditorUtilities.DrawHeaderLabel("Area Property");
            PropertyField(centerOffset);
            PropertyField(AreaSize);
            PropertyField(areaSmooth);

            EditorUtilities.DrawHeaderLabel("Debug");
            PropertyField(showPreview);

        }

    }
}
        

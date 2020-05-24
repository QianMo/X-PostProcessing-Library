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
    [PostProcessEditor(typeof(PixelizeCircle))]
    public sealed class PixelizeCircleEditor : PostProcessEffectEditor<PixelizeCircle>
    {

        SerializedParameterOverride pixelSize;
        SerializedParameterOverride circleRadius;
        SerializedParameterOverride pixelIntervalX;
        SerializedParameterOverride pixelIntervalY;
        SerializedParameterOverride BackgroundColor;

        public override void OnEnable()
        {
            pixelSize = FindParameterOverride(x => x.pixelSize);
            circleRadius = FindParameterOverride(x => x.circleRadius);
            pixelIntervalX = FindParameterOverride(x => x.pixelIntervalX);
            pixelIntervalY = FindParameterOverride(x => x.pixelIntervalY);
            BackgroundColor = FindParameterOverride(x => x.BackgroundColor);
        }

        public override string GetDisplayTitle()
        {
            return XPostProcessingEditorUtility.DISPLAY_TITLE_PREFIX + base.GetDisplayTitle();
        }

        public override void OnInspectorGUI()
        {
            EditorUtilities.DrawHeaderLabel("Core Property");
            PropertyField(pixelSize);
            PropertyField(circleRadius);
            PropertyField(BackgroundColor);

            EditorUtilities.DrawHeaderLabel("Pixel Interval");
            PropertyField(pixelIntervalX);
            PropertyField(pixelIntervalY);
        }

    }
}
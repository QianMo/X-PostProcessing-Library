
//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License 
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;


namespace XPostProcessing
{

    [Serializable]
    [PostProcess(typeof(ColorAdjustmentContrastV3Renderer), PostProcessEvent.AfterStack, "X-PostProcessing/ColorAdjustment/ContrastV3")]
    public class ColorAdjustmentContrastV3 : PostProcessEffectSettings
    {


        [DisplayName("Contrast Wheel"),ColorWheel(ColorWheelAttribute.Mode.Contrast)]
        public Vector4Parameter contrast = new Vector4Parameter { value = new Vector4(1f, 1f, 1f, -0.1f) };
    }

    public sealed class ColorAdjustmentContrastV3Renderer : PostProcessEffectRenderer<ColorAdjustmentContrastV3>
    {
        private const string PROFILER_TAG = "X-ColorAdjustmentContrastV3";

        private Shader shader;

        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/ColorAdjustment/ContrastV3");
        }

        public override void Release()
        {
            base.Release();
        }

        public override void Render(PostProcessRenderContext context)
        {
            CommandBuffer cmd = context.command;
            PropertySheet sheet = context.propertySheets.Get(shader);
            cmd.BeginSample(PROFILER_TAG);


            sheet.properties.SetVector("_Contrast", settings.contrast);


            cmd.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
            cmd.EndSample(PROFILER_TAG);
        }
    }
}
        

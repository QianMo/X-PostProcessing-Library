
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
    [PostProcess(typeof(ColorAdjustmentContrastRenderer), PostProcessEvent.AfterStack, "X-PostProcessing/ColorAdjustment/Contrast")]
    public class ColorAdjustmentContrast : PostProcessEffectSettings
    {

        [Range(-1.0f, 2.0f)]
        public FloatParameter contrast = new FloatParameter { value = 0.2f };

    }

    public sealed class ColorAdjustmentContrastRenderer : PostProcessEffectRenderer<ColorAdjustmentContrast>
    {
        private Shader shader;
        private const string PROFILER_TAG = "X-ColorAdjustmentContrast";

        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/ColorAdjustment/Contrast");
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

            sheet.properties.SetFloat("_Contrast", settings.contrast + 1);


            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
            cmd.EndSample(PROFILER_TAG);
        }
    }
}
        

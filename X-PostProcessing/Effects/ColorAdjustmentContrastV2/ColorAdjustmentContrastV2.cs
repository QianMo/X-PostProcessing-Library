
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
    [PostProcess(typeof(ColorAdjustmentContrastV2Renderer), PostProcessEvent.AfterStack, "X-PostProcessing/ColorAdjustment/ContrastV2")]
    public class ColorAdjustmentContrastV2 : PostProcessEffectSettings
    {

        [Range(-1.0f, 5.0f)]
        public FloatParameter Contrast = new FloatParameter { value = 0.2f };

        [Range(-1.0f, 1.0f)]
        public FloatParameter ContrastFactorR = new FloatParameter { value = 0.0f };

        [Range(-1.0f, 1.0f)]
        public FloatParameter ContrastFactorG = new FloatParameter { value = 0.0f };

        [Range(-1.0f, 1.0f)]
        public FloatParameter ContrastFactorB = new FloatParameter { value = 0.0f };

    }

    public sealed class ColorAdjustmentContrastV2Renderer : PostProcessEffectRenderer<ColorAdjustmentContrastV2>
    {
        private Shader shader;
        private const string PROFILER_TAG = "X-ColorAdjustmentContrastV2";

        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/ColorAdjustment/ContrastV2");
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

            sheet.properties.SetFloat("_Contrast", settings.Contrast + 1);
            sheet.properties.SetVector("_ContrastFactorRGB", new Vector3(settings.ContrastFactorR, settings.ContrastFactorG , settings.ContrastFactorB));


            cmd.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
            cmd.EndSample(PROFILER_TAG);
        }
    }
}

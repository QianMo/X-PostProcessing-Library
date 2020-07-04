
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
    [PostProcess(typeof(ColorAdjustmentSaturationRenderer), PostProcessEvent.AfterStack, "X-PostProcessing/ColorAdjustment/Saturation")]
    public class ColorAdjustmentSaturation : PostProcessEffectSettings
    {

        [Range(0.0f, 2.0f)]
        public FloatParameter saturation = new FloatParameter { value = 1f };

    }

    public sealed class ColorAdjustmentSaturationRenderer : PostProcessEffectRenderer<ColorAdjustmentSaturation>
    {
        private Shader shader;

        private const string PROFILER_TAG = "X-ColorAdjustmentSaturation";

        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/ColorAdjustment/Saturation");
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

            sheet.properties.SetFloat("_Saturation", settings.saturation);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
            cmd.EndSample(PROFILER_TAG);
        }
    }
}
        

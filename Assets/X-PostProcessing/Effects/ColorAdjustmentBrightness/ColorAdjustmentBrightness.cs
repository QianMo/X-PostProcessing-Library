
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
    [PostProcess(typeof(ColorAdjustmentBrightnessRenderer), PostProcessEvent.AfterStack, "X-PostProcessing/ColorAdjustment/Brightness")]
    public class ColorAdjustmentBrightness : PostProcessEffectSettings
    {
        [Range(-0.9f ,1f)]
        public FloatParameter brightness = new FloatParameter { value = 0f };
    } 

    public sealed class ColorAdjustmentBrightnessRenderer : PostProcessEffectRenderer<ColorAdjustmentBrightness>
    {
        private Shader shader;
        private const string PROFILER_TAG = "X-ColorAdjustmentBrightness";

        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/ColorAdjustment/Brightness");
        }

        public override void Release()
        {
            base.Release();
        }

        static class ShaderIDs
        {
            internal static readonly int brightness = Shader.PropertyToID("_Brightness");
        }

        public override void Render(PostProcessRenderContext context)
        {
            CommandBuffer cmd = context.command;
            PropertySheet sheet = context.propertySheets.Get(shader);
            cmd.BeginSample(PROFILER_TAG);

            sheet.properties.SetFloat(ShaderIDs.brightness, settings.brightness + 1f);

            cmd.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);

            cmd.EndSample(PROFILER_TAG);
        }
    }
}
        

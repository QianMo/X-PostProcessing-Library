
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
    [PostProcess(typeof(ColorAdjustmentHueRenderer), PostProcessEvent.AfterStack, "X-PostProcessing/ColorAdjustment/Hue")]
    public class ColorAdjustmentHue : PostProcessEffectSettings
    {

        [Range(-180.0f, 180.0f)]
        public FloatParameter HueDegree = new FloatParameter { value = 20f };
    }

    public sealed class ColorAdjustmentHueRenderer : PostProcessEffectRenderer<ColorAdjustmentHue>
    {
        private Shader shader;

        private const string PROFILER_TAG = "X-ColorAdjustmentHue";

        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/ColorAdjustment/Hue");
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

            sheet.properties.SetFloat("_HueDegree", settings.HueDegree);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
            cmd.EndSample(PROFILER_TAG);
        }
    }
}
        

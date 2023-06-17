
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
    [PostProcess(typeof(ColorAdjustmentTintRenderer), PostProcessEvent.AfterStack, "X-PostProcessing/ColorAdjustment/Tint")]
    public class ColorAdjustmentTint : PostProcessEffectSettings
    {

        [Range(0.0f, 1.0f)]
        public FloatParameter indensity = new FloatParameter { value = 0.1f };

        [ColorUsageAttribute(true, true, 0f, 20f, 0.125f, 3f)]
        public ColorParameter colorTint = new ColorParameter { value = new Color(0.9f, 1.0f, 0.0f, 1) };
    }

    public sealed class ColorAdjustmentTintRenderer : PostProcessEffectRenderer<ColorAdjustmentTint>
    {
        private Shader shader;
        private const string PROFILER_TAG = "X-ColorAdjustmentTint";

        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/ColorAdjustment/Tint");
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

            sheet.properties.SetColor("_ColorTint", settings.colorTint);
            sheet.properties.SetFloat("_Indensity", settings.indensity);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
            cmd.EndSample(PROFILER_TAG);
        }
    }
}
        

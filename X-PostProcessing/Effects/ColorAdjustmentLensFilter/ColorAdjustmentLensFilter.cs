
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
using UnityEngine.Rendering.PostProcessing;


namespace XPostProcessing
{

    [Serializable]
    [PostProcess(typeof(ColorAdjustmentLensFilterRenderer), PostProcessEvent.AfterStack, "X-PostProcessing/ColorAdjustment/LensFilter")]
    public class ColorAdjustmentLensFilter : PostProcessEffectSettings
    {

        [ColorUsageAttribute(true, true, 0f, 20f, 0.125f, 3f)]
        public ColorParameter LensColor = new ColorParameter { value = new Color(1.0f, 1.0f, 0.1f, 1) };

        [Range(0.0f, 1.0f)]
        public FloatParameter Indensity = new FloatParameter { value = 0.5f };
    }

    public sealed class ColorAdjustmentLensFilterRenderer : PostProcessEffectRenderer<ColorAdjustmentLensFilter>
    {
        private Shader shader;
        private const string PROFILER_TAG = "X-ColorAdjustmentLensFilter";


        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/ColorAdjustment/LensFilter");
        }

        public override void Release()
        {
            base.Release();
        }

        static class ShaderIDs
        {
            internal static readonly int LensColor = Shader.PropertyToID("_LensColor");
            internal static readonly int Indensity = Shader.PropertyToID("_Indensity");
        }

        public override void Render(PostProcessRenderContext context)
        {
            context.command.BeginSample(PROFILER_TAG);
            PropertySheet sheet = context.propertySheets.Get(shader);

            sheet.properties.SetFloat(ShaderIDs.Indensity, settings.Indensity);
            sheet.properties.SetColor(ShaderIDs.LensColor, settings.LensColor);


            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
            context.command.EndSample(PROFILER_TAG);
        }
    }
}
        

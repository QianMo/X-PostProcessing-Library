
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
using XPostProcessing;

namespace XPostProcessing
{

    [Serializable]
    [PostProcess(typeof(RapidVignetteV2Renderer), PostProcessEvent.AfterStack, "X-PostProcessing/Vignette/RapidVignetteV2")]
    public class RapidVignetteV2 : PostProcessEffectSettings
    {

        public VignetteTypeParameter vignetteType = new VignetteTypeParameter { value = VignetteType.ClassicMode };

        [Range(0.0f, 5.0f)]
        public FloatParameter vignetteIndensity = new FloatParameter { value = 0.2f };

        [Range(-1f, 1f)]
        public FloatParameter vignetteSharpness = new FloatParameter { value = 0.1f };

        public Vector2Parameter vignetteCenter = new Vector2Parameter { value = new Vector2(0.5f, 0.5f) };

        [ColorUsageAttribute(true, true, 0f, 20f, 0.125f, 3f)]
        public ColorParameter vignetteColor = new ColorParameter { value = new Color(0.1f, 0.8f, 1.0f) };


        public GradientParameter grident1 = new GradientParameter { value = null };

    }

    public sealed class RapidVignetteV2Renderer : PostProcessEffectRenderer<RapidVignetteV2>
    {
        private Shader shader;
        private const string PROFILER_TAG = "X-RapidVignetteV2";

        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/RapidVignetteV2");
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

            sheet.properties.SetFloat("_VignetteIndensity", settings.vignetteIndensity);
            sheet.properties.SetFloat("_VignetteSharpness", settings.vignetteSharpness);
            sheet.properties.SetVector("_VignetteCenter", settings.vignetteCenter);
            if (settings.vignetteType.value == VignetteType.ColorMode)
            {
                sheet.properties.SetColor("_VignetteColor", settings.vignetteColor);
            }

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, (int)settings.vignetteType.value);
            cmd.EndSample(PROFILER_TAG);
        }
    }
}
        

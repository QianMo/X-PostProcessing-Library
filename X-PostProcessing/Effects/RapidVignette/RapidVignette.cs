
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

    public enum VignetteType
    {
        ClassicMode = 0,
        ColorMode = 1,
    }

    [Serializable]
    public sealed class VignetteTypeParameter : ParameterOverride<VignetteType> { }



    [Serializable]
    [PostProcess(typeof(RapidVignetteRenderer), PostProcessEvent.AfterStack, "X-PostProcessing/Vignette/RapidVignette")]
    public class RapidVignette : PostProcessEffectSettings
    {
        public VignetteTypeParameter vignetteType = new VignetteTypeParameter { value = VignetteType.ClassicMode };

        [Range(0.0f, 5.0f)]
        public FloatParameter vignetteIndensity = new FloatParameter { value = 1f };

        public Vector2Parameter vignetteCenter = new Vector2Parameter { value = new Vector2(0.5f, 0.5f) };

        [ColorUsageAttribute(true, true, 0f, 20f, 0.125f, 3f)]
        public ColorParameter vignetteColor = new ColorParameter { value = new Color(0.1f, 0.8f, 1.0f) };
    }

    public sealed class RapidVignetteRenderer : PostProcessEffectRenderer<RapidVignette>
    {
        private Shader shader;

        private const string PROFILER_TAG = "X-RapidVignette";

        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/RapidVignette");
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
            sheet.properties.SetVector("_VignetteCenter", settings.vignetteCenter);

            if (settings.vignetteType.value == VignetteType.ColorMode)
            {
                sheet.properties.SetColor("_VignetteColor", settings.vignetteColor);
            }

            cmd.BlitFullscreenTriangle(context.source, context.destination, sheet, (int)settings.vignetteType.value);
            cmd.EndSample(PROFILER_TAG);

        }
    }
}
        

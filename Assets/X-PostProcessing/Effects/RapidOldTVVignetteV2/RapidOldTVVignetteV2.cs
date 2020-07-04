
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
    [PostProcess(typeof(RapidOldTVVignetteV2Renderer), PostProcessEvent.AfterStack, "X-PostProcessing/Vignette/RapidOldTVVignetteV2")]
    public class RapidOldTVVignetteV2 : PostProcessEffectSettings
    {
        public VignetteTypeParameter vignetteType = new VignetteTypeParameter { value = VignetteType.ClassicMode };

        [Range(1.0f, 5000.0f)]
        public FloatParameter vignetteSize = new FloatParameter { value = 20f };

        [Range(0.0f, 1.0f)]
        public FloatParameter sizeOffset = new FloatParameter { value = 0.2f };

        [ColorUsageAttribute(true, true, 0f, 20f, 0.125f, 3f)]
        public ColorParameter vignetteColor = new ColorParameter { value = new Color(0.1f, 0.8f, 1.0f) };
    }

    public sealed class RapidOldTVVignetteV2Renderer : PostProcessEffectRenderer<RapidOldTVVignetteV2>
    {
        private Shader shader;
        private const string PROFILER_TAG = "X-GlitchLineBlock";

        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/RapidOldTVVignetteV2");
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

            sheet.properties.SetFloat("_VignetteSize", settings.vignetteSize);
            sheet.properties.SetFloat("_SizeOffset", settings.sizeOffset);
            if (settings.vignetteType.value == VignetteType.ColorMode)
            {
                sheet.properties.SetColor("_VignetteColor", settings.vignetteColor);
            }

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, (int)settings.vignetteType.value);
            cmd.EndSample(PROFILER_TAG);
        }
    }
}
        

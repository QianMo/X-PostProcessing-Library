
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
    [PostProcess(typeof(AuroraVignetteRenderer), PostProcessEvent.AfterStack, "X-PostProcessing/Vignette/AuroraVignette")]
    public class AuroraVignette : PostProcessEffectSettings
    {
        [Range(0.0f, 1.0f)]
        public FloatParameter vignetteArea = new FloatParameter { value = 0.8f };

        [Range(0.0f, 1.0f)]
        public FloatParameter vignetteSmothness = new FloatParameter { value = 0.5f };

        [Range(0.0f, 1.0f)]
        public FloatParameter vignetteFading = new FloatParameter { value = 1f };

        [Range(0.1f, 1f)]
        public FloatParameter colorChange = new FloatParameter { value = 0.1f };


        [Range(0.0f, 2.0f)]
        public FloatParameter colorFactorR = new FloatParameter { value = 1.0f };

        [Range(0.0f, 2.0f)]
        public FloatParameter colorFactorG = new FloatParameter { value = 1.0f };

        [Range(0.0f, 2.0f)]
        public FloatParameter colorFactorB = new FloatParameter { value = 1.0f };

        [Range(-2.0f, 2.0f)]
        public FloatParameter flowSpeed = new FloatParameter { value = 1.0f };



    }

    public sealed class AuroraVignetteRenderer : PostProcessEffectRenderer<AuroraVignette>
    {
        private Shader shader;
        private float TimeX = 1.0f;
        private const string PROFILER_TAG = "X-AuroraVignette";

        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/AuroraVignette");
        }

        public override void Release()
        {
            base.Release();
        }

        static class ShaderIDs
        {
            internal static readonly int vignetteArea = Shader.PropertyToID("_VignetteArea");
            internal static readonly int vignetteSmothness = Shader.PropertyToID("_VignetteSmothness");
            internal static readonly int colorChange = Shader.PropertyToID("_ColorChange");
            internal static readonly int colorFactor = Shader.PropertyToID("_ColorFactor");
            internal static readonly int TimeX = Shader.PropertyToID("_TimeX");
            internal static readonly int vignetteFading = Shader.PropertyToID("_Fading");
        }

        public override void Render(PostProcessRenderContext context)
        {
            CommandBuffer cmd = context.command;
            PropertySheet sheet = context.propertySheets.Get(shader);
            cmd.BeginSample(PROFILER_TAG);

            TimeX += Time.deltaTime;
            if (TimeX > 100)
            {
                TimeX = 0;
            }

            sheet.properties.SetFloat(ShaderIDs.vignetteArea, settings.vignetteArea);
            sheet.properties.SetFloat(ShaderIDs.vignetteSmothness, settings.vignetteSmothness);
            sheet.properties.SetFloat(ShaderIDs.colorChange, settings.colorChange * 10f);
            sheet.properties.SetVector(ShaderIDs.colorFactor, new Vector3(settings.colorFactorR, settings.colorFactorG, settings.colorFactorB));
            sheet.properties.SetFloat(ShaderIDs.TimeX, TimeX * settings.flowSpeed);
            sheet.properties.SetFloat(ShaderIDs.vignetteFading, settings.vignetteFading);

            cmd.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
            cmd.EndSample(PROFILER_TAG);
        }
    }
}
        

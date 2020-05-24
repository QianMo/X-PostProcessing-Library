
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
    [PostProcess(typeof(GlitchAnalogNoiseRenderer), PostProcessEvent.AfterStack, "X-PostProcessing/Glitch/AnalogNoise")]
    public class GlitchAnalogNoise : PostProcessEffectSettings
    {

        [Range(0.0f, 1.0f)]
        public FloatParameter NoiseSpeed = new FloatParameter { value = 0.5f };

        [Range(0.0f, 1.0f)]
        public FloatParameter NoiseFading = new FloatParameter { value = 0.5f };

        [Range(0.0f, 1.0f)]
        public FloatParameter LuminanceJitterThreshold = new FloatParameter { value = 0.8f };

    }

    public sealed class GlitchAnalogNoiseRenderer : PostProcessEffectRenderer<GlitchAnalogNoise>
    {

        private const string PROFILER_TAG = "X-GlitchAnalogNoise";
        private Shader shader;
        private float TimeX = 1.0f;

        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/Glitch/AnalogNoise");
        }

        public override void Release()
        {
            base.Release();
        }

        static class ShaderIDs
        {
            internal static readonly int Params = Shader.PropertyToID("_Params");
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


            sheet.properties.SetVector(ShaderIDs.Params, new Vector4(settings.NoiseSpeed, settings.NoiseFading, settings.LuminanceJitterThreshold, TimeX));

            cmd.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
            cmd.EndSample(PROFILER_TAG);
        }
    }
}
        

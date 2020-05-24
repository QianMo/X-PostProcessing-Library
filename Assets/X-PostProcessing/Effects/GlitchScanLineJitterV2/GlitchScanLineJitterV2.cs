
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
    [PostProcess(typeof(GlitchScanLineJitterV2Renderer), PostProcessEvent.AfterStack, "X-PostProcessing/Glitch/ScanLineJitterV2")]
    public class GlitchScanLineJitterV2 : PostProcessEffectSettings
    {
        public DirectionParameter JitterDirection = new DirectionParameter { value = Direction.Horizontal };

        public IntervalTypeParameter intervalType = new IntervalTypeParameter { value = IntervalType.Random };

        [Range(0f, 25f)]
        public FloatParameter frequency = new FloatParameter { value = 1f };

        [Range(0f, 200f)]
        public FloatParameter amount = new FloatParameter { value = 50f };

        [Range(0f, 15f)]
        public FloatParameter speed = new FloatParameter { value = 1f };
    }

    public sealed class GlitchScanLineJitterV2Renderer : PostProcessEffectRenderer<GlitchScanLineJitterV2>
    {

        private const string PROFILER_TAG = "X-GlitchScanLineJitterV2";
        private Shader shader;

        private float randomFrequency;

        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/Glitch/ScanLineJitterV2");
        }

        public override void Release()
        {
            base.Release();
        }

        static class ShaderIDs
        {

            internal static readonly int Params = Shader.PropertyToID("_Params");
            internal static readonly int Params2 = Shader.PropertyToID("_Params2");
}

        public override void Render(PostProcessRenderContext context)
        {

            CommandBuffer cmd = context.command;
            PropertySheet sheet = context.propertySheets.Get(shader);
            cmd.BeginSample(PROFILER_TAG);

            UpdateFrequency(sheet);

            sheet.properties.SetVector(ShaderIDs.Params, new Vector3(settings.amount, settings.speed, settings.intervalType.value == IntervalType.Random ? randomFrequency : settings.frequency));

            cmd.BlitFullscreenTriangle(context.source, context.destination, sheet, (int)settings.JitterDirection.value);
            cmd.EndSample(PROFILER_TAG);
        }

        void UpdateFrequency(PropertySheet sheet)
        {
            if (settings.intervalType.value == IntervalType.Random)
            {
                randomFrequency = UnityEngine.Random.Range(0, settings.frequency);
            }

            if (settings.intervalType.value == IntervalType.Infinite)
            {
                sheet.EnableKeyword("USING_FREQUENCY_INFINITE");
            }
            else
            {
                sheet.DisableKeyword("USING_FREQUENCY_INFINITE");
            }
        }
    }
}
        

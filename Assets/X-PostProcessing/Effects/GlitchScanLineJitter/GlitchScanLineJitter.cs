
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
    public sealed class DirectionParameter : ParameterOverride<Direction> { }

    [Serializable]
    [PostProcess(typeof(GlitchScanLineJitterRenderer), PostProcessEvent.AfterStack, "X-PostProcessing/Glitch/ScanLineJitter")]
    public class GlitchScanLineJitter : PostProcessEffectSettings
    {

        public DirectionParameter JitterDirection  = new DirectionParameter { value = Direction.Horizontal };

        [Range(0.0f, 1.0f)]
        public FloatParameter JitterIndensity = new FloatParameter { value = 0.1f };

    }

    public sealed class GlitchScanLineJitterRenderer : PostProcessEffectRenderer<GlitchScanLineJitter>
    {
        private const string PROFILER_TAG = "X-GlitchScanLineJitter";
        private Shader shader;

        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/Glitch/ScanLineJitter");
        }

        public override void Release()
        {
            base.Release();
        }

        static class ShaderIDs
        {
            internal static readonly int JitterIndensity = Shader.PropertyToID("_ScanLineJitter");
        }

        public override void Render(PostProcessRenderContext context)
        {
            CommandBuffer cmd = context.command;
            PropertySheet sheet = context.propertySheets.Get(shader);
            cmd.BeginSample(PROFILER_TAG);

            float displacement = 0.005f + Mathf.Pow(settings.JitterIndensity, 3) * 0.1f;
            float threshold = Mathf.Clamp01(1.0f - settings.JitterIndensity * 1.2f);

            sheet.properties.SetVector(ShaderIDs.JitterIndensity, new Vector2(displacement, threshold));

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, (int)settings.JitterDirection.value);
            cmd.EndSample(PROFILER_TAG);
        }
    }
}
        

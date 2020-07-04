
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
    [PostProcess(typeof(SharpenV1Renderer), PostProcessEvent.AfterStack, "X-PostProcessing/ImageProcessing/SharpenV1")]
    public class SharpenV1 : PostProcessEffectSettings
    {

        [Range(0.0f, 5.0f)]
        public FloatParameter Strength = new FloatParameter { value = 0.5f };

        [Range(0.0f, 1.0f)]
        public FloatParameter Threshold = new FloatParameter { value = 0.1f };
    }

    public sealed class SharpenV1Renderer : PostProcessEffectRenderer<SharpenV1>
    {
        private const string PROFILER_TAG = "X-SharpenV1";
        private Shader shader;

        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/SharpenV1");
        }

        public override void Release()
        {
            base.Release();
        }

        static class ShaderIDs
        {
            internal static readonly int Strength = Shader.PropertyToID("_Strength");
            internal static readonly int Threshold = Shader.PropertyToID("_Threshold");
        }

        public override void Render(PostProcessRenderContext context)
        {
            CommandBuffer cmd = context.command;
            PropertySheet sheet = context.propertySheets.Get(shader);
            cmd.BeginSample(PROFILER_TAG);

            sheet.properties.SetFloat(ShaderIDs.Strength, settings.Strength);
            sheet.properties.SetFloat(ShaderIDs.Threshold, settings.Threshold);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
            cmd.EndSample(PROFILER_TAG);
        }
    }
}
        


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
    [PostProcess(typeof(SharpenV3Renderer), PostProcessEvent.AfterStack, "X-PostProcessing/ImageProcessing/SharpenV3")]
    public class SharpenV3 : PostProcessEffectSettings
    {

        [Range(0.0f, 1.0f)]
        public FloatParameter Sharpness = new FloatParameter { value = 0.5f };
    }

    public sealed class SharpenV3Renderer : PostProcessEffectRenderer<SharpenV3>
    {
        private const string PROFILER_TAG = "X-SharpenV3";
        private Shader shader;

        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/SharpenV3");
        }

        public override void Release()
        {
            base.Release();
        }

        static class ShaderIDs
        {
            internal static readonly int CentralFactor = Shader.PropertyToID("_CentralFactor");
            internal static readonly int SideFactor = Shader.PropertyToID("_SideFactor");

        }

        public override void Render(PostProcessRenderContext context)
        {
            CommandBuffer cmd = context.command;
            PropertySheet sheet = context.propertySheets.Get(shader);
            cmd.BeginSample(PROFILER_TAG);

            sheet.properties.SetFloat(ShaderIDs.CentralFactor, 1.0f + (3.2f * settings.Sharpness));
            sheet.properties.SetFloat(ShaderIDs.SideFactor, 0.8f * settings.Sharpness);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
            cmd.EndSample(PROFILER_TAG);
        }
    }
}
        

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
    [PostProcess(typeof(GrainyBlurRenderer), PostProcessEvent.AfterStack, "X-PostProcessing/Blur/GrainyBlur")]
    public class GrainyBlur : PostProcessEffectSettings
    {

        [Range(0.0f, 50.0f)]
        public FloatParameter BlurRadius = new FloatParameter { value = 5.0f };

        [Range(1, 8)]
        public IntParameter Iteration = new IntParameter { value = 4 };

        [Range(1, 10)]
        public FloatParameter RTDownScaling = new FloatParameter { value = 1 };
    }

    public sealed class GrainyBlurRenderer : PostProcessEffectRenderer<GrainyBlur>
    {

        private const string PROFILER_TAG = "X-GrainyBlur";
        private Shader shader;

        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/GrainyBlur");
        }

        public override void Release()
        {
            base.Release();
        }

        static class ShaderIDs
        {
            internal static readonly int Params = Shader.PropertyToID("_Params");
            internal static readonly int BufferRT = Shader.PropertyToID("_BufferRT");
        }

        public override void Render(PostProcessRenderContext context)
        {

            CommandBuffer cmd = context.command;
            PropertySheet sheet = context.propertySheets.Get(shader);

            cmd.BeginSample(PROFILER_TAG);

            if (settings.RTDownScaling > 1)
            {
                int RTWidth = (int)(context.screenWidth / settings.RTDownScaling);
                int RTHeight = (int)(context.screenHeight / settings.RTDownScaling);
                cmd.GetTemporaryRT(ShaderIDs.BufferRT, RTWidth, RTHeight, 0, FilterMode.Bilinear);
                // downsample screen copy into smaller RT
                context.command.BlitFullscreenTriangle(context.source, ShaderIDs.BufferRT);
            }

            sheet.properties.SetVector(ShaderIDs.Params, new Vector2(settings.BlurRadius / context.height, settings.Iteration));

            if (settings.RTDownScaling > 1)
            {
                cmd.BlitFullscreenTriangle(ShaderIDs.BufferRT, context.destination, sheet, 0);
            }
            else
            {
                cmd.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
            }

            cmd.ReleaseTemporaryRT(ShaderIDs.BufferRT);
            cmd.EndSample(PROFILER_TAG);
        }

    }
}
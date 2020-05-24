
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
    [PostProcess(typeof(GaussianBlurRenderer), PostProcessEvent.AfterStack, "X-PostProcessing/Blur/GaussianBlur")]
    public class GaussianBlur : PostProcessEffectSettings
    {

        [Range(0f, 5f)]
        public FloatParameter BlurRadius = new FloatParameter { value = 3f };

        [Range(1, 15)]
        public IntParameter Iteration = new IntParameter { value = 6 };

        [Range(1, 8)]
        public FloatParameter RTDownScaling = new FloatParameter { value = 2f };
    }

    public sealed class GaussianBlurRenderer : PostProcessEffectRenderer<GaussianBlur>
    {
        private Shader shader;
        private const string PROFILER_TAG = "X-GaussianBlur";


        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/GaussianBlur");
        }

        public override void Release()
        {
            base.Release();
        }

        static class ShaderIDs
        {

            internal static readonly int BlurRadius = Shader.PropertyToID("_BlurOffset");
            internal static readonly int BufferRT1 = Shader.PropertyToID("_BufferRT1");
            internal static readonly int BufferRT2 = Shader.PropertyToID("_BufferRT2");
        }

        public override void Render(PostProcessRenderContext context)
        {

            CommandBuffer cmd = context.command;
            PropertySheet sheet = context.propertySheets.Get(shader);

            cmd.BeginSample(PROFILER_TAG);

            int RTWidth = (int)(context.screenWidth / settings.RTDownScaling);
            int RTHeight = (int)(context.screenHeight / settings.RTDownScaling);
            cmd.GetTemporaryRT(ShaderIDs.BufferRT1, RTWidth, RTHeight, 0, FilterMode.Bilinear);
            cmd.GetTemporaryRT(ShaderIDs.BufferRT2, RTWidth, RTHeight, 0, FilterMode.Bilinear);

            // downsample screen copy into smaller RT
            context.command.BlitFullscreenTriangle(context.source, ShaderIDs.BufferRT1);


            for (int i = 0; i < settings.Iteration; i++)
            {
                // horizontal blur
                sheet.properties.SetVector(ShaderIDs.BlurRadius, new Vector4(settings.BlurRadius / context.screenWidth, 0, 0, 0));
                context.command.BlitFullscreenTriangle(ShaderIDs.BufferRT1, ShaderIDs.BufferRT2, sheet, 0);

                // vertical blur
                sheet.properties.SetVector(ShaderIDs.BlurRadius, new Vector4(0, settings.BlurRadius / context.screenHeight, 0, 0));
                context.command.BlitFullscreenTriangle(ShaderIDs.BufferRT2, ShaderIDs.BufferRT1, sheet, 0);
            }

            // Render blurred texture in blend pass
            cmd.BlitFullscreenTriangle(ShaderIDs.BufferRT1, context.destination, sheet, 1);

            // release
            cmd.ReleaseTemporaryRT(ShaderIDs.BufferRT1);
            cmd.ReleaseTemporaryRT(ShaderIDs.BufferRT2);

            cmd.EndSample(PROFILER_TAG);
        }
    }
}
        

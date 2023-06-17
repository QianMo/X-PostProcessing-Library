
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
    [PostProcess(typeof(DualGaussianBlurRenderer), PostProcessEvent.AfterStack, "X-PostProcessing/Blur/DualGaussianBlur")]
    public class DualGaussianBlur : PostProcessEffectSettings
    {

        [Range(0.0f, 15.0f)]
        public FloatParameter BlurRadius = new FloatParameter { value = 5.0f };

        [Range(1.0f, 8.0f)]
        public IntParameter Iteration = new IntParameter { value = 4 };

        [Range(1, 10)]
        public FloatParameter RTDownScaling = new FloatParameter { value = 2 };
    }

    public sealed class DualGaussianBlurRenderer : PostProcessEffectRenderer<DualGaussianBlur>
    {

        private const string PROFILER_TAG = "X-DualGaussianBlur";
        private Shader shader;

        // [down,up]
        Level[] m_Pyramid;
        const int k_MaxPyramidSize = 16;

        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/DualGaussianBlur");

            m_Pyramid = new Level[k_MaxPyramidSize];

            for (int i = 0; i < k_MaxPyramidSize; i++)
            {
                m_Pyramid[i] = new Level
                {
                    down_vertical = Shader.PropertyToID("_BlurMipDownV" + i),
                    down_horizontal = Shader.PropertyToID("_BlurMipDownH" + i),
                    up_vertical = Shader.PropertyToID("_BlurMipUpV" + i),
                    up_horizontal = Shader.PropertyToID("_BlurMipUpH" + i),

                };
            }
        }

        public override void Release()
        {
            base.Release();
        }

        static class ShaderIDs
        {
            internal static readonly int BlurOffset = Shader.PropertyToID("_BlurOffset");
            internal static readonly int BufferRT1 = Shader.PropertyToID("_BufferRT1");
            internal static readonly int BufferRT2 = Shader.PropertyToID("_BufferRT2");
        }


        struct Level
        {
            internal int down_vertical;
            internal int down_horizontal;
            internal int up_horizontal;
            internal int up_vertical;
        }


        public override void Render(PostProcessRenderContext context)
        {

            CommandBuffer cmd = context.command;
            PropertySheet sheet = context.propertySheets.Get(shader);
            cmd.BeginSample(PROFILER_TAG);


            int tw = (int)(context.screenWidth / settings.RTDownScaling);
            int th = (int)(context.screenHeight / settings.RTDownScaling);

            Vector4 BlurOffset = new Vector4(settings.BlurRadius / (float)context.screenWidth, settings.BlurRadius / (float)context.screenHeight, 0, 0);
            sheet.properties.SetVector(ShaderIDs.BlurOffset, BlurOffset);
            // Downsample
            RenderTargetIdentifier lastDown = context.source;
            for (int i = 0; i < settings.Iteration; i++)
            {
                int mipDownV = m_Pyramid[i].down_vertical;
                int mipDowH = m_Pyramid[i].down_horizontal;
                int mipUpV = m_Pyramid[i].up_vertical;
                int mipUpH = m_Pyramid[i].up_horizontal;

                context.GetScreenSpaceTemporaryRT(cmd, mipDownV, 0, context.sourceFormat, RenderTextureReadWrite.Default, FilterMode.Bilinear, tw, th);
                context.GetScreenSpaceTemporaryRT(cmd, mipDowH, 0, context.sourceFormat, RenderTextureReadWrite.Default, FilterMode.Bilinear, tw, th);
                context.GetScreenSpaceTemporaryRT(cmd, mipUpV, 0, context.sourceFormat, RenderTextureReadWrite.Default, FilterMode.Bilinear, tw, th);
                context.GetScreenSpaceTemporaryRT(cmd, mipUpH, 0, context.sourceFormat, RenderTextureReadWrite.Default, FilterMode.Bilinear, tw, th);

                // horizontal blur
                sheet.properties.SetVector(ShaderIDs.BlurOffset, new Vector4(settings.BlurRadius / context.screenWidth, 0, 0, 0));
                context.command.BlitFullscreenTriangle(lastDown, mipDowH, sheet, 0);

                // vertical blur
                sheet.properties.SetVector(ShaderIDs.BlurOffset, new Vector4(0, settings.BlurRadius / context.screenHeight, 0, 0));
                context.command.BlitFullscreenTriangle(mipDowH, mipDownV, sheet, 0);

                lastDown = mipDownV;
                tw = Mathf.Max(tw / 2, 1);
                th = Mathf.Max(th / 2, 1);
            }

            // Upsample
            int lastUp = m_Pyramid[settings.Iteration - 1].down_vertical;
            for (int i = settings.Iteration - 2; i >= 0; i--)
            {

                int mipUpV = m_Pyramid[i].up_vertical;
                int mipUpH = m_Pyramid[i].up_horizontal;

                // horizontal blur
                sheet.properties.SetVector(ShaderIDs.BlurOffset, new Vector4(settings.BlurRadius / context.screenWidth, 0, 0, 0));
                context.command.BlitFullscreenTriangle(lastUp, mipUpH, sheet, 0);

                // vertical blur
                sheet.properties.SetVector(ShaderIDs.BlurOffset, new Vector4(0, settings.BlurRadius / context.screenHeight, 0, 0));
                context.command.BlitFullscreenTriangle(mipUpH, mipUpV, sheet, 0);

                lastUp = mipUpV;
            }


            // Render blurred texture in blend pass
            cmd.BlitFullscreenTriangle(lastUp, context.destination, sheet, 1);

            // Cleanup
            for (int i = 0; i < settings.Iteration; i++)
            {
                if (m_Pyramid[i].down_vertical != lastUp)
                    cmd.ReleaseTemporaryRT(m_Pyramid[i].down_vertical);
                if (m_Pyramid[i].down_horizontal != lastUp)
                    cmd.ReleaseTemporaryRT(m_Pyramid[i].down_horizontal);
                if (m_Pyramid[i].up_horizontal != lastUp)
                    cmd.ReleaseTemporaryRT(m_Pyramid[i].up_horizontal);
                if (m_Pyramid[i].up_vertical != lastUp)
                    cmd.ReleaseTemporaryRT(m_Pyramid[i].up_vertical);
            }

            cmd.EndSample(PROFILER_TAG);
        }
    }
}


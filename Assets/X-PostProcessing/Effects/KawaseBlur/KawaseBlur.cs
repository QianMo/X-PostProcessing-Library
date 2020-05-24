
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
    [PostProcess(typeof(KawaseBlurRenderer), PostProcessEvent.AfterStack, "X-PostProcessing/Blur/KawaseBlur")]
    public class KawaseBlur : PostProcessEffectSettings
    {

        [Range(0.0f, 5.0f)]
        public FloatParameter BlurRadius = new FloatParameter { value = 0.5f };

        [Range(1, 10)]
        public IntParameter Iteration = new IntParameter { value = 6 };

        [Range(1, 8)]
        public FloatParameter RTDownScaling = new FloatParameter { value = 2 };

    }

    public sealed class KawaseBlurRenderer : PostProcessEffectRenderer<KawaseBlur>
    {

        private const string PROFILER_TAG = "X-KawaseBlur";
        private Shader shader;


        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/KawaseBlur");
        }

        public override void Release()
        {
            base.Release();
        }

        static class ShaderIDs
        {
            internal static readonly int BlurRadius = Shader.PropertyToID("_Offset");

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


            bool needSwitch = true;
            for (int i = 0; i < settings.Iteration; i++)
            {
                sheet.properties.SetFloat(ShaderIDs.BlurRadius, i / settings.RTDownScaling + settings.BlurRadius);
                context.command.BlitFullscreenTriangle(needSwitch ? ShaderIDs.BufferRT1 : ShaderIDs.BufferRT2, needSwitch ? ShaderIDs.BufferRT2 : ShaderIDs.BufferRT1, sheet, 0);
                needSwitch = !needSwitch;
            }


            sheet.properties.SetFloat(ShaderIDs.BlurRadius, settings.Iteration / settings.RTDownScaling + settings.BlurRadius);
            cmd.BlitFullscreenTriangle(needSwitch ? ShaderIDs.BufferRT1 : ShaderIDs.BufferRT2, context.destination, sheet, 0);

            // release
            cmd.ReleaseTemporaryRT(ShaderIDs.BufferRT1);
            cmd.ReleaseTemporaryRT(ShaderIDs.BufferRT2);
            cmd.EndSample(PROFILER_TAG);
        }
    }
}


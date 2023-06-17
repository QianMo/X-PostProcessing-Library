
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
    [PostProcess(typeof(DirectionalBlurRenderer), PostProcessEvent.AfterStack, "X-PostProcessing/Blur/DirectionalBlur")]
    public class DirectionalBlur : PostProcessEffectSettings
    {

        [Range(0.0f, 5.0f)]
        public FloatParameter BlurRadius = new FloatParameter { value = 1f };

        [Range(1, 30)]
        public IntParameter Iteration = new IntParameter { value = 12 };

        [Range(0.0f, 6.0f)]
        public FloatParameter Angle = new FloatParameter { value = 0.5f };

        [Range(1.0f, 10.0f)]
        public FloatParameter RTDownScaling = new FloatParameter { value = 1.0f };

    }

    public sealed class DirectionalBlurRenderer : PostProcessEffectRenderer<DirectionalBlur>
    {

        private const string PROFILER_TAG = "X-DirectionalBlur";
        private Shader shader;


        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/DirectionalBlur");
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

            float sinVal = (Mathf.Sin(settings.Angle) * settings.BlurRadius * 0.05f) / settings.Iteration;
            float cosVal = (Mathf.Cos(settings.Angle) * settings.BlurRadius * 0.05f) / settings.Iteration;        
            sheet.properties.SetVector(ShaderIDs.Params, new Vector3(settings.Iteration, sinVal, cosVal));

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
        

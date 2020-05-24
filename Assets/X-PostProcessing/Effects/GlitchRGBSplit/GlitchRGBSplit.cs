
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
    public sealed class GlitchRGBSplitDirectionParameter : ParameterOverride<DirectionEX> { }

    [Serializable]
    [PostProcess(typeof(GlitchRGBSplitRenderer), PostProcessEvent.AfterStack, "X-PostProcessing/Glitch/RGBSplit")]
    public class GlitchRGBSplit : PostProcessEffectSettings
    {
        public GlitchRGBSplitDirectionParameter SplitDirection = new GlitchRGBSplitDirectionParameter { value = DirectionEX.Horizontal };

        [Range(0.0f, 1.0f)]
        public FloatParameter Fading = new FloatParameter { value = 1f };

        [Range(0.0f, 5.0f)]
        public FloatParameter Amount = new FloatParameter { value = 1f };

        [Range(0.0f, 10.0f)]
        public FloatParameter Speed = new FloatParameter { value = 1f };

        [Range(0.0f, 1.0f)]
        public FloatParameter CenterFading = new FloatParameter { value = 1f };

        [Range(0.0f, 5.0f)]
        public FloatParameter AmountR = new FloatParameter { value = 1f };

        [Range(0.0f, 5.0f)]
        public FloatParameter AmountB = new FloatParameter { value = 1f };

    }

    public sealed class GlitchRGBSplitRenderer : PostProcessEffectRenderer<GlitchRGBSplit>
    {

        private const string PROFILER_TAG = "X-GlitchRGBSplit";
        private Shader shader;
        private float TimeX = 1.0f;


        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/Glitch/RGBSplit");
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

            TimeX += Time.deltaTime;
            if (TimeX > 100)
            {
                TimeX = 0;
            }


            sheet.properties.SetVector(ShaderIDs.Params, new Vector4(settings.Fading, settings.Amount, settings.Speed, settings.CenterFading));
            sheet.properties.SetVector(ShaderIDs.Params2, new Vector3(TimeX, settings.AmountR, settings.AmountB));


            cmd.BlitFullscreenTriangle(context.source, context.destination, sheet, (int)settings.SplitDirection.value);
            cmd.EndSample(PROFILER_TAG);
        }
    }
}
        

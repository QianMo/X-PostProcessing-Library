
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
    [PostProcess(typeof(GlitchRGBSplitV4Renderer), PostProcessEvent.AfterStack, "X-PostProcessing/Glitch/RGBSplitV4")]
    public class GlitchRGBSplitV4 : PostProcessEffectSettings
    {

        public GlitchRGBSplitDirectionParameter splitDirection = new GlitchRGBSplitDirectionParameter { value = DirectionEX.Horizontal };

        [Range(-1.0f, 1.0f)]
        public FloatParameter indensity = new FloatParameter { value = 0.5f };

        [Range(0.0f, 100.0f)]
        public FloatParameter speed = new FloatParameter { value = 10.0f };

    }

    public sealed class GlitchRGBSplitV4Renderer : PostProcessEffectRenderer<GlitchRGBSplitV4>
    {
        private const string PROFILER_TAG = "X-GlitchRGBSplitV4";
        private Shader shader;
        private float randomFrequency;
        private float TimeX = 1.0f;

        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/Glitch/RGBSplitV4");
        }

        public override void Release()
        {
            base.Release();
        }

        static class ShaderIDs
        {
            internal static readonly int Params = Shader.PropertyToID("_Params");
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


            sheet.properties.SetVector(ShaderIDs.Params, new Vector2(settings.indensity * 0.1f, Mathf.Floor(TimeX * settings.speed)));


            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, (int)settings.splitDirection.value);
            cmd.EndSample(PROFILER_TAG);
        }

    }
}
        

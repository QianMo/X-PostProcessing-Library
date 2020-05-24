
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
    [PostProcess(typeof(GlitchRGBSplitV2Renderer), PostProcessEvent.AfterStack, "X-PostProcessing/Glitch/RGBSplitV2")]
    public class GlitchRGBSplitV2 : PostProcessEffectSettings
    {
        public GlitchRGBSplitDirectionParameter SplitDirection = new GlitchRGBSplitDirectionParameter { value = DirectionEX.Horizontal };

        [Range(0.0f, 1.0f)]
        public FloatParameter Amount = new FloatParameter { value = 0.5f };

        [Range(1.0f, 6.0f)]
        public FloatParameter Amplitude = new FloatParameter { value = 3.0f };

        [Range(0.0f, 2.0f)]
        public FloatParameter Speed = new FloatParameter { value = 1f };

    }

    public sealed class GlitchRGBSplitV2Renderer : PostProcessEffectRenderer<GlitchRGBSplitV2>
    {

        private const string PROFILER_TAG = "X-GlitchRGBSplitV2";
        private Shader shader;
        private float TimeX = 1.0f;

        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/Glitch/RGBSplitV2");
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

            sheet.properties.SetVector(ShaderIDs.Params, new Vector3(TimeX * settings.Speed, settings.Amount, settings.Amplitude ));


            cmd.BlitFullscreenTriangle(context.source, context.destination, sheet, (int)settings.SplitDirection.value);
            cmd.EndSample(PROFILER_TAG);
        }
    }
}
        

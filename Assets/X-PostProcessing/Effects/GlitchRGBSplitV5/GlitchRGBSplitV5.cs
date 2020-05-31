
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
    [PostProcess(typeof(GlitchRGBSplitV5Renderer), PostProcessEvent.AfterStack, "X-PostProcessing/Glitch/RGBSplitV5")]
    public class GlitchRGBSplitV5 : PostProcessEffectSettings
    {

        [Range(0.0f, 5.0f)]
        public FloatParameter Amplitude = new FloatParameter { value = 3f };

        [Range(0.0f, 1.0f)]
        public FloatParameter Speed = new FloatParameter { value = 0.1f };

    }

    public sealed class GlitchRGBSplitV5Renderer : PostProcessEffectRenderer<GlitchRGBSplitV5>
    {

        private const string PROFILER_TAG = "X-GlitchRGBSplitV5";
        private Shader shader;
        private Texture2D NoiseTex;


        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/Glitch/RGBSplitV5");
            NoiseTex = Resources.Load("X-Noise256") as Texture2D;
        }

        public override void Release()
        {
            base.Release();
        }

        static class ShaderIDs
        {
            internal static readonly int NoiseTex = Shader.PropertyToID("_NoiseTex");
            internal static readonly int Params = Shader.PropertyToID("_Params");
        }

        public override void Render(PostProcessRenderContext context)
        {

            CommandBuffer cmd = context.command;
            PropertySheet sheet = context.propertySheets.Get(shader);
            cmd.BeginSample(PROFILER_TAG);


            sheet.properties.SetVector(ShaderIDs.Params, new Vector2(settings.Amplitude, settings.Speed));
            if (NoiseTex != null)
            {
                sheet.properties.SetTexture(ShaderIDs.NoiseTex, NoiseTex);
            }

            cmd.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
            cmd.EndSample(PROFILER_TAG);
        }
    }
}
        

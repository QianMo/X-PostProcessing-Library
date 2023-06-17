
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
    [PostProcess(typeof(GlitchImageBlockV3Renderer), PostProcessEvent.AfterStack, "X-PostProcessing/Glitch/ImageBlockV3")]
    public class GlitchImageBlockV3 : PostProcessEffectSettings
    {

        [Range(0.0f, 50.0f)]
        public FloatParameter Speed = new FloatParameter { value = 10f };

        [Range(0.0f, 50.0f)]
        public FloatParameter BlockSize = new FloatParameter { value = 8f };

    }

    public sealed class GlitchImageBlockV3Renderer : PostProcessEffectRenderer<GlitchImageBlockV3>
    {

        private const string PROFILER_TAG = "X-GlitchImageBlockV3";
        private Shader shader;

        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/Glitch/ImageBlockV3");
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

            sheet.properties.SetVector(ShaderIDs.Params, new Vector2(settings.Speed, settings.BlockSize));

            cmd.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
            cmd.EndSample(PROFILER_TAG);
        }
    }
}
        

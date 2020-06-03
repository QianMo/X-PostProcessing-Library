
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
    [PostProcess(typeof(GlitchImageBlockV4Renderer), PostProcessEvent.AfterStack, "X-PostProcessing/Glitch/ImageBlockV4")]
    public class GlitchImageBlockV4 : PostProcessEffectSettings
    {
        [Range(0f, 50f)]
        public FloatParameter Speed = new FloatParameter { value = 10f };

        [Range(0f, 50f)]
        public FloatParameter BlockSize = new FloatParameter { value = 8f };

        [Range(0f, 25f)]
        public FloatParameter MaxRGBSplitX = new FloatParameter { value = 1f };

        [Range(0f, 25f)]
        public FloatParameter MaxRGBSplitY = new FloatParameter { value = 1f };
    }

    public sealed class GlitchImageBlockV4Renderer : PostProcessEffectRenderer<GlitchImageBlockV4>
    {

        private const string PROFILER_TAG = "X-GlitchImageBlockV4";
        private Shader shader;


        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/Glitch/ImageBlockV4");
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

            sheet.properties.SetVector(ShaderIDs.Params, new Vector4(settings.Speed, settings.BlockSize, settings.MaxRGBSplitX, settings.MaxRGBSplitY));

            cmd.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
            cmd.EndSample(PROFILER_TAG);
        }
    }
}
        

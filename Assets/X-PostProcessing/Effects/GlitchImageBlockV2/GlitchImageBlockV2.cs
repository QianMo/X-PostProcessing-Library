
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
    [PostProcess(typeof(GlitchImageBlockV2Renderer), PostProcessEvent.AfterStack, "X-PostProcessing/Glitch/ImageBlockV2")]
    public class GlitchImageBlockV2 : PostProcessEffectSettings
    {

        [Range(0.0f, 1.0f)]
        public FloatParameter Fade = new FloatParameter { value = 1f };

        [Range(0.0f, 1.0f)]
        public FloatParameter Speed = new FloatParameter { value = 0.5f };

        [Range(0.0f, 10.0f)]
        public FloatParameter Amount = new FloatParameter { value = 1f };

        [Range(0.0f, 50.0f)]
        public FloatParameter BlockLayer1_U = new FloatParameter { value = 2f };

        [Range(0.0f, 50.0f)]
        public FloatParameter BlockLayer1_V = new FloatParameter { value = 16f };


        [Range(0.0f, 50.0f)]
        public FloatParameter BlockLayer1_Indensity = new FloatParameter { value = 8f };

        [Range(0.0f, 50.0f)]
        public FloatParameter RGBSplitIndensity = new FloatParameter { value = 2f };

        public BoolParameter BlockVisualizeDebug = new BoolParameter { value = false };
    }

    public sealed class GlitchImageBlockV2Renderer : PostProcessEffectRenderer<GlitchImageBlockV2>
    {

        private const string PROFILER_TAG = "X-GlitchImageBlockV2";
        private Shader shader;
        private float TimeX = 1.0f;

        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/Glitch/ImageBlockV2");
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


            sheet.properties.SetVector(ShaderIDs.Params, new Vector3(TimeX * settings.Speed, settings.Amount, settings.Fade));
            sheet.properties.SetVector(ShaderIDs.Params2, new Vector4(settings.BlockLayer1_U, settings.BlockLayer1_V, settings.BlockLayer1_Indensity, settings.RGBSplitIndensity));

            sheet.properties.SetFloat("_TimeX", TimeX * settings.Speed);
            sheet.properties.SetFloat("_Offset", settings.Amount);
            sheet.properties.SetFloat("_Fade", settings.Fade);

            sheet.properties.SetFloat("_BlockLayer1_U", settings.BlockLayer1_U);
            sheet.properties.SetFloat("_BlockLayer1_V", settings.BlockLayer1_V);
            sheet.properties.SetFloat("_BlockLayer1_Indensity", settings.BlockLayer1_Indensity);
            sheet.properties.SetFloat("_RGBSplit_Indensity", settings.RGBSplitIndensity);


            if (settings.BlockVisualizeDebug)
            {
                //debug
                cmd.BlitFullscreenTriangle(context.source, context.destination, sheet, 1);
            }
            else
            {
                cmd.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
            }

            cmd.EndSample(PROFILER_TAG);
        }
    }
}
        

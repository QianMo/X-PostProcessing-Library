
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
    [PostProcess(typeof(EdgeDetectionSobelNeonV2Renderer), PostProcessEvent.AfterStack, "X-PostProcessing/EdgeDetection/EdgeDetectionSobelNeonV2")]
    public class EdgeDetectionSobelNeonV2 : PostProcessEffectSettings
    {
        [Range(0.05f, 5.0f)]
        public FloatParameter EdgeWidth = new FloatParameter { value = 1f };

        [Range(0.1f, 1.0f)]
        public FloatParameter EdgeNeonFade = new FloatParameter { value = 1f };

        [Range(0.0f, 1.0f)]
        public FloatParameter BackgroundFade = new FloatParameter { value = 0f };

        [Range(0.2f, 2.0f)]
        public FloatParameter Brigtness = new FloatParameter { value = 1f };

        [ColorUsageAttribute(true, true, 0f, 20f, 0.125f, 3f)]
        public ColorParameter BackgroundColor = new ColorParameter { value = new Color(0.0f, 0.0f, 0.0f, 1.0f) };
    }

    public sealed class EdgeDetectionSobelNeonV2Renderer : PostProcessEffectRenderer<EdgeDetectionSobelNeonV2>
    {

        private const string PROFILER_TAG = "X-EdgeDetectionSobelNeonV2";
        private Shader shader;


        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/EdgeDetectionSobelNeonV2");
        }

        public override void Release()
        {
            base.Release();
        }

        static class ShaderIDs
        {
            internal static readonly int Params = Shader.PropertyToID("_Params");
            internal static readonly int BackgroundColor = Shader.PropertyToID("_BackgroundColor");
        }

        public override void Render(PostProcessRenderContext context)
        {

            CommandBuffer cmd = context.command;
            PropertySheet sheet = context.propertySheets.Get(shader);
            cmd.BeginSample(PROFILER_TAG);

            sheet.properties.SetVector(ShaderIDs.Params, new Vector4(settings.EdgeWidth, settings.EdgeNeonFade, settings.Brigtness, settings.BackgroundFade));
            sheet.properties.SetColor(ShaderIDs.BackgroundColor, settings.BackgroundColor);

            cmd.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
            cmd.EndSample(PROFILER_TAG);
        }
    }
}
        

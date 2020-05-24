
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
    [PostProcess(typeof(EdgeDetectionSobelRenderer), PostProcessEvent.AfterStack, "X-PostProcessing/EdgeDetection/EdgeDetectionSobel")]
    public class EdgeDetectionSobel : PostProcessEffectSettings
    {

        [Range(0.05f, 5.0f)]
        public FloatParameter edgeWidth = new FloatParameter { value = 0.3f };

        [ColorUsageAttribute(true, true, 0f, 20f, 0.125f, 3f)]
        public ColorParameter edgeColor = new ColorParameter { value = new Color(0.0f, 0.0f, 0.0f, 1) };

        [Range(0.0f, 1.0f)]
        public FloatParameter backgroundFade = new FloatParameter { value = 1f };

        [ColorUsageAttribute(true, true, 0f, 20f, 0.125f, 3f)]
        public ColorParameter backgroundColor = new ColorParameter { value = new Color(1.0f, 1.0f, 1.0f, 1) };
    }

    public sealed class EdgeDetectionSobelRenderer : PostProcessEffectRenderer<EdgeDetectionSobel>
    {

        private const string PROFILER_TAG = "X-EdgeDetectionSobel";
        private Shader shader;


        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/EdgeDetectionSobel");
        }

        public override void Release()
        {
            base.Release();
        }

        static class ShaderIDs
        {
            internal static readonly int Params = Shader.PropertyToID("_Params");
            internal static readonly int EdgeColor = Shader.PropertyToID("_EdgeColor");
            internal static readonly int BackgroundColor = Shader.PropertyToID("_BackgroundColor");
        }

        public override void Render(PostProcessRenderContext context)
        {

            CommandBuffer cmd = context.command;
            PropertySheet sheet = context.propertySheets.Get(shader);
            cmd.BeginSample(PROFILER_TAG);

            sheet.properties.SetVector(ShaderIDs.Params, new Vector2(settings.edgeWidth, settings.backgroundFade));
            sheet.properties.SetColor(ShaderIDs.EdgeColor, settings.edgeColor);
            sheet.properties.SetColor(ShaderIDs.BackgroundColor, settings.backgroundColor);

            cmd.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
            cmd.EndSample(PROFILER_TAG);
        }
    }
}
        

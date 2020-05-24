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
    [PostProcess(typeof(PixelizeCircleRenderer), PostProcessEvent.BeforeStack, "X-PostProcessing/Pixelize/PixelizeCircle")]
    public class PixelizeCircle : PostProcessEffectSettings
    {

        [Range(0.01f, 1.0f)]
        public FloatParameter pixelSize = new FloatParameter { value = 0.8f };
        [Range(0.01f, 1.0f)]
        public FloatParameter circleRadius = new FloatParameter { value = 0.45f };
        [Range(0.2f, 5.0f), Tooltip("Pixel interval X")]
        public FloatParameter pixelIntervalX = new FloatParameter { value = 1f };
        [Range(0.2f, 5.0f), Tooltip("Pixel interval Y")]
        public FloatParameter pixelIntervalY = new FloatParameter { value = 1f };
        [ColorUsageAttribute(true, true, 0f, 20f, 0.125f, 3f)]
        public ColorParameter BackgroundColor = new ColorParameter { value = new Color(0.0f, 0.0f, 0.0f) };

    }

    public sealed class PixelizeCircleRenderer : PostProcessEffectRenderer<PixelizeCircle>
    {
        private const string PROFILER_TAG = "X-PixelizeCircle";
        private Shader shader;

        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/PixelizeCircle");
        }

        public override void Release()
        {
            base.Release();
        }

        static class ShaderIDs
        {
            internal static readonly int Params = Shader.PropertyToID("_Params");
            internal static readonly int Params2 = Shader.PropertyToID("_Params2");
            internal static readonly int BackgroundColor = Shader.PropertyToID("_BackgroundColor");
        }

        public override void Render(PostProcessRenderContext context)
        {
            CommandBuffer cmd = context.command;
            PropertySheet sheet = context.propertySheets.Get(shader);
            cmd.BeginSample(PROFILER_TAG);

            float size = (1.01f - settings.pixelSize) * 300f;
            Vector4 parameters = new Vector4(size, ((context.screenWidth * 2 / context.screenHeight) * size / Mathf.Sqrt(3f)), settings.circleRadius, 0f);

            sheet.properties.SetVector(ShaderIDs.Params, parameters);
            sheet.properties.SetVector(ShaderIDs.Params2, new Vector2(settings.pixelIntervalX, settings.pixelIntervalY));
            sheet.properties.SetColor(ShaderIDs.BackgroundColor, settings.BackgroundColor);

            cmd.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
            cmd.EndSample(PROFILER_TAG);
        }
    }
}
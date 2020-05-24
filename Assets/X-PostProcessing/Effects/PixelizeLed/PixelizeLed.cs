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
    [PostProcess(typeof(PixelizeLedV2Renderer), PostProcessEvent.BeforeStack, "X-PostProcessing/Pixelize/PixelizeLed")]
    public class PixelizeLed : PostProcessEffectSettings
    {

        [Range(0.01f, 1.0f)]
        public FloatParameter pixelSize = new FloatParameter { value = 0.5f };

        [Range(0.01f, 1.0f)]
        public FloatParameter ledRadius = new FloatParameter { value = 1.0f };

        [ColorUsageAttribute(true, true, 0f, 20f, 0.125f, 3f)]
        public ColorParameter BackgroundColor = new ColorParameter { value = new Color(0.0f, 0.0f, 0.0f) };

        public BoolParameter useAutoScreenRatio = new BoolParameter { value = true };

        [Range(0.2f, 5.0f)]
        public FloatParameter pixelRatio = new FloatParameter { value = 1f };

    }

    public sealed class PixelizeLedV2Renderer : PostProcessEffectRenderer<PixelizeLed>
    {
        private const string PROFILER_TAG = "X-PixelizeLed";
        private Shader shader;

        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/PixelizeLed");
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


            float size = (1.01f - settings.pixelSize) * 300f;

            float ratio = settings.pixelRatio;
            if (settings.useAutoScreenRatio)
            {
                ratio = (float)(context.width / (float)context.height);
                if (ratio == 0)
                {
                    ratio = 1f;
                }
            }


            sheet.properties.SetVector(ShaderIDs.Params, new Vector4(size, ratio, settings.ledRadius));
            sheet.properties.SetColor(ShaderIDs.BackgroundColor, settings.BackgroundColor);

            cmd.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
            cmd.EndSample(PROFILER_TAG);
        }
    }
}
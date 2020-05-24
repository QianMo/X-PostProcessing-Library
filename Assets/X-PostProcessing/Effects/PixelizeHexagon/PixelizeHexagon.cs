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
    [PostProcess(typeof(PixelizeHexagonRenderer), PostProcessEvent.BeforeStack, "X-PostProcessing/Pixelize/PixelizeHexagon")]
    public class PixelizeHexagon : PostProcessEffectSettings
    {

        [Range(0.01f, 1.0f)]
        public FloatParameter pixelSize = new FloatParameter { value = 0.05f };

        [Range(0.01f, 5.0f)]
        public FloatParameter gridWidth = new FloatParameter { value = 1.0f };
        

        public BoolParameter useAutoScreenRatio = new BoolParameter { value = false };

        [Range(0.2f, 5.0f)]
        public FloatParameter pixelRatio = new FloatParameter { value = 1f };

        [Range(0.2f, 5.0f), Tooltip("像素缩放X")]
        public FloatParameter pixelScaleX = new FloatParameter { value = 1f };

        [Range(0.2f, 5.0f), Tooltip("像素缩放Y")]
        public FloatParameter pixelScaleY = new FloatParameter { value = 1f };

    }

    public sealed class PixelizeHexagonRenderer : PostProcessEffectRenderer<PixelizeHexagon>
    {
        private const string PROFILER_TAG = "X-PixelizeHexagon";
        private Shader shader;

        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/PixelizeHexagon");
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

            float size = settings.pixelSize * 0.2f;
            sheet.properties.SetFloat("_PixelSize", size);
            float ratio = settings.pixelRatio;
            if (settings.useAutoScreenRatio)
            {
                ratio = (float)(context.width / (float)context.height);
                if (ratio == 0)
                {
                    ratio = 1f;
                }
            }

            sheet.properties.SetVector(ShaderIDs.Params, new Vector4(size, ratio, settings.pixelScaleX, settings.pixelScaleY));



            cmd.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
            cmd.EndSample(PROFILER_TAG);

        }


    }
}
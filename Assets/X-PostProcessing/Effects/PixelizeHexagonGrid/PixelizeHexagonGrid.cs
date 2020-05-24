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
    [PostProcess(typeof(PixelizeHexagonGridRenderer), PostProcessEvent.BeforeStack, "X-PostProcessing/Pixelize/PixelizeHexagonGrid")]
    public class PixelizeHexagonGrid : PostProcessEffectSettings
    {
        [Range(0.01f, 1.0f)]
        public FloatParameter pixelSize = new FloatParameter { value = 0.05f };

        [Range(0.01f, 5.0f)]
        public FloatParameter gridWidth = new FloatParameter { value = 1.0f };
        

        public BoolParameter useAutoScreenRatio = new BoolParameter { value = false };

        [Range(0.2f, 5.0f)]
        public FloatParameter pixelRatio = new FloatParameter { value = 1f };

        [Range(0.2f, 5.0f)]
        public FloatParameter pixelScaleX = new FloatParameter { value = 1f };

        [Range(0.2f, 5.0f)]
        public FloatParameter pixelScaleY = new FloatParameter { value = 1f };

    }

    public sealed class PixelizeHexagonGridRenderer : PostProcessEffectRenderer<PixelizeHexagonGrid>
    {
        private const string PROFILER_TAG = "X-PixelizeHexagonGrid";
        private Shader shader;

        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/PixelizeHexagonGrid");
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

            sheet.properties.SetVector(ShaderIDs.Params, new Vector2(settings.pixelSize, settings.gridWidth));


            cmd.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
            cmd.EndSample(PROFILER_TAG);
        }  

    }
}
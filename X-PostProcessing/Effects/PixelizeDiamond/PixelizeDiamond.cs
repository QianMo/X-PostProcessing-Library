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
    [PostProcess(typeof(PixelizeDiamondRenderer), PostProcessEvent.BeforeStack, "X-PostProcessing/Pixelize/PixelizeDiamond")]
    public class PixelizeDiamond : PostProcessEffectSettings
    {

        [Range(0.01f, 1.0f)]
        public FloatParameter pixelSize = new FloatParameter { value = 0.2f };

    }

    public sealed class PixelizeDiamondRenderer : PostProcessEffectRenderer<PixelizeDiamond>
    {
        private const string PROFILER_TAG = "X-PixelizeDiamond";
        private Shader shader;

        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/PixelizeDiamond");
        }

        public override void Release()
        {
            base.Release();
        }

        static class ShaderIDs
        {
            internal static readonly int PixelSize = Shader.PropertyToID("_PixelSize");
        }

        public override void Render(PostProcessRenderContext context)
        {
            CommandBuffer cmd = context.command;
            PropertySheet sheet = context.propertySheets.Get(shader);
            cmd.BeginSample(PROFILER_TAG);

            sheet.properties.SetFloat(ShaderIDs.PixelSize, settings.pixelSize);

            cmd.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
            cmd.EndSample(PROFILER_TAG);
        }
    }
}
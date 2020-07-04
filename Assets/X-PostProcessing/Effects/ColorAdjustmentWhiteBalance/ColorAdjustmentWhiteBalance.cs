
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
    [PostProcess(typeof(ColorAdjustmentWhiteBalanceRenderer), PostProcessEvent.AfterStack, "X-PostProcessing/ColorAdjustment/WhiteBalance")]
    public class ColorAdjustmentWhiteBalance : PostProcessEffectSettings
    {

        /// <summary>
        /// custom color temperature.
        /// </summary>
        [Range(-1f, 1f)]
        public FloatParameter temperature = new FloatParameter { value = 0f };

        /// <summary>
        /// for a green or magenta tint.
        /// </summary>
        [Range(-1f, 1f)]
        public FloatParameter tint = new FloatParameter { value = 0f };

    }

    public sealed class ColorAdjustmentWhiteBalanceRenderer : PostProcessEffectRenderer<ColorAdjustmentWhiteBalance>
    {
        private Shader shader;
        private const string PROFILER_TAG = "X-ColorAdjustmentWhiteBalance";

        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/ColorAdjustment/WhiteBalance");
        }

        public override void Release()
        {
            base.Release();
        }

        public override void Render(PostProcessRenderContext context)
        {
            CommandBuffer cmd = context.command;
            PropertySheet sheet = context.propertySheets.Get(shader);
            cmd.BeginSample(PROFILER_TAG);

            sheet.properties.SetFloat("_Temperature", settings.temperature);
            sheet.properties.SetFloat("_Tint", settings.tint);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
            cmd.EndSample(PROFILER_TAG);
        }
    }
}
        

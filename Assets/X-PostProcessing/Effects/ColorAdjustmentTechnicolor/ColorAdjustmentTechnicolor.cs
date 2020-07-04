
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
using UnityEngine.Rendering.PostProcessing;


namespace XPostProcessing
{

    [Serializable]
    [PostProcess(typeof(ColorAdjustmentTechnicolorRenderer), PostProcessEvent.AfterStack, "X-PostProcessing/ColorAdjustment/Technicolor")]
    public class ColorAdjustmentTechnicolor : PostProcessEffectSettings
    {

        [Range(0.0f, 8.0f)]
        public FloatParameter exposure = new FloatParameter { value = 4.0f };

        [Range(0.0f, 1.0f)]
        public FloatParameter colorBalanceR = new FloatParameter { value = 0.2f };

        [Range(0.0f, 1.0f)]
        public FloatParameter colorBalanceG = new FloatParameter { value = 0.2f };

        [Range(0.0f, 1.0f)]
        public FloatParameter colorBalanceB = new FloatParameter { value = 0.2f };

        [Range(0.0f, 1.0f)]
        public FloatParameter indensity = new FloatParameter { value = 0.5f };

    }

    public sealed class ColorAdjustmentTechnicolorRenderer : PostProcessEffectRenderer<ColorAdjustmentTechnicolor>
    {
        private Shader shader;
        private const string PROFILER_TAG = "X-ColorAdjustmentTechnicolor";


        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/ColorAdjustment/Technicolor");
        }

        public override void Release()
        {
            base.Release();
        }

        static class ShaderIDs
        {
            internal static readonly int exposure = Shader.PropertyToID("_Exposure");
            internal static readonly int colorBalance = Shader.PropertyToID("_ColorBalance");
            internal static readonly int indensity = Shader.PropertyToID("_Indensity");
        }

        public override void Render(PostProcessRenderContext context)
        {
            context.command.BeginSample(PROFILER_TAG);
            PropertySheet sheet = context.propertySheets.Get(shader);


            sheet.properties.SetFloat(ShaderIDs.exposure, 8f- settings.exposure);
            sheet.properties.SetVector(ShaderIDs.colorBalance, Vector3.one - new Vector3(settings.colorBalanceR, settings.colorBalanceG, settings.colorBalanceB));
            sheet.properties.SetFloat(ShaderIDs.indensity, settings.indensity);



            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
            context.command.EndSample(PROFILER_TAG);
        }
    }
}
        


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
    [PostProcess(typeof(TiltShiftBlurV2Renderer), PostProcessEvent.AfterStack, "X-PostProcessing/Blur/TiltShiftBlur/TiltShiftBlurV2")]
    public class TiltShiftBlurV2 : PostProcessEffectSettings
    {

        [Range(0f, 3f)]
        public FloatParameter BlurRadius = new FloatParameter { value = 1f };

        [Range(8, 128)]
        public IntParameter Iteration = new IntParameter { value = 32 };

        [Range(-1f, 1f)]
        public FloatParameter centerOffset = new FloatParameter { value = 0f };

        [Range(0f, 20f)]
        public FloatParameter AreaSize = new FloatParameter { value = 1f };

        [Range(1f, 20f)]
        public FloatParameter areaSmooth = new FloatParameter { value = 1.2f };

        public BoolParameter showPreview = new BoolParameter { value = false };

    }

    public sealed class TiltShiftBlurV2Renderer : PostProcessEffectRenderer<TiltShiftBlurV2>
    {

        private const string PROFILER_TAG = "X-TiltShiftBlurV2";
        private Shader shader;
        private Vector4 mGoldenRot = new Vector4();

        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/TiltShiftBlurV2");

            // Precompute rotations
            float c = Mathf.Cos(2.39996323f);
            float s = Mathf.Sin(2.39996323f);
            mGoldenRot.Set(c, s, -s, c);
        }

        public override void Release()
        {
            base.Release();
        }

        static class ShaderIDs
        {
            internal static readonly int GoldenRot = Shader.PropertyToID("_GoldenRot");
            internal static readonly int Gradient = Shader.PropertyToID("_Gradient");
            internal static readonly int Params = Shader.PropertyToID("_Params");
        }

        public override void Render(PostProcessRenderContext context)
        {
            CommandBuffer cmd = context.command;
            PropertySheet sheet = context.propertySheets.Get(shader);
            cmd.BeginSample(PROFILER_TAG);

            sheet.properties.SetVector(ShaderIDs.GoldenRot, mGoldenRot);
            sheet.properties.SetVector(ShaderIDs.Gradient, new Vector3(settings.centerOffset, settings.AreaSize, settings.areaSmooth));
            sheet.properties.SetVector(ShaderIDs.Params, new Vector4(settings.Iteration, settings.BlurRadius, 1f / context.width, 1f / context.height));


            cmd.BlitFullscreenTriangle(context.source, context.destination, sheet, settings.showPreview ? 1 : 0);
            cmd.EndSample(PROFILER_TAG);
        }
    }
}
        

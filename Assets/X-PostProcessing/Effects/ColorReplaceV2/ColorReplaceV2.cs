
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
    [PostProcess(typeof(ColorReplaceV2Renderer), PostProcessEvent.AfterStack, "X-PostProcessing/ColorAdjustment/ColorReplaceV2")]
    public class ColorReplaceV2 : PostProcessEffectSettings
    {


        public GradientParameter FromGradientColor = new GradientParameter { value = null };

        public GradientParameter ToGradientColor = new GradientParameter { value = null };


        [Range(0.0f, 100.0f)]
        public FloatParameter gridentSpeed = new FloatParameter { value = 0.5f };

        [Range(0.0f, 1.0f)]
        public FloatParameter Range = new FloatParameter { value = 0.2f };

        [Range(0.0f, 1.0f)]
        public FloatParameter Fuzziness = new FloatParameter { value = 0.5f };
    }

    public sealed class ColorReplaceV2Renderer : PostProcessEffectRenderer<ColorReplaceV2>
    {
        private Shader shader;
        private float TimeX = 1.0f;
        private const string PROFILER_TAG = "X-ColorReplaceV2";

        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/ColorReplaceV2");
        }

        public override void Release()
        {
            base.Release();
        }

        static class ShaderIDs
        {
            internal static readonly int FromColor = Shader.PropertyToID("_FromColor");
            internal static readonly int ToColor = Shader.PropertyToID("_ToColor");
            internal static readonly int Range = Shader.PropertyToID("_Range");
            internal static readonly int Fuzziness = Shader.PropertyToID("_Fuzziness");
        }

        public override void Render(PostProcessRenderContext context)
        {
            CommandBuffer cmd = context.command;
            PropertySheet sheet = context.propertySheets.Get(shader);
            cmd.BeginSample(PROFILER_TAG);

            TimeX += (Time.deltaTime * settings.gridentSpeed);
            if (TimeX > 100)
            {
                TimeX = 0;
            }
            if (settings.FromGradientColor.value != null)
            {
                sheet.properties.SetColor(ShaderIDs.FromColor, settings.FromGradientColor.value.Evaluate(TimeX * 0.01f));
            }

            if (settings.ToGradientColor.value != null)
            {
                sheet.properties.SetColor(ShaderIDs.ToColor, settings.ToGradientColor.value.Evaluate(TimeX * 0.01f));
            }

            sheet.properties.SetFloat(ShaderIDs.Range, settings.Range);
            sheet.properties.SetFloat(ShaderIDs.Fuzziness, settings.Fuzziness);

            cmd.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
            cmd.EndSample(PROFILER_TAG);
        }
    }
}


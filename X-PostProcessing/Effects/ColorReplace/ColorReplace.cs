
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
    [PostProcess(typeof(ColorReplaceRenderer), PostProcessEvent.AfterStack, "X-PostProcessing/ColorAdjustment/ColorReplace")]
    public class ColorReplace : PostProcessEffectSettings
    {

        [ColorUsageAttribute(true, true, 0f, 20f, 0.125f, 3f)]
        public ColorParameter FromColor = new ColorParameter { value = new Color(0.8f, 0.0f, 0.0f, 1) };

        [ColorUsageAttribute(true, true, 0f, 20f, 0.125f, 3f)]
        public ColorParameter ToColor = new ColorParameter { value = new Color(0.0f, 0.8f, 0.0f, 1) };

        [Range(0.0f, 1.0f)]
        public FloatParameter Range = new FloatParameter { value = 0.2f };

        [Range(0.0f, 1.0f)]
        public FloatParameter Fuzziness = new FloatParameter { value = 0.5f };

    }

    public sealed class ColorReplaceRenderer : PostProcessEffectRenderer<ColorReplace>
    {
        private Shader shader;
        private const string PROFILER_TAG = "X-ColorReplace";

        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/ColorReplace");
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

            sheet.properties.SetColor("_FromColor", settings.FromColor);
            sheet.properties.SetColor("_ToColor", settings.ToColor);
            sheet.properties.SetFloat("_Range", settings.Range);
            sheet.properties.SetFloat("_Fuzziness", settings.Fuzziness);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
            cmd.EndSample(PROFILER_TAG);
        }
    }
}
        

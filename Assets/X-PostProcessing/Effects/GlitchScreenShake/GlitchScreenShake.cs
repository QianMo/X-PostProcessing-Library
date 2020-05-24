
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
    public sealed class GlitchScreenShakeDirectionParameter : ParameterOverride<Direction> { }

    [Serializable]
    [PostProcess(typeof(GlitchScreenShakeRenderer), PostProcessEvent.AfterStack, "X-PostProcessing/Glitch/ScreenShake")]
    public class GlitchScreenShake : PostProcessEffectSettings
    {
        public GlitchScreenShakeDirectionParameter ScreenShakeDirection = new GlitchScreenShakeDirectionParameter { value = Direction.Horizontal };

        [Range(0.0f, 1.0f)]
        public FloatParameter ScreenShakeIndensity = new FloatParameter { value = 0.5f };
    }

    public sealed class GlitchScreenShakeRenderer : PostProcessEffectRenderer<GlitchScreenShake>
    {
        private Shader shader;

        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/Glitch/ScreenShake");
        }

        public override void Release()
        {
            base.Release();
        }

        static class ShaderIDs
        {
            internal static readonly int ScreenShakeIndensity = Shader.PropertyToID("_ScreenShake");
        }

        public override void Render(PostProcessRenderContext context)
        {
            PropertySheet sheet = context.propertySheets.Get(shader);

            sheet.properties.SetFloat(ShaderIDs.ScreenShakeIndensity, settings.ScreenShakeIndensity * 0.25f);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, (int)settings.ScreenShakeDirection.value);
        }
    }
}
        

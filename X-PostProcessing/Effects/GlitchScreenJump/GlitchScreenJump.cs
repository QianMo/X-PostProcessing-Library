
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
    public sealed class GlitchScreenJumpDirectionParameter : ParameterOverride<Direction> { }

    [Serializable]
    [PostProcess(typeof(GlitchScreenJumpRenderer), PostProcessEvent.AfterStack, "X-PostProcessing/Glitch/ScreenJump", false)]
    public class GlitchScreenJump : PostProcessEffectSettings
    {
        public GlitchScreenJumpDirectionParameter ScreenJumpDirection = new GlitchScreenJumpDirectionParameter { value = Direction.Vertical };

        [Range(0.0f, 1.0f)]
        public FloatParameter ScreenJumpIndensity= new FloatParameter { value = 0.35f };


    }

    public sealed class GlitchScreenJumpRenderer : PostProcessEffectRenderer<GlitchScreenJump>
    {
        private Shader shader;

        float ScreenJumpTime;

        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/Glitch/ScreenJump");
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
            PropertySheet sheet = context.propertySheets.Get(shader);

            ScreenJumpTime += Time.deltaTime * settings.ScreenJumpIndensity * 9.8f;

            Vector2 ScreenJumpVector = new Vector2(settings.ScreenJumpIndensity, ScreenJumpTime);

            sheet.properties.SetVector(ShaderIDs.Params, ScreenJumpVector);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, (int)settings.ScreenJumpDirection.value);
        }
    }
}
        

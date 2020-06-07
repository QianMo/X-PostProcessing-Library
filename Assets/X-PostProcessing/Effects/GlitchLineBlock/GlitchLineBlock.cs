
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
    public sealed class IntervalTypeParameter : ParameterOverride<IntervalType> { };

    [Serializable]
    [PostProcess(typeof(GlitchLineBlockRenderer), PostProcessEvent.AfterStack, "X-PostProcessing/Glitch/LineBlock")]
    public class GlitchLineBlock : PostProcessEffectSettings
    {

        public DirectionParameter blockDirection = new DirectionParameter { value = Direction.Horizontal };

        public IntervalTypeParameter intervalType = new IntervalTypeParameter { value = IntervalType.Random };

        [Range(0f, 25f)]
        public FloatParameter frequency = new FloatParameter { value = 1f };

        [Range(0f, 1f)]
        public FloatParameter Amount = new FloatParameter { value = 0.5f };

        [Range(0.1f, 10f)]
        public FloatParameter LinesWidth = new FloatParameter { value = 1f };

        [Range(0f, 1f)]
        public FloatParameter Speed = new FloatParameter { value = 0.8f };

        [Range(0f, 13f)]
        public FloatParameter Offset = new FloatParameter { value = 1f };

        [Range(0f, 1f)]
        public FloatParameter Alpha = new FloatParameter { value = 1f };

    }

    public sealed class GlitchLineBlockRenderer : PostProcessEffectRenderer<GlitchLineBlock>
    {

        private const string PROFILER_TAG = "X-GlitchLineBlock";
        private Shader shader;
        private float TimeX = 1.0f;
        private float randomFrequency;
        private int frameCount = 0;

        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/Glitch/LineBlock");
        }

        public override void Release()
        {
            base.Release();
        }

        static class ShaderIDs
        {

            internal static readonly int Params = Shader.PropertyToID("_Params");
            internal static readonly int Params2 = Shader.PropertyToID("_Params2");
        }

        public override void Render(PostProcessRenderContext context)
        {

            CommandBuffer cmd = context.command;
            PropertySheet sheet = context.propertySheets.Get(shader);
            cmd.BeginSample(PROFILER_TAG);

            UpdateFrequency(sheet);

            TimeX += Time.deltaTime;
            if (TimeX > 100)
            {
                TimeX = 0;
            }

            sheet.properties.SetVector(ShaderIDs.Params, new Vector3(
                settings.intervalType.value == IntervalType.Random ? randomFrequency : settings.frequency,
                TimeX * settings.Speed * 0.2f , settings.Amount));

            sheet.properties.SetVector(ShaderIDs.Params2, new Vector3(settings.Offset, 1 / settings.LinesWidth, settings.Alpha));

            cmd.BlitFullscreenTriangle(context.source, context.destination, sheet, (int)settings.blockDirection.value);
            cmd.EndSample(PROFILER_TAG);
        }

        void UpdateFrequency(PropertySheet sheet)
        {
            if (settings.intervalType.value == IntervalType.Random)
            {
                if (frameCount > settings.frequency)
                {

                    frameCount = 0;
                    randomFrequency = UnityEngine.Random.Range(0, settings.frequency);
                }
                frameCount++;
            }

            if (settings.intervalType.value == IntervalType.Infinite)
            {
                sheet.EnableKeyword("USING_FREQUENCY_INFINITE");
            }
            else
            {
                sheet.DisableKeyword("USING_FREQUENCY_INFINITE");
            }
        }
    }
}
        

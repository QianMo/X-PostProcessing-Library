
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
    [PostProcess(typeof(GlitchWaveJitterRenderer), PostProcessEvent.AfterStack, "X-PostProcessing/Glitch/WaveJitter")]
    public class GlitchWaveJitter : PostProcessEffectSettings
    {

        public DirectionParameter jitterDirection = new DirectionParameter { value = Direction.Horizontal };

        public IntervalTypeParameter intervalType = new IntervalTypeParameter { value = IntervalType.Random };

        [Range(0f, 50f)]
        public FloatParameter frequency = new FloatParameter { value = 5f };

        [Range(0f, 50f)]
        public FloatParameter RGBSplit = new FloatParameter { value = 20f };

        [Range(0f, 1f)]
        public FloatParameter speed = new FloatParameter { value = 0.25f };

        [Range(0f, 2f)]
        public FloatParameter amount = new FloatParameter { value = 1f };

        public BoolParameter customResolution = new BoolParameter { value = false };

        public Vector2Parameter resolution = new Vector2Parameter { value = new Vector2(640f, 480f) };
    }

    public sealed class GlitchWaveJitterRenderer : PostProcessEffectRenderer<GlitchWaveJitter>
    {

        private const string PROFILER_TAG = "X-GlitchWaveJitter";
        private Shader shader;
        private float randomFrequency;

        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/Glitch/WaveJitter");
        }

        public override void Release()
        {
            base.Release();
        }

        static class ShaderIDs
        {
            internal static readonly int Params = Shader.PropertyToID("_Params");
            internal static readonly int Resolution = Shader.PropertyToID("_Resolution");
        }

        public override void Render(PostProcessRenderContext context)
        {

            CommandBuffer cmd = context.command;
            PropertySheet sheet = context.propertySheets.Get(shader);
            cmd.BeginSample(PROFILER_TAG);

            UpdateFrequency(sheet);

            sheet.properties.SetVector(ShaderIDs.Params, new Vector4(settings.intervalType.value == IntervalType.Random ? randomFrequency : settings.frequency
                , settings.RGBSplit, settings.speed, settings.amount));
            sheet.properties.SetVector(ShaderIDs.Resolution, settings.customResolution ? settings.resolution : new Vector2(Screen.width, Screen.height));

            cmd.BlitFullscreenTriangle(context.source, context.destination, sheet, (int)settings.jitterDirection.value);
            cmd.EndSample(PROFILER_TAG);
        }


        void UpdateFrequency(PropertySheet sheet)
        {
            if (settings.intervalType.value == IntervalType.Random)
            {
                randomFrequency = UnityEngine.Random.Range(0, settings.frequency);
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


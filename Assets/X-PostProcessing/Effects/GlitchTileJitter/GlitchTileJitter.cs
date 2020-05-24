
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
    [PostProcess(typeof(GlitchTileJitterRenderer), PostProcessEvent.AfterStack, "X-PostProcessing/Glitch/TileJitter")]
    public class GlitchTileJitter : PostProcessEffectSettings
    {

        public DirectionParameter jitterDirection = new DirectionParameter { value = Direction.Horizontal };

        public IntervalTypeParameter intervalType = new IntervalTypeParameter { value = IntervalType.Random };

        [Range(0f, 25f)]
        public FloatParameter frequency = new FloatParameter { value = 1f };

        public DirectionParameter splittingDirection = new DirectionParameter { value = Direction.Vertical };

        [Range(0f, 50f)]
        public FloatParameter splittingNumber = new FloatParameter { value = 5f };

        [Range(0f, 100f)]
        public FloatParameter amount = new FloatParameter { value = 10f };

        [Range(0f, 1f)]
        public FloatParameter speed = new FloatParameter { value = 0.35f };
    }

    public sealed class GlitchTileJitterRenderer : PostProcessEffectRenderer<GlitchTileJitter>
    {

        private const string PROFILER_TAG = "X-GlitchTileJitter";
        private Shader shader;
        private float randomFrequency;


        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/Glitch/TileJitter");
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

            CommandBuffer cmd = context.command;
            PropertySheet sheet = context.propertySheets.Get(shader);
            cmd.BeginSample(PROFILER_TAG);

            UpdateFrequency(sheet);       

            if (settings.jitterDirection.value == Direction.Horizontal)
            {
                sheet.EnableKeyword("JITTER_DIRECTION_HORIZONTAL");
            }
            else
            {
                 sheet.DisableKeyword("JITTER_DIRECTION_HORIZONTAL");
            }

            sheet.properties.SetVector(ShaderIDs.Params, new Vector4(settings.splittingNumber, settings.amount, settings.speed * 100f, 
                settings.intervalType.value == IntervalType.Random ? randomFrequency : settings.frequency));

            cmd.BlitFullscreenTriangle(context.source, context.destination, sheet, settings.splittingDirection.value == Direction.Horizontal ? 0 : 1);
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
        

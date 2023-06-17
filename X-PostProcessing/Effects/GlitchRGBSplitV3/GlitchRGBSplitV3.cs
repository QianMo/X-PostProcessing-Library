
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
    public sealed class DirectionEXParameter : ParameterOverride<DirectionEX> { }
    [Serializable]
    [PostProcess(typeof(GlitchRGBSplitV3Renderer), PostProcessEvent.AfterStack, "X-PostProcessing/Glitch/RGBSplitV3")]
    public class GlitchRGBSplitV3 : PostProcessEffectSettings
    {
        public DirectionEXParameter SplitDirection = new DirectionEXParameter { value = DirectionEX.Horizontal };

        public IntervalTypeParameter intervalType = new IntervalTypeParameter { value = IntervalType.Random };
        
        [Range(0.1f, 25f)]
        public FloatParameter Frequency = new FloatParameter { value = 3f };
      
        
        [Range(0f, 200f)]
        public FloatParameter Amount = new FloatParameter { value = 30f };
        
        [Range(0f, 15f)]
        public FloatParameter Speed = new FloatParameter { value = 20f };
    }

    public sealed class GlitchRGBSplitV3Renderer : PostProcessEffectRenderer<GlitchRGBSplitV3>
    {

        private const string PROFILER_TAG = "X-GlitchRGBSplitV3";
        private Shader shader;
        private float randomFrequency;
        private int frameCount = 0;

        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/Glitch/RGBSplitV3");
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

            sheet.properties.SetVector(ShaderIDs.Params, new Vector3(settings.intervalType.value == IntervalType.Random ? randomFrequency : settings.Frequency
             , settings.Amount, settings.Speed));

            cmd.BlitFullscreenTriangle(context.source, context.destination, sheet, (int)settings.SplitDirection.value);
            cmd.EndSample(PROFILER_TAG);
        }


        void UpdateFrequency(PropertySheet sheet)
        {
            if (settings.intervalType.value == IntervalType.Random)
            {
                if (frameCount > settings.Frequency)
                {

                    frameCount = 0;
                    randomFrequency = UnityEngine.Random.Range(0, settings.Frequency);
                }
                frameCount++;
            }

            if (settings.intervalType.value == IntervalType.Infinite)
            {
                sheet.EnableKeyword("USING_Frequency_INFINITE");
            }
            else
            {
                sheet.DisableKeyword("USING_Frequency_INFINITE");
            }
        }
    }
}
        

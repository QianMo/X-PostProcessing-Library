
//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// created by QianMo @ 2020
//----------------------------------------------------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;


namespace XPostProcessing
{

    [Serializable]
    [PostProcess(typeof(GrainyBlurRenderer), PostProcessEvent.AfterStack, "X-PostProcessing/Blur/GrainyBlur")]
    public class GrainyBlur : PostProcessEffectSettings
    {

        [Range(0.0f, 50.0f)]
        public FloatParameter BlurRadius = new FloatParameter { value = 5.0f };

        [Range(1, 8)]
        public IntParameter Iteration = new IntParameter { value = 4 };

        [Range(1, 10)]
        public FloatParameter RTDownScaling = new FloatParameter { value = 1 };
    }

    public sealed class GrainyBlurRenderer : PostProcessEffectRenderer<GrainyBlur>
    {

        private const string PROFILER_TAG = "X-GrainyBlur";
        private Shader shader;


        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/GrainyBlur");
        }

        public override void Release()
        {
            base.Release();
        }

        static class ShaderIDs
        {
            internal static readonly int BlurRadius = Shader.PropertyToID("_BlurRadius");
            internal static readonly int Iteration = Shader.PropertyToID("_Iteration");
            internal static readonly int bufferRT = Shader.PropertyToID("_BufferRT");
        }

        public override void Render(PostProcessRenderContext context)
        {

            CommandBuffer cmd = context.command;
            PropertySheet sheet = context.propertySheets.Get(shader);

            cmd.BeginSample(PROFILER_TAG);

            if (settings.RTDownScaling >1)
            {
                int RTWidth = (int)(context.screenWidth / settings.RTDownScaling);
                int RTHeight = (int)(context.screenHeight / settings.RTDownScaling);
                cmd.GetTemporaryRT(ShaderIDs.bufferRT, RTWidth, RTHeight, 0, FilterMode.Bilinear);
                // downsample screen copy into smaller RT
                context.command.BlitFullscreenTriangle(context.source, ShaderIDs.bufferRT);
            }

            sheet.properties.SetFloat(ShaderIDs.BlurRadius, settings.BlurRadius / context.height);
            sheet.properties.SetFloat(ShaderIDs.Iteration, settings.Iteration);

            if (settings.RTDownScaling > 1)
            {
                cmd.BlitFullscreenTriangle(ShaderIDs.bufferRT, context.destination, sheet, 0);
            }
            else
            {
                cmd.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
            }
                

            cmd.ReleaseTemporaryRT(ShaderIDs.bufferRT);
            cmd.EndSample(PROFILER_TAG);
        }

    }
}
        

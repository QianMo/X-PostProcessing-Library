
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
    [PostProcess(typeof(GaussianBlurRenderer), PostProcessEvent.AfterStack, "X-PostProcessing/Blur/GaussianBlur")]
    public class GaussianBlur : PostProcessEffectSettings
    {

        [Range(0f, 5f)]
        public FloatParameter BlurRadius = new FloatParameter { value = 3f };

        [Range(1, 15)]
        public IntParameter Iteration = new IntParameter { value = 6 };

        [Range(1, 8)]
        public FloatParameter RTDownScaling = new FloatParameter { value = 2f };
    }

    public sealed class GaussianBlurRenderer : PostProcessEffectRenderer<GaussianBlur>
    {
        private Shader shader;
        private const string PROFILER_TAG = "X-GaussianBlur";


        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/GaussianBlur");
        }

        public override void Release()
        {
            base.Release();
        }

        static class ShaderIDs
        {

            internal static readonly int BlurRadius = Shader.PropertyToID("_BlurOffset");
            internal static readonly int bufferRT1 = Shader.PropertyToID("_BufferRT1");
            internal static readonly int bufferRT2 = Shader.PropertyToID("_BufferRT2");
        }

        public override void Render(PostProcessRenderContext context)
        {

            CommandBuffer cmd = context.command;
            PropertySheet sheet = context.propertySheets.Get(shader);

            cmd.BeginSample(PROFILER_TAG);

            int RTWidth = (int)(context.screenWidth / settings.RTDownScaling);
            int RTHeight = (int)(context.screenHeight / settings.RTDownScaling);
            cmd.GetTemporaryRT(ShaderIDs.bufferRT1, RTWidth, RTHeight, 0, FilterMode.Bilinear);
            cmd.GetTemporaryRT(ShaderIDs.bufferRT2, RTWidth, RTHeight, 0, FilterMode.Bilinear);

            // downsample screen copy into smaller RT
            context.command.BlitFullscreenTriangle(context.source, ShaderIDs.bufferRT1);


            for (int i = 0; i < settings.Iteration; i++)
            {
                // horizontal blur
                sheet.properties.SetVector(ShaderIDs.BlurRadius, new Vector4(settings.BlurRadius / context.screenWidth, 0, 0, 0));
                context.command.BlitFullscreenTriangle(ShaderIDs.bufferRT1, ShaderIDs.bufferRT2, sheet, 0);

                // vertical blur
                sheet.properties.SetVector(ShaderIDs.BlurRadius, new Vector4(0, settings.BlurRadius / context.screenHeight, 0, 0));
                context.command.BlitFullscreenTriangle(ShaderIDs.bufferRT2, ShaderIDs.bufferRT1, sheet, 0);
            }

            // Render blurred texture in blend pass
            cmd.BlitFullscreenTriangle(ShaderIDs.bufferRT1, context.destination, sheet, 1);

            // release
            cmd.ReleaseTemporaryRT(ShaderIDs.bufferRT1);
            cmd.ReleaseTemporaryRT(ShaderIDs.bufferRT2);

            cmd.EndSample(PROFILER_TAG);
        }
    }
}
        

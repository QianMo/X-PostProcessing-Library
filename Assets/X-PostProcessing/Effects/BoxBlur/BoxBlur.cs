
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
    [PostProcess(typeof(BoxBlurRenderer), PostProcessEvent.AfterStack, "X-PostProcessing/Blur/BoxBlur")]
    public class BoxBlur : PostProcessEffectSettings
    {
        [Range(0f, 5f)]
        public FloatParameter BlurRadius = new FloatParameter { value = 3f };

        [Range(1, 20)]
        public IntParameter Iteration = new IntParameter { value = 6 };

        [Range(1, 8)]
        public FloatParameter RTDownScaling = new FloatParameter { value = 2 };
    }

    public sealed class BoxBlurRenderer : PostProcessEffectRenderer<BoxBlur>
    {
        private Shader shader;
        private const string PROFILER_TAG = "X-BoxBlur";


        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/BoxBlur");
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

            int RTWidth = (int) (context.screenWidth / settings.RTDownScaling);
            int RTHeight = (int) (context.screenHeight / settings.RTDownScaling);
            cmd.GetTemporaryRT(ShaderIDs.bufferRT1, RTWidth, RTHeight, 0, FilterMode.Bilinear);
            cmd.GetTemporaryRT(ShaderIDs.bufferRT2, RTWidth, RTHeight, 0, FilterMode.Bilinear);

            // downsample screen copy into smaller RT
            context.command.BlitFullscreenTriangle(context.source, ShaderIDs.bufferRT1);


            for (int i = 0; i < settings.Iteration; i++)
            {
                if (settings.Iteration > 20)
                {
                    return;
                }

                Vector4 BlurRadius = new Vector4(settings.BlurRadius / (float)context.screenWidth, settings.BlurRadius / (float)context.screenHeight, 0, 0);
                // RT1 -> RT2
                sheet.properties.SetVector(ShaderIDs.BlurRadius, BlurRadius);
                context.command.BlitFullscreenTriangle(ShaderIDs.bufferRT1, ShaderIDs.bufferRT2, sheet, 0);

                // RT2 -> RT1
                sheet.properties.SetVector(ShaderIDs.BlurRadius, BlurRadius);
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
        

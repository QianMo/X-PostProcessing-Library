
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
    [PostProcess(typeof(KawaseBlurRenderer), PostProcessEvent.AfterStack, "X-PostProcessing/Blur/KawaseBlur")]
    public class KawaseBlur : PostProcessEffectSettings
    {

        [Range(0.0f, 5.0f)]
        public FloatParameter BlurRadius = new FloatParameter { value = 0.5f };

        [Range(1, 10)]
        public IntParameter Iteration = new IntParameter { value = 6 };

        [Range(1, 8)]
        public FloatParameter RTDownScaling = new FloatParameter { value = 2 };

    }

    public sealed class KawaseBlurRenderer : PostProcessEffectRenderer<KawaseBlur>
    {

        private const string PROFILER_TAG = "X-KawaseBlur";
        private Shader shader;


        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/KawaseBlur");
        }

        public override void Release()
        {
            base.Release();
        }

        static class ShaderIDs
        {
            internal static readonly int BlurRadius = Shader.PropertyToID("_Offset");

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


            bool needSwitch = true;
            for (int i = 0; i < settings.Iteration; i++)
            {
                sheet.properties.SetFloat(ShaderIDs.BlurRadius, i / settings.RTDownScaling + settings.BlurRadius);
                context.command.BlitFullscreenTriangle(needSwitch ? ShaderIDs.bufferRT1 : ShaderIDs.bufferRT2, needSwitch ? ShaderIDs.bufferRT2 : ShaderIDs.bufferRT1, sheet, 0);
                needSwitch = !needSwitch;
            }


            sheet.properties.SetFloat(ShaderIDs.BlurRadius, settings.Iteration / settings.RTDownScaling + settings.BlurRadius);
            cmd.BlitFullscreenTriangle(needSwitch ? ShaderIDs.bufferRT1 : ShaderIDs.bufferRT2, context.destination, sheet, 0);

            // release
            cmd.ReleaseTemporaryRT(ShaderIDs.bufferRT1);
            cmd.ReleaseTemporaryRT(ShaderIDs.bufferRT2);
            cmd.EndSample(PROFILER_TAG);
        }
    }
}


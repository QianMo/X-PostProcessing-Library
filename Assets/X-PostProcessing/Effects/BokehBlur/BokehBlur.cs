
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
    [PostProcess(typeof(BokehBlurRenderer), PostProcessEvent.AfterStack, "X-PostProcessing/Blur/BokehBlur")]
    public class BokehBlur : PostProcessEffectSettings
    {
        [Range(0f, 3f)]
        public FloatParameter blurRadius = new FloatParameter { value = 1f };

        [Range(8, 128)]
        public IntParameter iteration = new IntParameter { value = 32 };

        [Range(1, 10)]
        public FloatParameter RTDownScaling = new FloatParameter { value = 2 };

    }

    public sealed class BokehBlurRenderer : PostProcessEffectRenderer<BokehBlur>
    {

        private const string PROFILER_TAG = "X-BokehBlur";
        private Shader shader;
        private Vector4 mGoldenRot = new Vector4();

        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/BokehBlur");

            // Precompute rotations
            float c = Mathf.Cos(2.39996323f);
            float s = Mathf.Sin(2.39996323f);
            mGoldenRot.Set(c, s, -s, c);
        }

        public override void Release()
        {
            base.Release();
        }

        static class ShaderIDs
        {
            internal static readonly int GoldenRot = Shader.PropertyToID("_GoldenRot");
            internal static readonly int Params = Shader.PropertyToID("_Params");
            internal static readonly int bufferRT1 = Shader.PropertyToID("_BufferRT1");
        }

        public override void Render(PostProcessRenderContext context)
        {

            CommandBuffer cmd = context.command;
            PropertySheet sheet = context.propertySheets.Get(shader);
            cmd.BeginSample(PROFILER_TAG);

            int RTWidth = (int)(context.screenWidth / settings.RTDownScaling);
            int RTHeight = (int)(context.screenHeight / settings.RTDownScaling);
            cmd.GetTemporaryRT(ShaderIDs.bufferRT1, RTWidth, RTHeight, 0, FilterMode.Bilinear);

            // downsample screen copy into smaller RT
            context.command.BlitFullscreenTriangle(context.source, ShaderIDs.bufferRT1);

            sheet.properties.SetVector(ShaderIDs.GoldenRot, mGoldenRot);
            sheet.properties.SetVector(ShaderIDs.Params, new Vector4(settings.iteration, settings.blurRadius, 1f / context.width, 1f / context.height));

            cmd.BlitFullscreenTriangle(ShaderIDs.bufferRT1, context.destination, sheet, 0);
            cmd.ReleaseTemporaryRT(ShaderIDs.bufferRT1);
            cmd.EndSample(PROFILER_TAG);
        }
    }
}
        

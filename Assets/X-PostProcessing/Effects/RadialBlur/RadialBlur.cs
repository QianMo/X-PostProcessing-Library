using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;


namespace XPostProcessing
{


    [Serializable]
    [PostProcess(typeof(RadialBlurRenderer), PostProcessEvent.AfterStack, "X-PostProcessing/Blur/RadialBlur/RadialBlurV1")]
    public class RadialBlur : PostProcessEffectSettings
    {

        [Range(0.0f, 1.0f)]
        public FloatParameter blurRadius = new FloatParameter { value = 0.6f };

        [Range(2,30)]
        public IntParameter iteration = new IntParameter { value = 10 };

        [Range(0f, 1.0f)]
        public FloatParameter radialCenterX = new FloatParameter { value = 0.5f };
        [Range(0f, 1.0f)]
        public FloatParameter radialCenterY = new FloatParameter { value = 0.5f };

    }

    public sealed class RadialBlurRenderer : PostProcessEffectRenderer<RadialBlur>
    {
        private const string PROFILER_TAG = "X-RadialBlurV1";
        private Shader shader;

        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/RadialBlur");
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

            sheet.properties.SetVector(ShaderIDs.Params , new Vector4(settings.blurRadius * 0.02f, settings.iteration, settings.radialCenterX, settings.radialCenterY));

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
            cmd.EndSample(PROFILER_TAG);
        }
    }
}
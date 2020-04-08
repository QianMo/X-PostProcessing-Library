
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

    public enum RadialBlurQuality
    {
        RadialBlur_4Tap_Fatest = 0,
        RadialBlur_6Tap = 1,
        RadialBlur_8Tap_Balance = 2,
        RadialBlur_10Tap = 3,
        RadialBlur_12Tap = 4,
        RadialBlur_20Tap_Quality = 5,
        RadialBlur_30Tap_Extreme = 6,
    }

    [Serializable]
    public sealed class RadialBlurQualityParameter : ParameterOverride<RadialBlurQuality> { }


    [Serializable]
    [PostProcess(typeof(RadialBlurV2Renderer), PostProcessEvent.AfterStack, "X-PostProcessing/Blur/RadialBlur/RadialBlurV2")]
    public class RadialBlurV2 : PostProcessEffectSettings
    {
        public RadialBlurQualityParameter QualityLevel = new RadialBlurQualityParameter { value = RadialBlurQuality.RadialBlur_8Tap_Balance };

        [Range(-1.0f, 1.0f)]
        public FloatParameter BlurRadius = new FloatParameter { value = 0.6f };

        [Range(0f, 1.0f)]
        public FloatParameter RadialCenterX = new FloatParameter { value = 0.5f };
        
        [Range(0f, 1.0f)]
        public FloatParameter RadialCenterY = new FloatParameter { value = 0.5f };
    }

    public sealed class RadialBlurV2Renderer : PostProcessEffectRenderer<RadialBlurV2>
    {
        private Shader shader;
        private const string PROFILER_TAG = "X-RadialBlurV2";
        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/RadialBlurV2");
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

            sheet.properties.SetVector(ShaderIDs.Params, new Vector3(settings.BlurRadius * 0.02f, settings.RadialCenterX, settings.RadialCenterY));

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, (int)settings.QualityLevel.value);
            cmd.EndSample(PROFILER_TAG);
        }
    }
}
        

using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;


namespace XPostProcessing
{

    [Serializable]
    [PostProcess(typeof(RapidOldTVVignetteRenderer), PostProcessEvent.AfterStack, "X-PostProcessing/Vignette/RapidOldTVVignette")]
    public class RapidOldTVVignette : PostProcessEffectSettings
    {

        public VignetteTypeParameter vignetteType = new VignetteTypeParameter { value = VignetteType.ClassicMode };

        [Range(0.0f, 5.0f)]
        public FloatParameter vignetteIndensity = new FloatParameter { value = 1f };

        public Vector2Parameter vignetteCenter = new Vector2Parameter { value = new Vector2(0.5f, 0.5f) };

        [ColorUsageAttribute(true, true, 0f, 20f, 0.125f, 3f)]
        public ColorParameter vignetteColor = new ColorParameter { value = new Color(0.1f, 0.8f, 1.0f) };
    }

    public sealed class RapidOldTVVignetteRenderer : PostProcessEffectRenderer<RapidOldTVVignette>
    {
        private Shader shader;
        private const string PROFILER_TAG = "X-RapidOldTVVignette";

        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/RapidOldTVVignette");
        }

        public override void Release()
        {
            base.Release();
        }

        public override void Render(PostProcessRenderContext context)
        {
            CommandBuffer cmd = context.command;
            PropertySheet sheet = context.propertySheets.Get(shader);
            cmd.BeginSample(PROFILER_TAG);

            sheet.properties.SetFloat("_VignetteIndensity", settings.vignetteIndensity);
            sheet.properties.SetVector("_VignetteCenter", settings.vignetteCenter);

            if (settings.vignetteType.value == VignetteType.ColorMode)
            {
                sheet.properties.SetVector("_VignetteColor", settings.vignetteColor);
            }

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, (int)settings.vignetteType.value);
            cmd.EndSample(PROFILER_TAG);
        }
    }
}
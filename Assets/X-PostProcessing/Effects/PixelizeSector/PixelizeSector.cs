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
    [PostProcess(typeof(PixelizeSectorRenderer), PostProcessEvent.BeforeStack, "X-PostProcessing/Pixelize/PixelizeSector")]
    public class PixelizeSector : PostProcessEffectSettings
    {

        [Range(0.01f, 1.0f)]
        public FloatParameter pixelSize = new FloatParameter { value = 0.8f };
        [Range(0.01f, 1.0f)]
        public FloatParameter circleRadius = new FloatParameter { value = 0.8f };
        [Range(0.2f, 5.0f), Tooltip("Pixel interval X")]
        public FloatParameter pixelIntervalX = new FloatParameter { value = 1f };
        [Range(0.2f, 5.0f), Tooltip("Pixel interval Y")]
        public FloatParameter pixelIntervalY = new FloatParameter { value = 1f };
        [ColorUsageAttribute(true, true, 0f, 20f, 0.125f, 3f)]
        public ColorParameter BackgroundColor = new ColorParameter { value = new Color(0.0f, 0.0f, 0.0f) };
    }

    public sealed class PixelizeSectorRenderer : PostProcessEffectRenderer<PixelizeSector>
    {
        private const string PROFILER_TAG = "X-PixelizeSector";
        private Shader shader;

        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/PixelizeSector");
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

            float size = (1.01f - settings.pixelSize) * 300f;
            Vector4 parameters = new Vector4(size, ((context.screenWidth * 2 / context.screenHeight) * size / Mathf.Sqrt(3f)), settings.circleRadius, 0f);

            sheet.properties.SetVector("_Params", parameters);
            sheet.properties.SetFloat("_PixelIntervalX", settings.pixelIntervalX);
            sheet.properties.SetFloat("_PixelIntervalY", settings.pixelIntervalY);
            sheet.properties.SetColor("_BackgroundColor", settings.BackgroundColor);


            cmd.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
            cmd.EndSample(PROFILER_TAG);
        }
    }
}
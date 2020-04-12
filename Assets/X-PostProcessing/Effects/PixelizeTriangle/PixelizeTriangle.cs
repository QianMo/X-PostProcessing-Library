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
    [PostProcess(typeof(PixelizeTriangleRenderer), PostProcessEvent.BeforeStack, "X-PostProcessing/Pixelize/PixelizeTriangle")]
    public class PixelizeTriangle : PostProcessEffectSettings
    {

        [Range(0.001f, 1.0f)]
        public FloatParameter pixelSize = new FloatParameter { value = 0.5f };

        public BoolParameter useAutoScreenRatio = new BoolParameter { value = true };

        [Range(0.2f, 5.0f)]
        public FloatParameter pixelRatio = new FloatParameter { value = 1f };

        [Range(0.2f, 5.0f), Tooltip("像素缩放X")]
        public FloatParameter pixelScaleX = new FloatParameter { value = 1f };

        [Range(0.2f, 5.0f), Tooltip("像素缩放Y")]
        public FloatParameter pixelScaleY = new FloatParameter { value = 1f };
    }

    public sealed class PixelizeTriangleRenderer : PostProcessEffectRenderer<PixelizeTriangle>
    {
        private const string PROFILER_TAG = "X-PixelizeTriangle";
        private Shader shader;

        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/PixelizeTriangle");
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

            float size = (1.01f - settings.pixelSize) * 5f;
            sheet.properties.SetFloat("_PixelSize", size);

            float ratio = settings.pixelRatio;
            if (settings.useAutoScreenRatio)
            {
                ratio = (float)(context.width / (float)context.height);
                if (ratio == 0)
                {
                    ratio = 1f;
                }
            }
            sheet.properties.SetFloat("_PixelRatio", ratio);
            sheet.properties.SetFloat("_PixelScaleX", settings.pixelScaleX * 20);
            sheet.properties.SetFloat("_PixelScaleY", settings.pixelScaleY * 20);

            cmd.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
            cmd.EndSample(PROFILER_TAG);
        }
    }
}
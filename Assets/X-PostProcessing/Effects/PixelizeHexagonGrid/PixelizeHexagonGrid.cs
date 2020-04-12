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
    [PostProcess(typeof(PixelizeHexagonGridRenderer), PostProcessEvent.BeforeStack, "X-PostProcessing/Pixelize/PixelizeHexagonGrid")]
    public class PixelizeHexagonGrid : PostProcessEffectSettings
    {
        [Range(0.01f, 1.0f)]
        public FloatParameter pixelSize = new FloatParameter { value = 0.05f };

        [Range(0.01f, 5.0f)]
        public FloatParameter gridWidth = new FloatParameter { value = 1.0f };
        

        public BoolParameter useAutoScreenRatio = new BoolParameter { value = false };

        [Range(0.2f, 5.0f)]
        public FloatParameter pixelRatio = new FloatParameter { value = 1f };

        [Range(0.2f, 5.0f)]
        public FloatParameter pixelScaleX = new FloatParameter { value = 1f };

        [Range(0.2f, 5.0f)]
        public FloatParameter pixelScaleY = new FloatParameter { value = 1f };

    }

    public sealed class PixelizeHexagonGridRenderer : PostProcessEffectRenderer<PixelizeHexagonGrid>
    {
        private const string PROFILER_TAG = "X-PixelizeHexagonGrid";
        private Shader shader;

        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/PixelizeHexagonGrid");
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

            sheet.properties.SetFloat("_PixelSize", settings.pixelSize);
            sheet.properties.SetFloat("_GridWidth", settings.gridWidth);

            cmd.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
            cmd.EndSample(PROFILER_TAG);
        }  

    }
}
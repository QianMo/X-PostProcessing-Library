
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
    public enum IrisBlurQualityLevel
    {
        High_Quality = 0,
        Normal_Quality = 1,
    }

    [Serializable]
    public sealed class IrisBlurQualityLevelParameter : ParameterOverride<IrisBlurQualityLevel> { }

    [Serializable]
    [PostProcess(typeof(IrisBlurRenderer), PostProcessEvent.AfterStack, "X-PostProcessing/Blur/IrisBlur/IrisBlurV1")]
    public class IrisBlur : PostProcessEffectSettings
    {
        public IrisBlurQualityLevelParameter qualityLevel = new IrisBlurQualityLevelParameter { value = IrisBlurQualityLevel.High_Quality };

        [Range(0.0f, 1.0f)]
        public FloatParameter areaSize = new FloatParameter { value = 0.5f };

        [Range(0.0f, 1.0f)]
        public FloatParameter blurRadius = new FloatParameter { value = 1.0f };

        [Range(1,8)]
        public IntParameter Iteration = new IntParameter { value = 2 };

        [Range(1, 2)]
        public FloatParameter RTDownScaling = new FloatParameter { value = 1.0f };
    }

    public sealed class IrisBlurRenderer : PostProcessEffectRenderer<IrisBlur>
    {

        private const string PROFILER_TAG = "X-IrisBlur";
        private Shader shader;


        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/IrisBlur");
        }

        public override void Release()
        {
            base.Release();
        }

        static class ShaderIDs
        {
            internal static readonly int areaSize = Shader.PropertyToID("_BluSize");
            internal static readonly int blurRadius = Shader.PropertyToID("_BlurRadius");
            internal static readonly int blurredTex = Shader.PropertyToID("_BlurredTex");
            internal static readonly int bufferRT1 = Shader.PropertyToID("_BufferRT1");
            internal static readonly int bufferRT2 = Shader.PropertyToID("_BufferRT2");
        }

        public override void Render(PostProcessRenderContext context)
        {

            CommandBuffer cmd = context.command;
            PropertySheet sheet = context.propertySheets.Get(shader);
            cmd.BeginSample(PROFILER_TAG);

            if (settings.Iteration ==1)
            {
                HandleOneBlitBlur(context, cmd, sheet);
            }
            else
            {
                HandleMultipleIterationBlur(context, cmd, sheet, settings.Iteration);
            }

            cmd.EndSample(PROFILER_TAG);
        }

        void HandleOneBlitBlur(PostProcessRenderContext context, CommandBuffer cmd, PropertySheet sheet)
        {
            if (context == null || cmd == null || sheet == null)
            {
                return;
            }

            // Get RT
            int RTWidth = (int)(context.screenWidth / settings.RTDownScaling);
            int RTHeight = (int)(context.screenHeight / settings.RTDownScaling);
            cmd.GetTemporaryRT(ShaderIDs.bufferRT1, RTWidth, RTHeight, 0, FilterMode.Bilinear);

            // Set Property
            sheet.properties.SetFloat(ShaderIDs.areaSize, settings.areaSize);
            sheet.properties.SetFloat(ShaderIDs.blurRadius, settings.blurRadius);

            // Do Blit
            context.command.BlitFullscreenTriangle(context.source, ShaderIDs.bufferRT1, sheet, (int)settings.qualityLevel.value);

            // Final Blit
            cmd.SetGlobalTexture(ShaderIDs.blurredTex, ShaderIDs.bufferRT1);
            cmd.BlitFullscreenTriangle(context.source, context.destination, sheet, 2);

            // Release
            cmd.ReleaseTemporaryRT(ShaderIDs.bufferRT1);
        }


        void HandleMultipleIterationBlur(PostProcessRenderContext context, CommandBuffer cmd, PropertySheet sheet, int iteration)
        {
            if (context == null || cmd == null || sheet == null)
            {
                return;
            }

            // Get RT
            int RTWidth = (int)(context.screenWidth / settings.RTDownScaling);
            int RTHeight = (int)(context.screenHeight / settings.RTDownScaling);
            cmd.GetTemporaryRT(ShaderIDs.bufferRT1, RTWidth, RTHeight, 0, FilterMode.Bilinear);
            cmd.GetTemporaryRT(ShaderIDs.bufferRT2, RTWidth, RTHeight, 0, FilterMode.Bilinear);

            // Set Property
            sheet.properties.SetFloat(ShaderIDs.areaSize, settings.areaSize);
            sheet.properties.SetFloat(ShaderIDs.blurRadius, settings.blurRadius);

            RenderTargetIdentifier finalBlurID = ShaderIDs.bufferRT1;
            RenderTargetIdentifier firstID = context.source;
            RenderTargetIdentifier secondID = ShaderIDs.bufferRT1;
            for (int i = 0; i < iteration; i++)
            {
                // Do Blit
                context.command.BlitFullscreenTriangle(firstID, secondID, sheet, (int)settings.qualityLevel.value);

                finalBlurID = secondID;
                firstID = secondID;
                secondID = (secondID == ShaderIDs.bufferRT1) ? ShaderIDs.bufferRT2 : ShaderIDs.bufferRT1;
            }

            // Final Blit
            cmd.SetGlobalTexture(ShaderIDs.blurredTex, finalBlurID);
            cmd.BlitFullscreenTriangle(context.source, context.destination, sheet, 2);

            // Release
            cmd.ReleaseTemporaryRT(ShaderIDs.bufferRT1);
            cmd.ReleaseTemporaryRT(ShaderIDs.bufferRT2);
        }

    }
}
        

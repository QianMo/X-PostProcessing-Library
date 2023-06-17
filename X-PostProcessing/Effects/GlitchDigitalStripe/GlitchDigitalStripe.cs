
//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License 
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------

//reference : https://github.com/keijiro/KinoGlitch

using System;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

namespace XPostProcessing
{
    [Serializable]
    [PostProcess(typeof(GlitchDigitalStripeRenderer), PostProcessEvent.AfterStack, "X-PostProcessing/Glitch/DigitalStripe")]
    public class GlitchDigitalStripe : PostProcessEffectSettings
    {

        [Range(0.0f, 1.0f)]
        public FloatParameter intensity = new FloatParameter { value = 0.25f };

        [Range(1, 10)]
        public IntParameter frequncy = new IntParameter { value = 3 };

        [Range(0f, 0.99f)]
        public FloatParameter stripeLength = new FloatParameter { value = 0.89f };

        [Range(8, 256)]
        public IntParameter noiseTextureWidth = new IntParameter { value = 20 };

        [Range(8, 256)]
        public IntParameter noiseTextureHeight = new IntParameter { value = 20 };

        public BoolParameter needStripColorAdjust = new BoolParameter { value = false };

        [ColorUsageAttribute(true, true, 0f, 20f, 0.125f, 3f)]
        public ColorParameter StripColorAdjustColor = new ColorParameter { value = new Color(0.1f, 0.1f, 0.1f) };

        [Range(0, 10)]
        public FloatParameter StripColorAdjustIndensity = new FloatParameter { value = 2f };

    }

    public sealed class GlitchDigitalStripeRenderer : PostProcessEffectRenderer<GlitchDigitalStripe>
    {
        private const string PROFILER_TAG = "X-GlitchDigitalStripe";
        private Shader shader;
        Texture2D _noiseTexture;
        RenderTexture _trashFrame1;
        RenderTexture _trashFrame2;

        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/Glitch/DigitalStripe");
        }

        public override void Release()
        {
            base.Release();
        }

        static class ShaderIDs
        {
            internal static readonly int indensity = Shader.PropertyToID("_Indensity");
            internal static readonly int noiseTex = Shader.PropertyToID("_NoiseTex");
            internal static readonly int StripColorAdjustColor = Shader.PropertyToID("_StripColorAdjustColor");
            internal static readonly int StripColorAdjustIndensity = Shader.PropertyToID("_StripColorAdjustIndensity");
        }



        void UpdateNoiseTexture(int frame, int noiseTextureWidth, int noiseTextureHeight, float stripLength)
        {
            int frameCount = Time.frameCount;
            if (frameCount % frame != 0)
            {
                return;
            }

            _noiseTexture = new Texture2D(noiseTextureWidth, noiseTextureHeight, TextureFormat.ARGB32, false);
            _noiseTexture.wrapMode = TextureWrapMode.Clamp;
            _noiseTexture.filterMode = FilterMode.Point;

            _trashFrame1 = new RenderTexture(Screen.width, Screen.height, 0);
            _trashFrame2 = new RenderTexture(Screen.width, Screen.height, 0);
            _trashFrame1.hideFlags = HideFlags.DontSave;
            _trashFrame2.hideFlags = HideFlags.DontSave;

            Color32 color = XPostProcessingUtility.RandomColor();

            for (int y = 0; y < _noiseTexture.height; y++)
            {
                for (int x = 0; x < _noiseTexture.width; x++)
                {
                    //随机值若大于给定strip随机阈值，重新随机颜色
                    if (UnityEngine.Random.value > stripLength)
                    {
                        color = XPostProcessingUtility.RandomColor();
                    }
                    //设置贴图像素值
                    _noiseTexture.SetPixel(x, y, color);
                }
            }

            _noiseTexture.Apply();

            var bytes = _noiseTexture.EncodeToPNG();
        }




        public override void Render(PostProcessRenderContext context)
        {
            CommandBuffer cmd = context.command;
            PropertySheet sheet = context.propertySheets.Get(shader);
            cmd.BeginSample(PROFILER_TAG);

            UpdateNoiseTexture(settings.frequncy, settings.noiseTextureWidth,settings.noiseTextureHeight, settings.stripeLength);

            sheet.properties.SetFloat(ShaderIDs.indensity, settings.intensity);

            if (_noiseTexture != null)
            {
                sheet.properties.SetTexture(ShaderIDs.noiseTex, _noiseTexture);
            }

            if (settings.needStripColorAdjust == true)
            {
                sheet.EnableKeyword("NEED_TRASH_FRAME");
                sheet.properties.SetColor(ShaderIDs.StripColorAdjustColor, settings.StripColorAdjustColor);
                sheet.properties.SetFloat(ShaderIDs.StripColorAdjustIndensity, settings.StripColorAdjustIndensity);
            }
            else
            {
                sheet.DisableKeyword("NEED_TRASH_FRAME");
            }

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
            cmd.EndSample(PROFILER_TAG);
        }
    }
}


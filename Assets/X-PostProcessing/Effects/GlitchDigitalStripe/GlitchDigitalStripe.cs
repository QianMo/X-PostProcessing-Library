
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
using UnityEngine;
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
        [Range(0f, 9.9f)]
        public FloatParameter stripeLength = new FloatParameter { value = 8.9f };
        [Range(8, 64)]
        public IntParameter stripeWidth = new IntParameter { value = 20 };
    }

    public sealed class GlitchDigitalStripeRenderer : PostProcessEffectRenderer<GlitchDigitalStripe>
    {
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
            internal static readonly int trashTex = Shader.PropertyToID("_TrashTex");
        }



        void UpdateNoiseTexture(int frame, int stripeWidth, float stripLength)
        {
            int frameCount = Time.frameCount;
            if (frameCount % frame != 0)
            {
                return;
            }

            _noiseTexture = new Texture2D(64, stripeWidth, TextureFormat.ARGB32, false);
            _noiseTexture.wrapMode = TextureWrapMode.Clamp;
            _noiseTexture.filterMode = FilterMode.Point;

            _trashFrame1 = new RenderTexture(Screen.width, Screen.height, 0);
            _trashFrame2 = new RenderTexture(Screen.width, Screen.height, 0);
            _trashFrame1.hideFlags = HideFlags.DontSave;
            _trashFrame2.hideFlags = HideFlags.DontSave;

            var color = XPostProcessingUtility.RandomColor();

            for (var y = 0; y < _noiseTexture.height; y++)
            {
                for (var x = 0; x < _noiseTexture.width; x++)
                {
                    if (UnityEngine.Random.value > stripLength) color = XPostProcessingUtility.RandomColor();
                    _noiseTexture.SetPixel(x, y, color);
                }
            }

            _noiseTexture.Apply();
        }




        public override void Render(PostProcessRenderContext context)
        {

            UpdateNoiseTexture(settings.frequncy, settings.stripeWidth, settings.stripeLength * 0.1f);

            PropertySheet sheet = context.propertySheets.Get(shader);

            sheet.properties.SetFloat(ShaderIDs.indensity, settings.intensity);

            int frameCount = Time.frameCount;
            if (frameCount % 16 == 0)
            {
                context.command.BlitFullscreenTriangle(context.source, _trashFrame1);
            }
            if (frameCount % 56 == 0)
            {
                context.command.BlitFullscreenTriangle(context.source, _trashFrame2);
            }
            if (_noiseTexture != null)
            {
                sheet.properties.SetTexture(ShaderIDs.noiseTex, _noiseTexture);
            }

            RenderTexture trashFrame = UnityEngine.Random.value > 0.5f ? _trashFrame1 : _trashFrame2;
            if (trashFrame != null)
            {
                sheet.properties.SetTexture(ShaderIDs.trashTex, trashFrame);
            }

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}


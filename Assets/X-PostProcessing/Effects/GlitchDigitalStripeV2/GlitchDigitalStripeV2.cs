
//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License 
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;


namespace XPostProcessing
{

    [Serializable]
    [PostProcess(typeof(GlitchDigitalStripeV2Renderer), PostProcessEvent.AfterStack, "X-PostProcessing/Glitch/DigitalStripeV2")]
    public class GlitchDigitalStripeV2 : PostProcessEffectSettings
    {
        [Range(0f, 2f)]
        public FloatParameter speed = new FloatParameter { value = 0.25f };
        [Range(-1f, 1f)]
        public FloatParameter intensity = new FloatParameter { value = 0.5f };
        [Range(1f, 2f)]
        public FloatParameter resolutionMultiplier = new FloatParameter { value = 1f };

        [Range(0f, 1f)]
        public FloatParameter stretchMultiplier = new FloatParameter { value = 0.88f };
    }

    public sealed class GlitchDigitalStripeV2Renderer : PostProcessEffectRenderer<GlitchDigitalStripeV2>
    {

        private const string PROFILER_TAG = "X-GlitchDigitalStripeV2";
        private Shader shader;

        private float T;
        private float amount = 0;
        RenderTexture _trashFrame1;
        RenderTexture _trashFrame2;
        Texture2D _noiseTexture;
        RenderTexture trashFrame;

        public override void Init()
        {
            shader = Shader.Find("Hidden/X-PostProcessing/Glitch/DigitalStripeV2");
        }

        public override void Release()
        {
            base.Release();
        }

        static class ShaderIDs
        {
            internal static readonly int floatProperty1 = Shader.PropertyToID("_Float1");
            internal static readonly int floatProperty2 = Shader.PropertyToID("_Float2");
            internal static readonly int floatProperty3 = Shader.PropertyToID("_Float3");
            internal static readonly int colorProperty1 = Shader.PropertyToID("_Color1");
            internal static readonly int bumpMap = Shader.PropertyToID("_BumpMap");

            internal static readonly int Params = Shader.PropertyToID("_Params");
            internal static readonly int Params2 = Shader.PropertyToID("_Params2");
}

        public override void Render(PostProcessRenderContext context)
        {

            CommandBuffer cmd = context.command;
            PropertySheet sheet = context.propertySheets.Get(shader);
            cmd.BeginSample(PROFILER_TAG);


            if (_trashFrame1 != null || _trashFrame2 != null)
            {
                SetUpResources(settings.resolutionMultiplier);

            }
            if (UnityEngine.Random.value > Mathf.Lerp(0.9f, 0.5f, settings.speed))
            {
                SetUpResources(settings.resolutionMultiplier);
                UpdateNoiseTexture(settings.resolutionMultiplier);
            }

            // Update trash frames.
            int fcount = Time.frameCount;

            if (fcount % 13 == 0) context.command.BlitFullscreenTriangle(context.source, _trashFrame1);
            if (fcount % 73 == 0) context.command.BlitFullscreenTriangle(context.source, _trashFrame2);

            trashFrame = UnityEngine.Random.value > 0.5f ? _trashFrame1 : _trashFrame2;

            sheet.properties.SetFloat("_Intensity", amount);
            sheet.properties.SetFloat("_ColorIntensity", settings.intensity);

            if (_noiseTexture == null)
            {
                UpdateNoiseTexture(settings.resolutionMultiplier);
            }

            sheet.properties.SetTexture("_NoiseTex", _noiseTexture);
            if (trashFrame != null)
                sheet.properties.SetTexture("_TrashTex", trashFrame);


            cmd.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
            cmd.EndSample(PROFILER_TAG);
        }


        void SetUpResources(float g_2Res)
        {
            if (_trashFrame1 != null || _trashFrame2 != null)
            {
                return;
            }
            Vector2Int texVec = new Vector2Int((int)(g_2Res * 64), (int)(g_2Res * 62));
            _noiseTexture = new Texture2D(texVec.x, texVec.y, TextureFormat.ARGB32, false)
            {

                hideFlags = HideFlags.DontSave,
                wrapMode = TextureWrapMode.Clamp,
                filterMode = FilterMode.Point
            };

            _trashFrame1 = new RenderTexture(Screen.width, Screen.height, 0)
            {
                hideFlags = HideFlags.DontSave
            };
            _trashFrame2 = new RenderTexture(Screen.width, Screen.height, 0)
            {
                hideFlags = HideFlags.DontSave
            };

            UpdateNoiseTexture(g_2Res);
        }
        void UpdateNoiseTexture(float g_2Res)
        {
            Color color = RandomColor();
            if (_noiseTexture == null)
            {
                Vector2Int texVec = new Vector2Int((int)(g_2Res * 64), (int)(g_2Res * 32));
                _noiseTexture = new Texture2D(texVec.x, texVec.y, TextureFormat.ARGB32, false);
            }
            for (int y = 0; y < _noiseTexture.height; y++)
            {
                for (int x = 0; x < _noiseTexture.width; x++)
                {
                    if (UnityEngine.Random.value > settings.stretchMultiplier) color = RandomColor();
                    _noiseTexture.SetPixel(x, y, color);
                }
            }

            _noiseTexture.Apply();
        }
        static Color RandomColor()
        {
            return new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
        }
    }
}
        

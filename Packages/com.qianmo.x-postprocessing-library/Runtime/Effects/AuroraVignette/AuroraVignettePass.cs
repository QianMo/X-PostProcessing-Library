using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using System;

namespace XPL.Runtime
{

    class AuroraVignettePass : ScriptableRenderPass
    {
        private Material _mat;

        private float TimeX = 1.0f;

        RTHandle m_ColorHandle, m_CameraColorHandle;

        static class ShaderIDs
        {
            internal static readonly int mainTex = Shader.PropertyToID("_MainTex");
            internal static readonly int vignetteArea = Shader.PropertyToID("_VignetteArea");
            internal static readonly int vignetteSmothness = Shader.PropertyToID("_VignetteSmothness");
            internal static readonly int colorChange = Shader.PropertyToID("_ColorChange");
            internal static readonly int colorFactor = Shader.PropertyToID("_ColorFactor");
            internal static readonly int TimeX = Shader.PropertyToID("_TimeX");
            internal static readonly int vignetteFading = Shader.PropertyToID("_Fading");
        }

        public void Setup(Material material, RenderingData renderingData)
        {
            _mat = material;
            var colorCopyDescriptor = renderingData.cameraData.cameraTargetDescriptor;
            colorCopyDescriptor.depthBufferBits = (int)DepthBits.None;
            RenderingUtils.ReAllocateIfNeeded(ref m_ColorHandle, colorCopyDescriptor, FilterMode.Bilinear, TextureWrapMode.Clamp, name: "_AuroraVignettePassHandle");
        }

        internal void SetTarget(RTHandle cameraColorTargetHandle)
        {
            m_CameraColorHandle = cameraColorTargetHandle;
        }

        public void Dispose()
        {
            m_ColorHandle?.Release();
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            ConfigureTarget(m_ColorHandle);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            ExecutePass(ref renderingData, ref context);
        }

        private void ExecutePass(ref RenderingData renderingData, ref ScriptableRenderContext context)
        {
            if (_mat == null)
                return;

            if (renderingData.cameraData.isPreviewCamera)
                return;

            VolumeStack volumes = VolumeManager.instance.stack;
            AuroraVignette settings = volumes.GetComponent<AuroraVignette>();

            if (settings == null) return;
            if (settings.IsActive() == false) return;

            CommandBuffer cmd = CommandBufferPool.Get("X-Processing-Library");
            var cameraData = renderingData.cameraData;

            using (new ProfilingScope(cmd, new ProfilingSampler("AuroraVignetteRendererFeature")))
            {
                var source = cameraData.renderer.cameraColorTargetHandle;

                TimeX += Time.deltaTime;
                if (TimeX > 100)
                {
                    TimeX = 0;
                }

                _mat.SetFloat(ShaderIDs.vignetteArea, settings.vignetteArea.value);
                _mat.SetFloat(ShaderIDs.vignetteSmothness, settings.vignetteSmothness.value);
                _mat.SetFloat(ShaderIDs.colorChange, settings.colorChange.value * 10f);
                _mat.SetVector(ShaderIDs.colorFactor, new Vector3(settings.colorFactorR.value, settings.colorFactorG.value, settings.colorFactorB.value));
                _mat.SetFloat(ShaderIDs.TimeX, TimeX * settings.flowSpeed.value);
                _mat.SetFloat(ShaderIDs.vignetteFading, settings.intensity.value);
                _mat.SetTexture(ShaderIDs.mainTex, source);

                Blitter.BlitCameraTexture(cmd, source, m_ColorHandle, _mat, 0);
                Blitter.BlitCameraTexture(cmd, m_ColorHandle, m_CameraColorHandle);
            }

            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();
        }
    }
}

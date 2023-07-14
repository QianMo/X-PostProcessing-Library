using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace XPL.Runtime
{
    public class RadialBlurV1Pass : ScriptableRenderPass
    {
        private Material _mat;

        RTHandle m_ColorHandle, m_CameraColorHandle;

        static class ShaderIDs
        {
            internal static readonly int mainTex = Shader.PropertyToID("_MainTex");
            internal static readonly int Params = Shader.PropertyToID("_Params");
        }

        internal void Setup(Material material, RenderingData renderingData)
        {
            _mat = material;
            var colorCopyDescriptor = renderingData.cameraData.cameraTargetDescriptor;
            colorCopyDescriptor.depthBufferBits = (int)DepthBits.None;
            RenderingUtils.ReAllocateIfNeeded(ref m_ColorHandle, colorCopyDescriptor, FilterMode.Bilinear, TextureWrapMode.Clamp, name: "_RadialBlurV1PassHandle");
        }

        internal void SetTarget(RTHandle cameraColorTargetHandle, RTHandle cameraDepthTargetHandle)
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
            RadialBlurV1Settings settings = volumes.GetComponent<RadialBlurV1Settings>();

            if (settings == null) return;
            if (settings.IsActive() == false) return;

            CommandBuffer cmd = CommandBufferPool.Get("X-Processing-Library");
            var cameraData = renderingData.cameraData;

            using (new ProfilingScope(cmd, new ProfilingSampler("X-RadialBlurV1")))
            {
                var source = cameraData.renderer.cameraColorTargetHandle;

                _mat.SetVector(ShaderIDs.Params, new Vector4(settings.BlurRadius.value * 0.02f * settings.Intensity.value, settings.Iteration.value, settings.RadialCenterX.value, settings.RadialCenterY.value));
                _mat.SetTexture(ShaderIDs.mainTex, source);

                Blitter.BlitCameraTexture(cmd, source, m_ColorHandle, _mat, 0);
                Blitter.BlitCameraTexture(cmd, m_ColorHandle, m_CameraColorHandle);
            }

            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();
            CommandBufferPool.Release(cmd);
        }
    }
}

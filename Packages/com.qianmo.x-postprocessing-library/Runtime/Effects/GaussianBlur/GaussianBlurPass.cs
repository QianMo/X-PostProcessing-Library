using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using XPostProcessing;

namespace XPL.Runtime
{
    public class GaussianBlurPass : XPostProcessingPass
    {
        private Material _mat;

        RTHandle m_CameraColorHandle, m_BufferRT1, m_BufferRT2;

        static class ShaderIDs
        {
            internal static readonly int mainTex = Shader.PropertyToID("_MainTex");
            internal static readonly int BlurRadius = Shader.PropertyToID("_BlurOffset");
        }

        internal override void Setup(Material material, RenderingData renderingData)
        {
            base.Setup(material, renderingData);

            _mat = material;
            //RenderingUtils.ReAllocateIfNeeded(ref m_BufferRT2, colorCopyDescriptor, FilterMode.Bilinear, TextureWrapMode.Clamp, name: "_GaussianBlurPassHandle");

        }

        internal void SetTarget(RTHandle cameraColorTargetHandle, RTHandle cameraDepthTargetHandle)
        {
            m_CameraColorHandle = cameraColorTargetHandle;
        }

        public void Dispose()
        {
            m_BufferRT1?.Release();
            m_BufferRT2?.Release();
            m_BufferRT1 = null;
            m_BufferRT2 = null;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            ConfigureTarget(m_CameraColorHandle);
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
            GaussianBlurSettings settings = volumes.GetComponent<GaussianBlurSettings>();

            if (settings == null) return;
            if (settings.IsActive() == false) return;

            CommandBuffer cmd = CommandBufferPool.Get("X-Processing-Library");
            var cameraData = renderingData.cameraData;

            using (new ProfilingScope(cmd, new ProfilingSampler("X-GaussianBlur")))
            {
                var source = cameraData.renderer.cameraColorTargetHandle;

                int RTWidth = (int)(source.rt.width / settings.RTDownScaling.value);
                int RTHeight = (int)(source.rt.height / settings.RTDownScaling.value);

                //downsample
                var desc = GetCompatibleDescriptor(RTWidth, RTHeight, GraphicsFormat.B8G8R8A8_UNorm);
                RenderingUtils.ReAllocateIfNeeded(ref m_BufferRT1, desc, FilterMode.Bilinear, TextureWrapMode.Clamp, name: "BufferRT1");
                RenderingUtils.ReAllocateIfNeeded(ref m_BufferRT2, desc, FilterMode.Bilinear, TextureWrapMode.Clamp, name: "BufferRT2");

                _mat.SetTexture(ShaderIDs.mainTex, source);
                Blitter.BlitCameraTexture(cmd, source, m_BufferRT1);

                for (int i = 0; i < settings.Iteration.value; i++)
                {

                    // horizontal blur
                    _mat.SetVector(ShaderIDs.BlurRadius, new Vector4(settings.BlurRadius.value / source.rt.width, 0, 0, 0));
                    Blitter.BlitCameraTexture(cmd, m_BufferRT1, m_BufferRT2, _mat, 0);

                    // vertical blur
                    _mat.SetVector(ShaderIDs.BlurRadius, new Vector4(0, settings.BlurRadius.value / source.rt.height, 0, 0));
                    Blitter.BlitCameraTexture(cmd, m_BufferRT2, m_BufferRT1, _mat, 0);
                }

                Blitter.BlitCameraTexture(cmd, m_BufferRT1, m_CameraColorHandle, _mat, 1);
            }

            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();
            CommandBufferPool.Release(cmd);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using XPostProcessing;

namespace XPL.Runtime
{
    [DisallowMultipleRendererFeature("X Post Processing Library")]
    [Tooltip("X-PostProcessing Library (XPL) is a high quality post processing library for Unity Post Processing Stack.")]
    public class XPostProcessingRendererFeature : ScriptableRendererFeature
    {

        public RenderPassEvent InjectionPoint = RenderPassEvent.BeforeRenderingPostProcessing;

        private AuroraVignettePass m_AuroraVignettePass;

        private RadialBlurV1Pass m_RadialBlurV1Pass;

        private RadialBlurV2Pass m_RadialBlurV2Pass;

        private GaussianBlurPass m_GaussianBlurPass;

        private Material m_AuroraVignetteMaterial, m_RadialBlurV1Material, m_RadialBlurV2Material, m_GaussianBlurMaterial;

        private VolumeStack m_VolumeStack;

        public override void Create()
        {
            m_VolumeStack = VolumeManager.instance.stack;

            m_AuroraVignettePass = new AuroraVignettePass();
            m_AuroraVignettePass.renderPassEvent = InjectionPoint;
            m_AuroraVignettePass.ConfigureInput(ScriptableRenderPassInput.Color);

            m_RadialBlurV1Pass = new RadialBlurV1Pass();
            m_RadialBlurV1Pass.renderPassEvent = InjectionPoint;
            m_RadialBlurV1Pass.ConfigureInput(ScriptableRenderPassInput.Color);

            m_RadialBlurV2Pass = new RadialBlurV2Pass();
            m_RadialBlurV2Pass.renderPassEvent = InjectionPoint;
            m_RadialBlurV2Pass.ConfigureInput(ScriptableRenderPassInput.Color);

            m_GaussianBlurPass = new GaussianBlurPass();
            m_GaussianBlurPass.renderPassEvent = InjectionPoint;
            m_GaussianBlurPass.ConfigureInput(ScriptableRenderPassInput.Color);

        }

        protected override void Dispose(bool disposing)
        {
            if (m_AuroraVignetteMaterial)
                CoreUtils.Destroy(m_AuroraVignetteMaterial);
            m_AuroraVignettePass.Dispose();

            if (m_RadialBlurV1Material)
                CoreUtils.Destroy(m_RadialBlurV1Material);
            m_RadialBlurV1Pass.Dispose();

            if (m_RadialBlurV2Material)
                CoreUtils.Destroy(m_RadialBlurV2Material);
            m_RadialBlurV2Pass.Dispose();

            if (m_GaussianBlurMaterial)
                CoreUtils.Destroy(m_GaussianBlurMaterial);
            m_GaussianBlurPass.Dispose();
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            AuroraVignetteSetup(renderer, ref renderingData);
            RadialBlurV1Setup(renderer, ref renderingData);
            RadialBlurV2Setup(renderer, ref renderingData);
            //GaussianBlurSetup(renderer, ref renderingData);
        }

        public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
        {
            m_AuroraVignettePass.SetTarget(renderer.cameraColorTargetHandle, renderer.cameraDepthTargetHandle);
            m_RadialBlurV1Pass.SetTarget(renderer.cameraColorTargetHandle, renderer.cameraDepthTargetHandle);
            m_RadialBlurV2Pass.SetTarget(renderer.cameraColorTargetHandle, renderer.cameraDepthTargetHandle);
            m_GaussianBlurPass.SetTarget(renderer.cameraColorTargetHandle, renderer.cameraDepthTargetHandle);
        }

        private void AuroraVignetteSetup(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            AuroraVignetteSettings settings = m_VolumeStack.GetComponent<AuroraVignetteSettings>();

            if (settings == null || settings.IsActive() == false)
                return;

            m_AuroraVignetteMaterial = CoreUtils.CreateEngineMaterial("Hidden/X-PostProcessing/AuroraVignette");

            if (!m_AuroraVignetteMaterial)
            {
                Debug.LogWarningFormat("Missing Post Processing effect Material. {0} Fullscreen pass will not execute. Check for missing reference in the assigned renderer.", m_AuroraVignettePass.GetType().Name);
                return;
            }

            m_AuroraVignettePass.Setup(m_AuroraVignetteMaterial, renderingData);
            renderer.EnqueuePass(m_AuroraVignettePass);
        }

        private void RadialBlurV1Setup(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            RadialBlurV1Settings settings = m_VolumeStack.GetComponent<RadialBlurV1Settings>();

            if (settings == null || settings.IsActive() == false)
                return;

            m_RadialBlurV1Material = CoreUtils.CreateEngineMaterial("Hidden/X-PostProcessing/RadialBlurV1");

            if (!m_RadialBlurV1Material)
            {
                Debug.LogWarningFormat("Missing Post Processing effect Material. {0} Fullscreen pass will not execute. Check for missing reference in the assigned renderer.", m_RadialBlurV1Pass.GetType().Name);
                return;
            }

            m_RadialBlurV1Pass.Setup(m_RadialBlurV1Material, renderingData);
            renderer.EnqueuePass(m_RadialBlurV1Pass);
        }

        private void RadialBlurV2Setup(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            RadialBlurV2Settings settings = m_VolumeStack.GetComponent<RadialBlurV2Settings>();

            if (settings == null || settings.IsActive() == false)
                return;

            m_RadialBlurV2Material = CoreUtils.CreateEngineMaterial("Hidden/X-PostProcessing/RadialBlurV2");

            if (!m_RadialBlurV2Material)
            {
                Debug.LogWarningFormat("Missing Post Processing effect Material. {0} Fullscreen pass will not execute. Check for missing reference in the assigned renderer.", m_RadialBlurV2Pass.GetType().Name);
                return;
            }

            m_RadialBlurV2Pass.Setup(m_RadialBlurV2Material, renderingData);
            renderer.EnqueuePass(m_RadialBlurV2Pass);
        }

        private void GaussianBlurSetup(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            GaussianBlurSettings settings = m_VolumeStack.GetComponent<GaussianBlurSettings>();

            if (settings == null || settings.IsActive() == false)
                return;

            m_GaussianBlurMaterial = CoreUtils.CreateEngineMaterial("Hidden/X-PostProcessing/GaussianBlur");

            if (!m_GaussianBlurMaterial)
            {
                Debug.LogWarningFormat("Missing Post Processing effect Material. {0} Fullscreen pass will not execute. Check for missing reference in the assigned renderer.", m_GaussianBlurPass.GetType().Name);
                return;
            }

            m_GaussianBlurPass.Setup(m_GaussianBlurMaterial, renderingData);
            renderer.EnqueuePass(m_GaussianBlurPass);
        }
    }
}

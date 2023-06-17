using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using System;

namespace XPL.Runtime
{
    public class AuroraVignetteRendererFeature : ScriptableRendererFeature
    {
        public enum InjectionPoint
        {
            BeforeRenderingTransparents = RenderPassEvent.BeforeRenderingTransparents,
            BeforeRenderingPostProcessing = RenderPassEvent.BeforeRenderingPostProcessing,
            AfterRenderingPostProcessing = RenderPassEvent.AfterRenderingPostProcessing
        }

        private AuroraVignettePass m_AuroraVignettePass;

        public InjectionPoint injectionPoint = InjectionPoint.AfterRenderingPostProcessing;
        public ScriptableRenderPassInput requirements = ScriptableRenderPassInput.Color;
        private bool requiresColor;
        private bool injectedBeforeTransparents;

        private Material _mat;

        public override void Create()
        {
            _mat = CoreUtils.CreateEngineMaterial("Hidden/X-PostProcessing/AuroraVignette");
            m_AuroraVignettePass = new AuroraVignettePass();
            m_AuroraVignettePass.renderPassEvent = (RenderPassEvent)injectionPoint;
            ScriptableRenderPassInput modifiedRequirements = requirements;
            requiresColor = (requirements & ScriptableRenderPassInput.Color) != 0;
            injectedBeforeTransparents = injectionPoint <= InjectionPoint.BeforeRenderingTransparents;
            if (requiresColor && !injectedBeforeTransparents)
            {
                modifiedRequirements ^= ScriptableRenderPassInput.Color;
            }
            m_AuroraVignettePass.ConfigureInput(modifiedRequirements);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (_mat == null)
            {
                Debug.LogWarningFormat("Missing Post Processing effect Material. {0} Fullscreen pass will not execute. Check for missing reference in the assigned renderer.", GetType().Name);
                return;
            }
            m_AuroraVignettePass.Setup(_mat, renderingData);
            renderer.EnqueuePass(m_AuroraVignettePass);
        }

        public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
        {
            m_AuroraVignettePass.SetTarget(renderer.cameraColorTargetHandle);
        }

        protected override void Dispose(bool disposing)
        {
            CoreUtils.Destroy(_mat);
            m_AuroraVignettePass.Dispose();
        }
    }
}

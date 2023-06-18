using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace XPL.Runtime
{
    public abstract class XPostProcessingPass : ScriptableRenderPass
    {

        internal RenderTextureDescriptor Descriptor;

        internal virtual void Setup(Material material, RenderingData renderingData)
        {
            Descriptor = renderingData.cameraData.cameraTargetDescriptor;
            Descriptor.depthBufferBits = (int)DepthBits.None;
        }

        internal RenderTextureDescriptor GetCompatibleDescriptor()
        => GetCompatibleDescriptor(Descriptor.width, Descriptor.height, Descriptor.graphicsFormat);

        internal RenderTextureDescriptor GetCompatibleDescriptor(int width, int height, GraphicsFormat format, DepthBits depthBufferBits = DepthBits.None)
            => GetCompatibleDescriptor(Descriptor, width, height, format, depthBufferBits);

        internal static RenderTextureDescriptor GetCompatibleDescriptor(RenderTextureDescriptor desc, int width, int height, GraphicsFormat format, DepthBits depthBufferBits = DepthBits.None)
        {
            desc.depthBufferBits = (int)depthBufferBits;
            desc.msaaSamples = 1;
            desc.width = width;
            desc.height = height;
            desc.graphicsFormat = format;
            return desc;
        }
    }
}

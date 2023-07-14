
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
using UnityEngine.Rendering.Universal;


namespace XPL.Runtime
{

    [Serializable]
    [VolumeComponentMenuForRenderPipeline("X-PostProcessing/Blur/RadialBlur/RadialBlurV1", typeof(UniversalRenderPipeline))]
    public class RadialBlurV1Settings : VolumeComponent, IPostProcessComponent
    {
        public ClampedFloatParameter Intensity = new ClampedFloatParameter(0f, 0.0f, 1.0f);

        public ClampedIntParameter Iteration = new ClampedIntParameter(10, 1, 20);

        public ClampedFloatParameter BlurRadius = new ClampedFloatParameter(0.6f, -1f, 1f);

        public ClampedFloatParameter RadialCenterX = new ClampedFloatParameter(0.5f, 0f, 1f);

        public ClampedFloatParameter RadialCenterY = new ClampedFloatParameter(0.5f, 0f, 1f);

        public bool IsActive() => Intensity.value > 0;

        public bool IsTileCompatible() => true;

    }
}


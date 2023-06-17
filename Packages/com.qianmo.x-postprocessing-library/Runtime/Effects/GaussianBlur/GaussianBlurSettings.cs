
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

namespace XPostProcessing
{
    [VolumeComponentMenuForRenderPipeline("X-PostProcessing/Blur/GaussianBlur", typeof(UniversalRenderPipeline))]
    public class GaussianBlurSettings : VolumeComponent, IPostProcessComponent
    {

        public ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0.0f, 1.0f);

        [Range(0f, 5f)]
        public ClampedFloatParameter BlurRadius = new ClampedFloatParameter(3f, 0.0f, 5.0f);

        [Range(1, 15)]
        public ClampedFloatParameter Iteration = new ClampedFloatParameter(6f, 1.0f, 15.0f);

        [Range(1, 8)]
        public ClampedFloatParameter RTDownScaling = new ClampedFloatParameter(2f, 1.0f, 8.0f);

        public bool IsActive() => intensity.value > 0;

        public bool IsTileCompatible() => true;
    }
}
        

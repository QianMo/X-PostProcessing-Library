
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

        public ClampedFloatParameter BlurRadius = new ClampedFloatParameter(3f, 0.0f, 5.0f);

        public ClampedIntParameter Iteration = new ClampedIntParameter(6, 1, 15);

        public ClampedIntParameter RTDownScaling = new ClampedIntParameter(2, 1, 8);

        public bool IsActive() => intensity.value > 0;

        public bool IsTileCompatible() => true;
    }
}
        

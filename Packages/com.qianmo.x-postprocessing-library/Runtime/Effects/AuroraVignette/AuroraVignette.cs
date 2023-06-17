
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
    [VolumeComponentMenuForRenderPipeline("X-PostProcessing/Vignette/AuroraVignette", typeof(UniversalRenderPipeline))]
    public class AuroraVignette : VolumeComponent, IPostProcessComponent
    {
        public ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0.0f, 1.0f);

        public ClampedFloatParameter vignetteArea = new ClampedFloatParameter(0.8f, 0.0f, 1.0f);

        public ClampedFloatParameter vignetteSmothness = new ClampedFloatParameter(0.5f, 0.0f, 1.0f);

        public ClampedFloatParameter colorChange = new ClampedFloatParameter(0.1f, 0.1f, 1f);

        public ClampedFloatParameter colorFactorR = new ClampedFloatParameter(1f, 0.0f, 2.0f);

        public ClampedFloatParameter colorFactorG = new ClampedFloatParameter(1f, 0.0f, 2.0f);

        public ClampedFloatParameter colorFactorB = new ClampedFloatParameter(1f, 0.0f, 2.0f);

        public ClampedFloatParameter flowSpeed = new ClampedFloatParameter(1f, -2.0f, 2.0f);

        public bool IsActive() => intensity.value > 0;

        public bool IsTileCompatible() => true;

    }
}


//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License 
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XPostProcessing
{


    public enum Direction
    {
        Horizontal = 0,
        Vertical = 1,
    }

    public enum DirectionEX
    {
        Horizontal = 0,
        Vertical = 1,
        Horizontal_Vertical =2,
    }

    public enum IntervalType
    {
        Infinite,
        Periodic,
        Random
    }

}
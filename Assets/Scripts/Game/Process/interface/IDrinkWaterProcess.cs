using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gululu.Config;
using Hank.Data;
using Gululu.Util;
using strange.extensions.mediation.impl;

namespace Hank
{
    public interface IDrinkWaterProcess
    {
        void DrinkFail();

        void DrinkWater(int intake);
        void RefreshLevelAndExpView();
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LogicRoll.Domain.Battle;
using UnityEngine;

namespace LogicRoll.Presentation
{
    public class StrategySelectFactory : MonoBehaviour
    {
        [SerializeField] GameObject prefab;
        [SerializeField] Transform parent;

        public StrategySelectButtonDriver[] Create()
        {
            var strategyArray = (BattlerStrategy[])Enum.GetValues(typeof(BattlerStrategy));

            var viewDrivers = new StrategySelectButtonDriver[strategyArray.Length];

            for(int i = 0; i < strategyArray.Length; i++)
            {
                var instance = Instantiate(prefab, parent);

                var view = instance.GetComponent<StrategySelectButton>();

                var viewDriver = new StrategySelectButtonDriver(view, strategyArray[i]);

                viewDrivers[i] = viewDriver;
            }

            return viewDrivers;
        }
    }
}

using UnityEngine;
using LogicRoll.Domain.Battle;
using System;
using System.Collections.Generic;

namespace LogicRoll.Presentation
{
    public class FilterToggleFactory : MonoBehaviour
    {
        [SerializeField] GameObject sortButtonPrefab;
        [SerializeField] Transform parent;

        public BattlerFilterToggleDriver[] Create(IEnumerable<IFilterableByJob> filterables)
        {
            var jobs = (BattlerJob[])Enum.GetValues(typeof(BattlerJob));
            var views = new BattlerFilterToggleDriver[jobs.Length];

            for(int i = 0; i < jobs.Length; i++)
            {
                var instance = Instantiate(sortButtonPrefab, parent);

                var view = instance.GetComponent<BattlerFilterToggle>();

                var job = jobs[i];

                var viewDriver = new BattlerFilterToggleDriver(view, job, filterables);

                views[i] = viewDriver;
            }

            return views;
        }
    }
}

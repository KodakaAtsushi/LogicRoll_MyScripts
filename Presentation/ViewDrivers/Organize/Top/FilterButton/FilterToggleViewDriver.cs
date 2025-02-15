using System;
using System.Collections.Generic;
using log4net.DateFormatter;
using LogicRoll.Domain.Battle;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace LogicRoll.Presentation
{
    public class BattlerFilterToggleDriver : IFilterToggle
    {
        // view info
        BattlerFilterToggle view;
        
        static List<Func<BattlerJob, bool>> filters = new();
        Func<BattlerJob, bool> filter;

        Color activeColor = new Color(0.85f, 0.85f, 0.85f);
        Color inactiveColor = Color.gray;

        Toggle IFilterToggle.Toggle => view.Toggle;
        Observable<bool> IFilterToggle.OnClick => view.OnClick;

        public BattlerFilterToggleDriver(BattlerFilterToggle view, BattlerJob job, IEnumerable<IFilterableByJob> filterables)
        {
            view.Init();
            this.view = view;
            filter = arg => arg == job;

            view.OnClick.Subscribe(isOn =>
            {
                UpdateFilter(isOn);

                filterables.ForEach(f => 
                {
                    var isMatch = MatchAnyFilter(f.Job);
                    if(isMatch)
                    {
                        f.Show();
                    }
                    else
                    {
                        f.Hide();
                    }
                });
            }).AddTo(view);

            view.Toggle.isOn = true; // Prefabの初期値がfalseになっていることを確認
        }

        void UpdateFilter(bool isOn)
        {
            if(isOn)
            {
                filters.Add(filter);
                view.SetColor(activeColor);
            }
            else
            {
                filters.Remove(filter);
                view.SetColor(inactiveColor);
            }
        }

        bool MatchAnyFilter(BattlerJob job)
        {
            foreach(var filter in filters)
            {
                if(filter(job))
                {
                    return true;
                }
            }

            return false;
        }
    }
}

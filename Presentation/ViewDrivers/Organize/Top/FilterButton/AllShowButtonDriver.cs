using System.Collections.Generic;
using R3;
using UnityEngine;

namespace LogicRoll.Presentation
{
    public class AllShowButtonDriver
    {
        AllShowButton view;

        Color activeColor = new Color(0.85f, 0.85f, 0.85f);
        Color inactiveColor = Color.gray;

        public AllShowButtonDriver(AllShowButton view, IEnumerable<IFilterToggle> filterToggles)
        {
            view.Init();
            this.view = view;

            filterToggles.ForEach(ft => ft.OnClick.Subscribe(isOn =>
            {
                SetColor(HasAnyOff(filterToggles));
            }).AddTo(view));

            view.OnClick.Subscribe(_ =>
            {
                var hasAnyOff = HasAnyOff(filterToggles);
                SetColor(hasAnyOff);
                filterToggles.ForEach(ft => ft.Toggle.isOn = hasAnyOff);
            }).AddTo(view);

            view.SetColor(activeColor);
        }

        void SetColor(bool hasAnyOff)
        {
            if(hasAnyOff)
            {
                view.SetColor(inactiveColor);
            }
            else
            {
                view.SetColor(activeColor);
            }
        }

        bool HasAnyOff(IEnumerable<IFilterToggle> filterToggles) // 1つ以上offがあるか
        {
            foreach(var ft in filterToggles)
            {
                if(!ft.Toggle.isOn)
                {
                    return true;
                }
            }

            return false;
        }
    }
}

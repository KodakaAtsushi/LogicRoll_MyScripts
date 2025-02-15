using System.Collections;
using System.Collections.Generic;
using log4net.DateFormatter;
using LogicRoll.Application;
using R3;
using UnityEngine;

namespace LogicRoll.Presentation
{
    public class MemberCounterViewDriver : IMemberCountViewDriver
    {
        readonly MemberCounterView view;
        readonly int maxCount;

        public MemberCounterViewDriver(MemberCounterView view, int maxCount)
        {
            view.Init();
            this.view = view;
            this.maxCount = maxCount;
        }

        void IMemberCountViewDriver.UpdateCounter(int count)
        {
            view.SetCounterText($"{count} / {maxCount}");
        }

        void IMemberCountViewDriver.UpdateColor(bool canSave)
        {
            if(canSave)
            {
                view.SetCounterColor(Color.black);
                view.SetCountOverLog(false);
            }
            else
            {
                view.SetCounterColor(new Color(0.85f, 0, 0));
                view.SetCountOverLog(true);
            }
        }
    }
}

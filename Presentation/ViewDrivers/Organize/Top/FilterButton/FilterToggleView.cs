using System;
using System.Collections.Generic;
using LogicRoll.Domain.Battle;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace LogicRoll.Presentation
{
    public class BattlerFilterToggle : MonoBehaviour
    {
        public Toggle Toggle { get; private set; }
        Image image;

        // event
        public Observable<bool> OnClick => Toggle.onValueChanged.AsObservable();

        public void Init()
        {
            Toggle = GetComponent<Toggle>();
            image = GetComponent<Image>();
        }

        public void SetColor(Color color)
        {
            image.color = color;
        }
    }
}

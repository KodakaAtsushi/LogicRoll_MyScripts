using LogicRoll.Domain.Battle;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LogicRoll.Presentation
{
    public class StrategySelectButton : MonoBehaviour
    {
        Button button;
        TextMeshProUGUI strategyName;

        int strategyValue;

        public Observable<int> OnClick => button.OnClickAsObservable().Select(_=> strategyValue);

        public void Init(int strategyValue, string strategyName)
        {
            button = GetComponent<Button>();
            this.strategyName = GetComponentInChildren<TextMeshProUGUI>();
            this.strategyName.text = strategyName;

            this.strategyValue = strategyValue;
        }
    }
}

using LogicRoll.Domain.Battle;
using TMPro;
using UnityEngine;

namespace LogicRoll.Presentation
{
    public class StrategyInfoView : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI strategyName;
        [SerializeField] TextMeshProUGUI strategyInfo;

        public void Init()
        {
            // 初期化
        }

        public void UpdateText(string strategyName)
        {
            this.strategyName.text = strategyName;

            // strategyの詳細テキストが追加され次第実装
        }
    }
}

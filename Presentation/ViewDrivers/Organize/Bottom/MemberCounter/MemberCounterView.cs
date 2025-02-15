using log4net.DateFormatter;
using R3;
using TMPro;
using UnityEngine;

namespace LogicRoll.Presentation
{
    public class MemberCounterView : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI counterText;
        [SerializeField] TextMeshProUGUI countOverLog;

        public void Init()
        {
            
        }

        public void SetCounterText(string text)
        {
            counterText.text = text;
        }

        public void SetCountOverLog(bool isActive)
        {
            countOverLog.gameObject.SetActive(isActive);
        }

        public void SetCounterColor(Color color)
        {
            counterText.color = color;
        }
    }
}

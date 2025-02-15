using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LogicRoll.Presentation
{
    public class OrganizeSelectView : MonoBehaviour
    {
        public TMP_InputField inputField { get; private set; }
        [SerializeField] Button edit;
        [SerializeField] Button left;
        [SerializeField] Button right;

        public Observable<Unit> OnLeftClick => left.OnClickAsObservable();
        public Observable<Unit> OnRightClick => right.OnClickAsObservable();
        public Observable<string> OnValueChange => inputField.onValueChanged.AsObservable();

        public void Init()
        {
            inputField = GetComponentInChildren<TMP_InputField>();
        }

        public void SetTeamName(string text)
        {
            inputField.text = text;
        }
    }
}

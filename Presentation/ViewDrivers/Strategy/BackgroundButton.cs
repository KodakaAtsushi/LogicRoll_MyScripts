using LogicRoll.Application;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace LogicRoll.Presentation
{
    public class BackgroundButton : MonoBehaviour
    {
        [SerializeField] WindowName windowName;

        Button button;

        public Observable<Unit> OnClick => button.OnClickAsObservable();

        public void Init()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(() => WindowActivator.SetActive(windowName, false));
        }
    }
}

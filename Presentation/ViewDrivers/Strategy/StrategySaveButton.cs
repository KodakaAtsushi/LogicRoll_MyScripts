using R3;
using UnityEngine;
using UnityEngine.UI;

namespace LogicRoll.Presentation
{
    public class StrategySaveButton : MonoBehaviour
    {
        Button button;

        public Observable<Unit> OnClick => button.OnClickAsObservable();

        public void Init()
        {
            button = GetComponent<Button>();
        }
    }
}

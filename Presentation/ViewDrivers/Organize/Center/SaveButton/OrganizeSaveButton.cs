using R3;
using UnityEngine;
using UnityEngine.UI;

namespace LogicRoll.Presentation
{
    public class OrganizeSaveButton : MonoBehaviour
    {
        Button button;
        RawImage image;

        public bool IsEnable { set { button.enabled = value; } }

        public Observable<Unit> OnClick => button.OnClickAsObservable();

        public void Init()
        {
            button = GetComponent<Button>();
            image = GetComponent<RawImage>();
        }

        public void SetButtonColor(Color color)
        {
            image.color = color;
        }
    }
}

using R3;
using UnityEngine;
using UnityEngine.UI;

namespace LogicRoll.Presentation
{
    public class AllShowButton : MonoBehaviour
    {
        Button button;
        Image image;
        
        public Observable<Unit> OnClick => button.OnClickAsObservable();

        public void Init()
        {
            button = GetComponent<Button>();
            image = GetComponent<Image>();
        }

        public void SetColor(Color color)
        {
            image.color = color;
        }
    }
}

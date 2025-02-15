using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LogicRoll.Presentation
{
    public class AlertLogView : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI alertText;

        [SerializeField] BackgroundButton backgroundButton;

        [SerializeField] Button saveButton;
        [SerializeField] Button dontSaveButton;
        [SerializeField] Button cancelButton;

        public Observable<Unit> OnBackgroundClick => backgroundButton.OnClick;

        public Observable<Unit> OnSaveClick => saveButton.OnClickAsObservable();
        public Observable<Unit> OnDontSaveClick => dontSaveButton.OnClickAsObservable();
        public Observable<Unit> OnCancelClick=> cancelButton.OnClickAsObservable();
    }
}

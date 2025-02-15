using R3;
using R3.Triggers;
using UnityEngine;

namespace LogicRoll.Presentation
{
    public class Draggable : MonoBehaviour, IDraggable, IDeployable, IDroppable
    {
        public int MasterID { get; private set; }

        Transform defaultParent;

        ObservableEventTrigger trigger;
        public Observable<IDraggable> OnBeginDrag => trigger.OnBeginDragAsObservable().Select(_=>(IDraggable)this);
        public Observable<IDraggable> OnDrag => trigger.OnDragAsObservable().Select(_=>(IDraggable)this);
        public Observable<IDraggable> OnEndDrag => trigger.OnEndDragAsObservable().Select(_=>(IDraggable)this);

        public Observable<Draggable> OnPointerDown => trigger.OnPointerDownAsObservable().Select(_=>this);

        public void Init(int masterID)
        {
            MasterID = masterID;

            defaultParent = transform.parent;
            trigger = GetComponent<ObservableEventTrigger>();
        }

        void IDeployable.ResetParent()
        {
            transform.SetParent(defaultParent);
            transform.localPosition = Vector3.zero;
        }
    }
}
using System.Collections.Generic;
using UnityEngine;
using R3;
using UnityEngine.EventSystems;
using LogicRoll.Application;

namespace LogicRoll.Presentation
{
    public class DragAndDropper : MonoBehaviour, IDragAndDropper
    {
        Vector2 offset;
        Vector2 mousePos => Input.mousePosition;

        [SerializeField] Transform overlay;

        // events
        Observable<int> IDragAndDropper.OnPick => onPick;
        Subject<int> onPick = new();
        Observable<Vector2Int> IDragAndDropper.OnDrop => onDrop;
        Subject<Vector2Int> onDrop = new();
        Observable<Unit> IDragAndDropper.OnRelease => onRelease;
        Subject<Unit> onRelease = new();

        public void Init(IEnumerable<IDraggable> draggables)
        {
            foreach(var d in draggables)
            {
                d.OnBeginDrag.Subscribe(d => OnBeginDrag(d)).AddTo(this);
                d.OnDrag.Subscribe(d => OnDrag(d)).AddTo(this);
                d.OnEndDrag.Subscribe(d => OnEndDrag()).AddTo(this);
            }
        }

        // -----------------------------------

        void OnBeginDrag(IDraggable draggable)
        {
            offset = (Vector2)draggable.transform.position - mousePos;

            draggable.transform.SetParent(overlay);

            onPick.OnNext(draggable.MasterID);
        }

        void OnDrag(IDraggable draggable)
        {
            draggable.transform.position = mousePos + offset;
        }

        void OnEndDrag()
        {
            if(RayHit(out IDropPoint dropPoint))
            {
                // drop requestをするだけで、実際に配置するかはmodelが決める。
                // 配置が成功したら、その通知を受けてMemberDeployerクラスがが配置する。
                onDrop.OnNext(dropPoint.GridPos);
            }
            else
            {
                onRelease.OnNext(default);
            }
        }

        bool RayHit<T>(out T hitInfo)
        {
            var eventSystem = EventSystem.current;

            var data = new PointerEventData(eventSystem);
            data.position = mousePos;

            var results = new List<RaycastResult>();
            eventSystem.RaycastAll(data, results);

            foreach(var r in results)
            {
                var hit = r.gameObject.GetComponent<T>();
                if(hit != null)
                {
                    hitInfo = hit;
                    return true;
                }
            }

            hitInfo = default;
            return false;
        }
    }
}

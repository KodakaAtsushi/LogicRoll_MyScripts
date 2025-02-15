using LogicRoll.Application;
using R3;
using UnityEngine;

namespace LogicRoll.Presentation
{
    public class DropPoint : MonoBehaviour, IDropPoint
    {
        public Vector2Int GridPos { get; private set; }

        public void Init(Vector2Int gridPos)
        {
            GridPos = gridPos;
        }

        public void Droped(IDroppable droppable)
        {
            droppable.transform.SetParent(transform);
            droppable.transform.localPosition = new Vector2(0, 55);
        }
    }
}

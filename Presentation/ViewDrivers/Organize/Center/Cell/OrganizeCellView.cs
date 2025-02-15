using LogicRoll.Application;
using R3;
using UnityEngine;

namespace LogicRoll.Presentation
{
    public class OrganizeCellView : MonoBehaviour
    {
        public DropPoint DropPoint { get; private set; }

        public void Init(Vector2Int gridPos)
        {
            DropPoint = GetComponent<DropPoint>();
            DropPoint.Init(gridPos);
        }
    }
}

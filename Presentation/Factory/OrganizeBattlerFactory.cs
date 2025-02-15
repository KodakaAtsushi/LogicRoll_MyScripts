using LogicRoll.Application;
using UnityEngine;

namespace LogicRoll.Presentation
{
    public class OrganizeMemberFactory : MonoBehaviour
    {
        [SerializeField] GameObject prefab;
        [SerializeField] Transform parent;

        public (OrganizeMemberViewDriver[], IDraggable[]) Create(int[] masterIDs)
        {
            var battlers = new OrganizeMemberViewDriver[masterIDs.Length];
            var draggables = new IDraggable[masterIDs.Length];

            for(int i = 0; i < masterIDs.Length; i++)
            {
                var id = masterIDs[i];

                // Object生成
                var instance = Instantiate(prefab, parent);

                // viewDriver初期化
                var view = instance.GetComponent<OrganizeBattlerView>();
                var viewDriver = new OrganizeMemberViewDriver(view, id);

                battlers[i] = viewDriver;
                draggables[i] = instance.GetComponentInChildren<IDraggable>();
            }

            return (battlers, draggables);
        }
    }
}

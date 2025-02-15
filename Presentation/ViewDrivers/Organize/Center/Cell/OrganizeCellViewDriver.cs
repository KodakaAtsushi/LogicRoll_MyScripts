using LogicRoll.Application;
using R3;
using UnityEngine;

namespace LogicRoll.Presentation
{
    public class OrganizeCellViewDriver : IOrganizeCellViewDriver
    {
        OrganizeCellView view;

        public OrganizeCellViewDriver(OrganizeCellView view, Vector2Int gridPos)
        {
            view.Init(gridPos);
            this.view = view;
        }
    }
}
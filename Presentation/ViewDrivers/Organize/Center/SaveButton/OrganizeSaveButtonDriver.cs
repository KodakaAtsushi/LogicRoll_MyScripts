using LogicRoll.Application;
using R3;
using UnityEngine;

namespace LogicRoll.Presentation
{
    public class OrganizeSaveButtonDriver : IOrganizeSaveButtonDriver
    {
        OrganizeSaveButton view;

        Observable<Unit> IOrganizeSaveButtonDriver.OnSaveRequest => view.OnClick;

        public OrganizeSaveButtonDriver(OrganizeSaveButton view)
        {
            view.Init();
            this.view = view;
        }

        void IOrganizeSaveButtonDriver.SetEnable(bool isEnable)
        {
            view.IsEnable = isEnable;

            if(isEnable)
            {
                view.SetButtonColor(new Color(0.8f, 0.8f, 0.8f));
            }
            else
            {
                view.SetButtonColor(new Color(0.3f, 0.3f, 0.3f));
            }
        }
    }
}

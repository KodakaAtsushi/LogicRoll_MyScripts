using LogicRoll.Application;
using LogicRoll.Domain;
using R3;

namespace LogicRoll.Presentation
{
    public class MemberInfoViewDriver : IMemberInfoViewDriver
    {
        MemberInfoView view;

        public MemberInfoViewDriver(MemberInfoView view)
        {
            view.Init();
            this.view = view;

            view.OnClick.Subscribe(_ =>
            {
                WindowActivator.SetActive(WindowName.Strategy, true);
            }).AddTo(view);
        }

        void IMemberInfoViewDriver.UpdateInfo(OrganizeMember member)
        {
            if(!view.isActive) view.SetActive(true);

            view.UpdateInfo(member.MasterID, member.Strategy.ToString());
        }

        void IMemberInfoViewDriver.Close()
        {
            view.SetActive(false);
        }
    }
}

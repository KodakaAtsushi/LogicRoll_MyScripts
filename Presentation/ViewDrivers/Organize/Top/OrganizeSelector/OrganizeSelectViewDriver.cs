using LogicRoll.Application;
using R3;

namespace LogicRoll.Presentation
{
    public class OrganizeSelectViewDriver : IOrganizeSelectViewDriver
    {
        readonly OrganizeSelectView view;

        // events
        Observable<int> IOrganizeSelectViewDriver.OnOrganizeIncrement => onOrganizeIncrement;
        Subject<int> onOrganizeIncrement = new();
        Observable<string> IOrganizeSelectViewDriver.OnOrganizeNameChange => view.OnValueChange;

        public OrganizeSelectViewDriver(OrganizeSelectView view)
        {
            view.Init();
            this.view = view;

            view.OnRightClick.Subscribe(_ => 
            {
                onOrganizeIncrement.OnNext(1);
            }).AddTo(view);

            view.OnLeftClick.Subscribe(_ =>
            {
                onOrganizeIncrement.OnNext(-1);
            }).AddTo(view);
        }

        void IOrganizeSelectViewDriver.SetTeamName(string name)
        {
            view.SetTeamName(name);
        }
    }
}

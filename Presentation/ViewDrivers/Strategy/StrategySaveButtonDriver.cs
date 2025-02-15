using LogicRoll.Application;
using R3;

namespace LogicRoll.Presentation
{
    public class StrategySaveButtonDriver : IStrategySaveButtonDriver
    {
        readonly StrategySaveButton view;

        public Observable<Unit> OnSaveRequest => view.OnClick;

        public StrategySaveButtonDriver(StrategySaveButton view)
        {
            view.Init();
            this.view = view;

            view.OnClick.Subscribe(_ =>
            {
                WindowActivator.SetActive(WindowName.Strategy, false);
            }).AddTo(view);
        }
    }
}

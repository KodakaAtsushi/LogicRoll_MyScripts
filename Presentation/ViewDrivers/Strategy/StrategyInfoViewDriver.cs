using LogicRoll.Application;
using LogicRoll.Domain.Battle;

namespace LogicRoll.Presentation
{
    public class StrategyInfoViewDriver : IStrategyInfoViewDriver
    {
        readonly StrategyInfoView view;

        public StrategyInfoViewDriver(StrategyInfoView view)
        {
            view.Init();
            this.view = view;
        }

        public void OnStrategySelected(BattlerStrategy strategy)
        {
            view.UpdateText(strategy.ToString());
        }

        public void UpdateInfo(BattlerStrategy strategy)
        {
            view.UpdateText(strategy.ToString());
        }
    }
}

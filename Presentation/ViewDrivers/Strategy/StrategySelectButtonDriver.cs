using LogicRoll.Domain.Battle;
using R3;
using LogicRoll.Application;

namespace LogicRoll.Presentation
{
    public class StrategySelectButtonDriver : IStrategySelectButtonDriver
    {
        StrategySelectButton view;

        Observable<int> IStrategySelectButtonDriver.OnSelect
            => view.OnClick.Select(_=>strategyValue);

        int strategyValue;

        public StrategySelectButtonDriver(StrategySelectButton view, BattlerStrategy strategy)
        {
            view.Init((int)strategy, strategy.ToString());
            this.view = view;
            this.strategyValue = (int)strategy;
        }
    }
}

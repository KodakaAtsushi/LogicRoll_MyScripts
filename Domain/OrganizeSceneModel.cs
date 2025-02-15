using System;
using LogicRoll.Domain.Entity;
using R3;

namespace LogicRoll.Domain
{
    public class OrganizeSceneModel : IDisposable
    {
        public OrganizeEntity CurrentOrganize { get; private set; }
        int slotID => CurrentOrganize.SlotID;
        string teamName => CurrentOrganize.TeamName;

        public bool IsDirty => DeployModel.IsDirty || StrategySelectModel.IsDirty;

        readonly int organizeCount;

        // models
        public readonly MemberDeployModel DeployModel;
        public readonly StrategySelectModel StrategySelectModel;

        // events
        public Observable<string> OnOrganizeLoad => onOrganizeLoadSubject;
        Subject<string> onOrganizeLoadSubject = new();
        public ReactiveProperty<bool> CanSave { get; private set; } = new();
        CompositeDisposable disposables = new();

        public OrganizeSceneModel(MemberDeployModel deployModel, StrategySelectModel strategySelectModel, int organizeCount,
        int maxMemberCount)
        {
            DeployModel = deployModel;
            StrategySelectModel = strategySelectModel;

            this.organizeCount = organizeCount;

            deployModel.OnMemberCountChange.Subscribe(count =>
            {
                CanSave.Value = count <= maxMemberCount;
            }).AddTo(disposables);

            disposables.Add(deployModel);
        }

        public void LoadOrganize(OrganizeEntity organize)
        {
            CurrentOrganize = organize;
            DeployModel.Load(organize);
            StrategySelectModel.Load(organize.Members);

            onOrganizeLoadSubject.OnNext(CurrentOrganize.TeamName);
        }

        public OrganizeEntity SetTeamName(string name)
        {
            CurrentOrganize = CurrentOrganize.SetTeamName(name);
            return CurrentOrganize;
        }

        public OrganizeEntity CreateEntity()
        {
            // entityを生成
            (var members, var positions) = DeployModel.CreateEntity();

            return new OrganizeEntity(slotID, teamName, members, positions);
        }

        public int GetIncrementedSlotID(int increment)
        {
            return (slotID + increment + organizeCount) % organizeCount;
        }

        public void ResetDirtyFlag()
        {
            DeployModel.ResetDirtyFlag();
            StrategySelectModel.ResetDirtyFlag();
        }

        public void Dispose()
        {
            disposables.Dispose();
        }
    }
}

using Cysharp.Threading.Tasks;
using LogicRoll.Domain;
using R3;
using UnityEngine;

namespace LogicRoll.Application
{
    public class OrganizeSceneController : ControllerBase
    {
        readonly OrganizeSceneModel sceneModel;
        readonly IOrganizeDataRepository repository;

        public OrganizeSceneController(OrganizeSceneModel sceneModel, ViewContainer container,
        IDragAndDropper dragAndDropper, IMemberDeployer deployer)
        {
            this.sceneModel = sceneModel;
            repository = ServiceLocator.Get<IOrganizeDataRepository>();

            var deployModel = sceneModel.DeployModel;
            var strategySelectModel = sceneModel.StrategySelectModel;

            var members = container.GetAll<IOrganizeMemberViewDriver>();
            var cells = container.GetAll<IOrganizeCellViewDriver>();
            var strategySelectors = container.GetAll<IStrategySelectButtonDriver>();
            var organizeSelector = container.Get<IOrganizeSelectViewDriver>();
            var memberInfo = container.Get<IMemberInfoViewDriver>();
            var organizeSave = container.Get<IOrganizeSaveButtonDriver>();
            var counter = container.Get<IMemberCountViewDriver>();
            var strategyInfo = container.Get<IStrategyInfoViewDriver>();
            var strategySave = container.Get<IStrategySaveButtonDriver>();
            var alertLog = container.Get<IAlertLogViewDriver>();

            // View Inputs --------------------------------------

            // Organize Selector Input
            organizeSelector.OnOrganizeIncrement.Subscribe(async increment =>
            {
                if(sceneModel.IsDirty)
                {
                    var result = await alertLog.ShowAlertLog();

                    switch(result)
                    {
                        case AlertResult.Save:
                            Save();
                            Debug.Log("save");
                            break;

                        case AlertResult.Discard:
                            Debug.Log("discard");
                            break;

                        case AlertResult.Cancel:
                            Debug.Log("cancel");
                            return;

                        default:
                            throw new System.NotImplementedException();
                    }
                }

                Debug.Log("increment");
                IncrementOrganize(increment);

            }).AddTo(disposables);

            organizeSelector.OnOrganizeNameChange.Subscribe(name =>
            {
                var organize = sceneModel.SetTeamName(name);
                repository.SaveOrganizeData(organize);
            }).AddTo(disposables);

            // Organize Member Input
            foreach(var m in members)
            {
                m.OnSelect.Subscribe(id =>
                {
                    strategySelectModel.SelectMember(id);
                }).AddTo(disposables);
            }

            // Organize Save Input
            organizeSave.OnSaveRequest.Subscribe(_ =>
            {
                Save();
            }).AddTo(disposables);
            
            // Drag And Drop Input
            dragAndDropper.OnPick.Subscribe(id =>
            {
                deployModel.Pick(id);
            }).AddTo(disposables);

            dragAndDropper.OnDrop.Subscribe(gridPos =>
            {
                deployModel.Deploy(gridPos);
            }).AddTo(disposables);

            dragAndDropper.OnRelease.Subscribe(_ =>
            {
                deployModel.ReturnToPool();
            }).AddTo(disposables);

            // Strategy Select Input
            foreach(var selector in strategySelectors)
            {
                selector.OnSelect.Subscribe(value =>
                {
                    strategySelectModel.SelectStrategy(value);
                }).AddTo(disposables);
            }

            // Strategy Save Input
            strategySave.OnSaveRequest.Subscribe(_ =>
            {
                strategySelectModel.SaveStrategy();
            }).AddTo(disposables);


            // Model Outputs ---------------------------------------

            // Scene Modle Output
            sceneModel.OnOrganizeLoad.Subscribe(teamName =>
            {
                organizeSelector.SetTeamName(teamName);
                memberInfo.Close();
            }).AddTo(disposables);

            sceneModel.CanSave.Subscribe(canSave =>
            {
                organizeSave.SetEnable(canSave);
                counter.UpdateColor(canSave);
            }).AddTo(disposables);

            // Member Deploy Model Output
            deployModel.OnDeploy.Subscribe(data =>
            {
                (var id, var gridPos) = data;
                deployer.Drop(id, gridPos);
            }).AddTo(disposables);

            deployModel.OnReturn.Subscribe(id =>
            {
                deployer.Return(id);
            }).AddTo(disposables);
            
            deployModel.OnMemberCountChange.Subscribe(count =>
            {
                counter.UpdateCounter(count);
            }).AddTo(disposables);

            // Stratety Select Modle Output
            strategySelectModel.OnMemberSelect.Subscribe(member =>
            {
                memberInfo.UpdateInfo(member);
            }).AddTo(disposables);

            strategySelectModel.OnStrategySelect.Subscribe(strategy =>
            {
                strategyInfo.UpdateInfo(strategy);
            }).AddTo(disposables);
        }

        void Save()
        {
            var entity = sceneModel.CreateEntity();
            repository.SaveOrganizeData(entity);
            sceneModel.ResetDirtyFlag();
        }

        void IncrementOrganize(int increment)
        {
            var incrementedID = sceneModel.GetIncrementedSlotID(increment);
            var organize = repository.GetOrganizeDataByID(incrementedID);
            sceneModel.LoadOrganize(organize);
        }
    }

    public enum AlertResult
    {
        Save,
        Discard,
        Cancel
    }
}

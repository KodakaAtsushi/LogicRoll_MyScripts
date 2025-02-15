using System.Collections.Generic;
using System.Linq;
using LogicRoll.Application;
using LogicRoll.Domain;
using LogicRoll.Presentation;
using R3;
using Unity.VisualScripting;
using UnityEngine;

public class OrganizeMain : MonoBehaviour
{
    [Header("Factory")]
    [SerializeField] OrganizeMemberFactory memberFactory;
    [SerializeField] OrganizeCellFactory cellFactory;
    [SerializeField] FilterToggleFactory filterToggleFactory;
    [SerializeField] StrategySelectFactory strategySelectFactory;
    
    [Header("Views")]
    [SerializeField] AllShowButton allShowButton;
    [SerializeField] MemberCounterView memberCounterView;
    [SerializeField] OrganizeSelectView organizeSelectView;
    [SerializeField] MemberInfoView memberInfoView;
    [SerializeField] OrganizeSaveButton organizeSaveButton;
    [SerializeField] StrategyInfoView strategyInfoView;
    [SerializeField] StrategySaveButton strategySaveButton;
    [SerializeField] AlertLogView alertLogView;
    [SerializeField] List<BackgroundButton> backgroundButtons;

    [Header("Utility")]
    [SerializeField] DragAndDropper dragAndDropper;
    [SerializeField] WindowActivator windowActivator;

    [Header("Model Args")]
    [SerializeField] Vector2Int cellCount;

    int[] masterIDs = new int[]{1001, 1002, 1008, 1010, 1011, 1014};
    int organizeCount = 3;
    int maxMemberCount = 5;

    CompositeDisposable disposables = new();

    void Awake()
    {
        var organizeMembers = GetOrganizeMembers(masterIDs);
        
        // Models
        var pool = new OrganizeMemberPool(organizeMembers);
        var board = new OrganizeBoard(cellCount);
        var deployModel = new MemberDeployModel(pool, board);
        var strategySelectModel = new StrategySelectModel(organizeMembers); // memberPoolと同じ参照を渡す
        var sceneModel = new OrganizeSceneModel(deployModel, strategySelectModel, organizeCount, maxMemberCount);

        // Views
        (var members, var draggables) = memberFactory.Create(masterIDs);
        (var cells, var dropPoints) = cellFactory.Create(cellCount);
        var filterToggles = filterToggleFactory.Create(members);
        var strategySelectors = strategySelectFactory.Create();

        new AllShowButtonDriver(allShowButton, filterToggles);
        var organizeSelect = new OrganizeSelectViewDriver(organizeSelectView);
        var memberInfo = new MemberInfoViewDriver(memberInfoView);
        var organizeSave = new OrganizeSaveButtonDriver(organizeSaveButton);
        var memberCounter = new MemberCounterViewDriver(memberCounterView, maxMemberCount);
        var strategyInfo = new StrategyInfoViewDriver(strategyInfoView);
        var strategySave = new StrategySaveButtonDriver(strategySaveButton);
        var alertLog = new AlertLogViewDriver(alertLogView);

        foreach(var button in backgroundButtons) { button.Init(); }

        // Register in the container
        var container = new ViewContainer();
        container.RegisterAll<IOrganizeMemberViewDriver>(members);
        container.RegisterAll<IOrganizeCellViewDriver>(cells);
        container.RegisterAll<IStrategySelectButtonDriver>(strategySelectors);
        container.Register<IOrganizeSelectViewDriver>(organizeSelect);
        container.Register<IMemberInfoViewDriver>(memberInfo);
        container.Register<IOrganizeSaveButtonDriver>(organizeSave);
        container.Register<IMemberCountViewDriver>(memberCounter);
        container.Register<IStrategyInfoViewDriver>(strategyInfo);
        container.Register<IStrategySaveButtonDriver>(strategySave);
        container.Register<IAlertLogViewDriver>(alertLog);

        // Utilitys
        dragAndDropper.Init(draggables);
        windowActivator.Init();
        var deployer = new MemberDeployer(draggables.Cast<IDeployable>(), dropPoints);

        // Controller
        disposables.Add(new OrganizeSceneController(sceneModel, container, dragAndDropper, deployer));

        // First Load
        var organize = ServiceLocator.Get<IOrganizeDataRepository>().GetOrganizeDataByID(0);
        sceneModel.LoadOrganize(organize);
    }

    OrganizeMember[] GetOrganizeMembers(params int[] ids)
    {
        var members = new OrganizeMember[ids.Length];

        for(int i = 0; i < ids.Length; i++)
        {
            members[i] = new OrganizeMember(ids[i], 0);
        }

        return members;
    }

    void OnDestroy()
    {
        disposables.Dispose();
    }
}

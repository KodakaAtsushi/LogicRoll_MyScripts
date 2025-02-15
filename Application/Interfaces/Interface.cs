using System.Threading.Tasks;
using LogicRoll.Domain;
using LogicRoll.Domain.Battle;
using R3;
using UnityEngine;

namespace LogicRoll.Application
{
    // View drivers
    public interface IOrganizeSelectViewDriver
    {
        Observable<int> OnOrganizeIncrement { get; }
        Observable<string> OnOrganizeNameChange { get; }

        void SetTeamName(string name);
    }

    public interface IOrganizeMemberViewDriver
    {
        Observable<int> OnSelect { get; }
    }

    public interface IOrganizeCellViewDriver
    {

    }

    public interface IStrategySelectButtonDriver
    {
        Observable<int> OnSelect { get; }
    }

    public interface IMemberCountViewDriver
    {
        void UpdateCounter(int count);
        void UpdateColor(bool canSave);
    }

    public interface IMemberInfoViewDriver
    {
        void UpdateInfo(OrganizeMember member);
        void Close();
    }

    public interface IOrganizeSaveButtonDriver
    {
        Observable<Unit> OnSaveRequest { get; }

        void SetEnable(bool canSave);
    }

    public interface IStrategyInfoViewDriver
    {
        void UpdateInfo(BattlerStrategy strategy);
    }

    public interface IStrategySaveButtonDriver
    {
        Observable<Unit> OnSaveRequest { get; }
    }

    public interface IAlertLogViewDriver
    {
        Task<AlertResult> ShowAlertLog();
    }

    // Utilitys
    public interface IDragAndDropper
    {
        Observable<int> OnPick { get; }
        Observable<Vector2Int> OnDrop { get; }
        Observable<Unit> OnRelease { get; }
    }

    public interface IMemberDeployer
    {
        void Drop(int masterID, Vector2 gridPos);
        void Return(int masterID);
    }
}

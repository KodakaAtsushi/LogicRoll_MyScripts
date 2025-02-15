using System.Threading.Tasks;
using LogicRoll.Application;
using R3;
using UnityEngine;

namespace LogicRoll.Presentation
{
    public class AlertLogViewDriver : IAlertLogViewDriver
    {
        AlertLogView view;

        CompositeDisposable disposables = new();

        public AlertLogViewDriver(AlertLogView view)
        {
            this.view = view;
        }

        async Task<AlertResult> IAlertLogViewDriver.ShowAlertLog()
        {
            WindowActivator.SetActive(WindowName.Alert, true);

            var result = await AddListeners();
            
            disposables.Clear();
            WindowActivator.SetActive(WindowName.Alert, false);

            return result;
        }

        Task<AlertResult> AddListeners()
        {
            var tcs = new TaskCompletionSource<AlertResult>();

            view.OnSaveClick.Subscribe(_ =>
            {
                tcs.TrySetResult(AlertResult.Save);
            }).AddTo(disposables);

            view.OnDontSaveClick.Subscribe(_ =>
            {
                tcs.TrySetResult(AlertResult.Discard);
            }).AddTo(disposables);

            view.OnCancelClick.Subscribe(_ =>
            {
                tcs.TrySetResult(AlertResult.Cancel);
            }).AddTo(disposables);

            view.OnBackgroundClick.Subscribe(_ =>
            {
                tcs.TrySetResult(AlertResult.Cancel);
            }).AddTo(disposables);

            return tcs.Task;
        }
    }
}

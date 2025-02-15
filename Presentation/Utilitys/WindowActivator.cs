using System.Collections.Generic;
using System.Linq;
using LogicRoll.Application;
using UnityEngine;

namespace LogicRoll.Presentation
{
    public class WindowActivator : MonoBehaviour
    {
        [SerializeField] List<Window> windowsList;

        static List<Window> windows;

        public void Init()
        {
            windows = windowsList;
        }

        public static void SetActive(WindowName windowName, bool isActive)
        {
            var window = windows.First(w => w.Name == windowName);

            window.Object.SetActive(isActive);
        }
    }

    [System.Serializable]
    public struct Window
    {
        public WindowName Name;
        public GameObject Object;
    }

    [System.Serializable]
    public enum WindowName
    {
        Organize,
        Strategy,
        Alert
    }
}

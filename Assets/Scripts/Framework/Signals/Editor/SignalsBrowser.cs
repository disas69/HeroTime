using UnityEditor;
using UnityEngine;

namespace Framework.Signals.Editor
{
    public class SignalsBrowser : EditorWindow
    {
        //[MenuItem()]
        public static void Open()
        {
            var window = EditorWindow.GetWindow<SignalsBrowser>("Signals Browser");
            window.minSize = new Vector2(650f, 450f);
            window.Show();
        }
    }
}
using System;
using UnityEngine;

namespace Game.Tools
{
    public class GraphicsSettingsSetter : MonoBehaviour
    {
        public int FrameRate = 60;
        public string QualityLevel = "Medium";
        public bool VSync = false;

        private void Awake()
        {
            Application.targetFrameRate = FrameRate;

            var index = Array.IndexOf(QualitySettings.names, QualityLevel);
            if (index >= 0)
            {
                QualitySettings.SetQualityLevel(index);
            }

            QualitySettings.vSyncCount = VSync ? 1 : 0;
        }
    }
}
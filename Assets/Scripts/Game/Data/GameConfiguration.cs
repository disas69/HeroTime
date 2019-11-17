using System.Collections.Generic;
using Framework.Attributes;
using Framework.Tools.Singleton;
using Game.Characters;
using Game.Levels;
using UnityEngine;

namespace Game.Data
{
    [ResourcePath("Configuration/GameConfiguration")]
    [CreateAssetMenu(fileName = "GameConfiguration", menuName = "Game/GameConfiguration")]
    public class GameConfiguration : ScriptableSingleton<GameConfiguration>
    {
        public PlayerSettings Player;
        public List<LevelConfiguration> Levels;

        public static PlayerSettings PlayerSettings => Instance.Player;

        public static LevelConfiguration GetLevelConfiguration(int level)
        {
            return Instance.Levels.Find(l => l.Level == level);
        }
    }
}
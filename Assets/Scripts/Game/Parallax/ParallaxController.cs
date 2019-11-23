using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Tools.Singleton;

namespace Assets.Scripts.Game.Parallax
{
    public sealed class ParallaxController : MonoSingleton<ParallaxController>
    {
        private readonly List<ParallaxBehavior> _parallaxBehaviors = new List<ParallaxBehavior>();

        public void AddBehavior(ParallaxBehavior behavior)
        {
            if (_parallaxBehaviors.Contains(behavior))
            {
                return;
            }

            _parallaxBehaviors.Add(behavior);
        }

        public void RemoveBehavior(ParallaxBehavior behavior)
        {
            _parallaxBehaviors.Remove(behavior);
        }

        public void Update()
        {

        }
    }
}

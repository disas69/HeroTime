using System.Collections;
using Framework.UI.Structure.Base.Model;
using Framework.UI.Structure.Base.View;
using UnityEngine;

namespace Game.UI
{
    public class EndPage : Page<PageModel>
    {
        public float WaitTime;
        
        public override void OnEnter()
        {
            base.OnEnter();
            StartCoroutine(GoToStart());
        }

        private IEnumerator GoToStart()
        {
            yield return new WaitForSecondsRealtime(WaitTime);
            GameController.Instance.SetState(GameState.Start);
        }
    }
}
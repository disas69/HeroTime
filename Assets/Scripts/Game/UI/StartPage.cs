using Framework.UI.Structure.Base.Model;
using Framework.UI.Structure.Base.View;
using UnityEngine;

namespace Game.UI
{
    public class StartPage : Page<PageModel>
    {
        public void Exit()
        {
            Application.Quit();
        }
    }
}
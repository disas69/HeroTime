using Framework.Signals.ParameterProviders;

namespace Game.Tools
{
    public class GameStateParameterProvider : IntParameterProvider
    {
        public GameState GameState;

        public override int GetValue()
        {
            return (int) GameState;
        }
    }
}
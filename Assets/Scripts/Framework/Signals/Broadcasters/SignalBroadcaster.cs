using Framework.Signals.ParameterProviders;
using UnityEngine;

namespace Framework.Signals.Broadcasters
{
    public class SignalBroadcaster : MonoBehaviour
    {
        public Signal Signal;

        public void Broadcast()
        {
            SignalsManager.Broadcast(Signal.Name);
        }
    }

    public abstract class SignalBroadcaster<TParameter, TParameterProvider> : MonoBehaviour
        where TParameterProvider : ParameterProvider<TParameter>
    {
        public Signal Signal;
        [HideInInspector] public bool IsParameterProvided;
        [HideInInspector] public TParameter Parameter;
        [HideInInspector] public TParameterProvider Provider;

        public abstract void Broadcast();
        public abstract void Broadcast(TParameter parameter);
        public abstract void Broadcast(TParameterProvider parameterProvider);
    }
}
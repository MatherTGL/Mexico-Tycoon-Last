using System.Collections;
using Boot;
using UnityEngine;

namespace GameSystem
{
    public sealed class CoroutineManager : MonoBehaviour, IBoot
    {
        public static CoroutineManager Instance { get; private set; }


        void IBoot.InitAwake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        void IBoot.InitStart() { }

        (Bootstrap.TypeLoadObject typeLoad, Bootstrap.TypeSingleOrLotsOf singleOrLotsOf) IBoot.GetTypeLoad()
            => (Bootstrap.TypeLoadObject.SuperImportant, Bootstrap.TypeSingleOrLotsOf.Single);

        public Coroutine StartManagedCoroutine(IEnumerator coroutine)
            => StartCoroutine(coroutine);

        public void StopManagedCoroutine(Coroutine coroutine)
            => StopCoroutine(coroutine);

        public IEnumerator WaitAndExecute(float waitTime, System.Action callback)
        {
            yield return new WaitForSeconds(waitTime);
            callback?.Invoke();
        }
    }
}

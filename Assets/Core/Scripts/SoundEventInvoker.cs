using UnityEngine;
using UnityEngine.Events;

public class SoundEventInvoker : MonoBehaviour
{
    [Tooltip("Подпишитесь в инспекторе на AudioPlayer.PlaySound() или другой метод")]
    public UnityEvent OnPlaySound;

    // Вызывается в коде в нужный момент:
    public void InvokePlay()
    {
        OnPlaySound?.Invoke();
    }

    // Для удобства: публичный метод, который можно вызвать из других скриптов
    public void InvokePlayDelayed(float delay)
    {
        StartCoroutine(InvokeDelayed(delay));
    }

    private System.Collections.IEnumerator InvokeDelayed(float t)
    {
        yield return new WaitForSeconds(t);
        OnPlaySound?.Invoke();
    }
}

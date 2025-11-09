using UnityEngine;
using UnityEngine.Events;

public class RockManager : MonoBehaviour
{
    public UnityEvent OnSolve;
    [SerializeField] private RockTrigger[] _rocks;

    public void CheckRocksStatus()
    {
        if (_rocks[0].IsRightRock == true && _rocks[1].IsRightRock == true && _rocks[2].IsRightRock == true)
        {
            Debug.Log("AllRIGHT");
            OnSolve.Invoke();
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerStatus : MonoBehaviour
{
    public bool IsMoving = false;
    public bool IsCrouching = false;
}

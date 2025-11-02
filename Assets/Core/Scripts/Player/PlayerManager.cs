using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerInteraction _playerInteraction;
    [SerializeField] private Flashlight _flashLight;

    // Простые методы для блокировки всего
    public void BlockAll()
    {
        _playerMovement.PlayerMovementBlock();
        _playerMovement.CameraBlock();
        _playerMovement.mouseUnlock();
        _playerInteraction.BlockRay();
        _flashLight.BlockFLashlight();
    }

    public void UnblockAll()
    {
        _playerMovement.PlayerMovementUnblock();
        _playerMovement.CameraUnblock();
        _playerMovement.mouseblock();
        _playerInteraction.UnblockRay();
        _flashLight.UnblockFLashlight();
    }
}
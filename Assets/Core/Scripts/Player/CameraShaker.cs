using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerMovement _playerMovement;

    [Header("Bobbing Settings")]
    [SerializeField] private float _bobbingSpeed = 14f;
    [SerializeField] private float _bobbingAmount = 0.05f;
    [SerializeField] private float _verticalRatio = 1f;
    [SerializeField] private float _horizontalRatio = 0.5f;
    [SerializeField] private float _returnSpeed = 5f;

    private Vector3 _originalLocalPos;
    private float _timer;
    private bool _wasMoving;

    private void Start()
    {
        _originalLocalPos = transform.localPosition;
    }

    private void LateUpdate()
    {
        bool isMoving = _playerMovement.IsMoving && _playerMovement.IsGrounded;

        if (isMoving)
        {
            HandleBobbing();
            _wasMoving = true;
        }
        else if (_wasMoving)
        {
            HandleReturn();
        }
    }

    private void HandleBobbing()
    {
        _timer += Time.deltaTime * _bobbingSpeed;

        Vector3 offset = CalculateBobbingOffset(_timer);
        transform.localPosition = _originalLocalPos + offset;
    }

    private void HandleReturn()
    {
        _timer = 0;
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            _originalLocalPos,
            Time.deltaTime * _returnSpeed
        );

        // —брасываем флаг когда достаточно близко к исходной позиции
        if (Vector3.Distance(transform.localPosition, _originalLocalPos) < 0.001f)
        {
            transform.localPosition = _originalLocalPos;
            _wasMoving = false;
        }
    }

    private Vector3 CalculateBobbingOffset(float time)
    {
        float vertical = Mathf.Sin(time) * _bobbingAmount * _verticalRatio;
        float horizontal = Mathf.Cos(time * 0.5f) * _bobbingAmount * _horizontalRatio;
        return new Vector3(horizontal, vertical, 0);
    }
}

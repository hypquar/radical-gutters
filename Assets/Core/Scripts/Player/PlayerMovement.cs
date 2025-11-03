using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform _playerVirtualCamera;
    CharacterController controller;
    [Range(1, 20)][SerializeField] private float _SpeedWalk = 3.0f;
    [SerializeField] private float _SpeedCurrent;
    [Range(1, 30)][SerializeField] private float _SpeedRun = 7.0f;
    [Range(0, 20)][SerializeField] private float _JumpPower = 10f;
    [SerializeField] private float _gravity = -13f;
    [SerializeField] private float _velocity;
    [Range(1f, 3f)] public float SmoothSpeed = 0.3f;
    private float _MousePOS_y = 0;
    [SerializeField] private float _MouseSensivity = 3.0f;
    public bool _IsPlayerCanMove = true;
    public bool _IsCameraCanMove = true;

    private void Start()
    {
        mouseblock();
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (_IsPlayerCanMove) Move();
        if( _IsCameraCanMove) Camera();
    }
    public void CameraBlock()
    {
        Debug.Log("Управление Основной камерой отключилось");
        _IsCameraCanMove = false;
    }
    public void CameraUnblock()
    {
        Debug.Log("Управление Основной камерой включилось");
        _IsCameraCanMove = true;
    }
    public void PlayerMovementBlock()
    {
        Debug.Log("Передвижение игрока заблокировано");
        _IsPlayerCanMove = false;
    }
    public void PlayerMovementUnblock()
    {
        Debug.Log("Передвижение игрока разблокировано");
        _IsPlayerCanMove = true;
    }
    public void mouseblock()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void mouseUnlock()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    private void Move()
    {
        Vector2 MoveDirection = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));

        // Гравитация 
        if (controller.isGrounded)
        {
            _velocity = 0.0f;

            // Прыжок
            if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded == true) _velocity = _JumpPower;
        }
        _velocity += _gravity * Time.deltaTime;
        // приседание
        if (Input.GetKey(KeyCode.LeftControl))
        {
            controller.height = 0.3f;
            _SpeedCurrent = 2.0f;
        }
        else controller.height = 1.8f;

        // Бег
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl) == false) _SpeedCurrent = Mathf.Lerp(_SpeedCurrent, _SpeedRun, Time.deltaTime * SmoothSpeed);
        else _SpeedCurrent = Mathf.Lerp(_SpeedCurrent, _SpeedWalk, Time.deltaTime * SmoothSpeed);

        Vector3 POS = (transform.forward * MoveDirection.x + transform.right * MoveDirection.y) * _SpeedCurrent + Vector3.up * _velocity;
        controller.Move(POS * Time.deltaTime);
    }
    private void Camera()
    {
        Vector2 cam = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        transform.Rotate(Vector3.up * cam.x * _MouseSensivity);
        _MousePOS_y -= cam.y * _MouseSensivity;
        _MousePOS_y = Mathf.Clamp(_MousePOS_y, -90f, 90f);
        _playerVirtualCamera.localEulerAngles = Vector3.right * _MousePOS_y;

    }
}

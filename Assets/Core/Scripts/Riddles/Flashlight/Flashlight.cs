using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Flashlight : MonoBehaviour
{
    [Header("Components")]
    [SerializeField ]private GameObject _flashlight;
    [SerializeField] private Slider _slider;
    [SerializeField] private Light _violet;
    [SerializeField] private Light _normal;

    [Header("Battery Settings")]
    [SerializeField] private int _maxBattery = 100;
    [SerializeField] private float _dischargeRate = 10f;
    [SerializeField] private float _chargeRate = 5f;
    [SerializeField] private float _chargeCooldown = 5f; // Кулдаун перед зарядкой

    [Header("Current State")]
    private bool _isOn = false;
    private bool _isUltraViolet = false;
    private int _battery = 100;
    private bool _isCooldown = false; // Флаг кулдауна

    private bool _isBlock = false;
    private Coroutine _batteryCoroutine;
    private Coroutine _cooldownCoroutine;

    private void Start()
    {
        UpdateFlashlightState();
        _slider.maxValue = _maxBattery;
    }

    private void Update()
    {
        _slider.value = _battery;
        if (!_isBlock) HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ToggleFlashlight();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            ToggleUltraViolet();
        }
    }

    private void ToggleFlashlight()
    {
        _isOn = !_isOn;

        // При выключении фонарика сбрасываем УФ режим
        if (!_isOn && _isUltraViolet)
        {
            _violet.transform.localEulerAngles = new Vector3(-180, 0, 0);
            _isUltraViolet = false;
        }

        UpdateFlashlightState();
        UpdateBatteryLogic();
    }

    private void ToggleUltraViolet()
    {
        if (!_isOn) return;

        // Если пытаемся включить УФ во время кулдауна или с пустой батареей - блокируем
        if (!_isUltraViolet && (_isCooldown || _battery <= 0))
        {
            Debug.Log("Батарея разряжена! Зарядка на кулдауне");
            return;
        }

        _isUltraViolet = !_isUltraViolet;
        UpdateFlashlightState();
        UpdateBatteryLogic();
    }

    private void UpdateFlashlightState()
    {
        _flashlight.SetActive(_isOn);

        if (_isOn)
        {
            if (_isUltraViolet)
            {
                _violet.transform.localEulerAngles = new Vector3(0, 0, 0);
                _violet.enabled = true;
                _normal.enabled = false;
            }
            else
            {
                _violet.transform.localEulerAngles = new Vector3(-180, 0, 0);
                _violet.enabled = false;
                _normal.enabled = true;
            }
        }
        else
        {
            // Гарантируем, что при выключенном фонарике УФ тоже выключен
            _isUltraViolet = false;
        }

        Debug.Log($"Фонарик: Вкл={_isOn}, УФ={_isUltraViolet}, Батарея={_battery}%, Кулдаун={_isCooldown}");
    }

    private void UpdateBatteryLogic()
    {
        // Останавливаем предыдущую корутину
        if (_batteryCoroutine != null)
        {
            StopCoroutine(_batteryCoroutine);
            _batteryCoroutine = null;
        }

        // Запускаем новую корутину в зависимости от состояния
        if (_isUltraViolet && _isOn && _battery > 0)
        {
            _batteryCoroutine = StartCoroutine(DischargeRoutine());
        }
        else if (!_isUltraViolet && !_isCooldown && _battery < _maxBattery)
        {
            _batteryCoroutine = StartCoroutine(ChargeRoutine());
        }
    }

    private IEnumerator DischargeRoutine()
    {
        Debug.Log("Началась разрядка батареи УФ-режима");

        while (_isUltraViolet && _isOn && _battery > 0)
        {
            yield return new WaitForSeconds(1f);
            _battery = Mathf.Max(0, _battery - Mathf.RoundToInt(_dischargeRate));
            Debug.Log($"Батарея разряжается: {_battery}%");

            if (_battery <= 0)
            {
                _battery = 0;
                _isUltraViolet = false;
                UpdateFlashlightState();
                Debug.Log("Батарея разряжена! УФ-режим отключен");

                // Запускаем кулдаун перед началом зарядки
                StartCooldown();
                yield break;
            }
        }

        Debug.Log("Разрядка батареи остановлена");
    }

    private IEnumerator ChargeRoutine()
    {
        Debug.Log("Началась зарядка батареи УФ-режима");

        while (!_isUltraViolet && !_isCooldown && _battery < _maxBattery)
        {
            yield return new WaitForSeconds(1f);
            _battery = Mathf.Min(_maxBattery, _battery + Mathf.RoundToInt(_chargeRate));
            Debug.Log($"Батарея заряжается: {_battery}%");

            if (_battery >= _maxBattery)
            {
                _battery = _maxBattery;
                Debug.Log("Батарея полностью заряжена!");
                yield break;
            }
        }

        Debug.Log("Зарядка батареи остановлена");
    }

    private void StartCooldown()
    {
        if (_cooldownCoroutine != null)
        {
            StopCoroutine(_cooldownCoroutine);
        }
        _cooldownCoroutine = StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        _isCooldown = true;
        Debug.Log($"Кулдаун зарядки начался: {_chargeCooldown} сек");

        yield return new WaitForSeconds(_chargeCooldown);

        _isCooldown = false;
        Debug.Log("Кулдаун завершен, можно заряжать");

        // После кулдауна запускаем зарядку если нужно
        UpdateBatteryLogic();
    }

    // Методы для UI
    public void BlockFLashlight() => _isBlock = true;
    public void UnblockFLashlight() => _isBlock = false;
}
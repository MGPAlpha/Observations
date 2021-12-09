using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class Keypad : MonoBehaviour
{
    public static Keypad _kp;

    private Vector3 _startPos;
    private Quaternion _startRot;
    private Transform _startParent;

    [SerializeField] private TextMeshProUGUI _codeText;
    [SerializeField] private Image _codeBackground;

    [SerializeField] private string _code = "9999";

    private Color defaultColor;
    [SerializeField] private Color successColor = Color.green;
    [SerializeField] private Color failureColor = Color.red;

    [SerializeField] private UnityEvent _onSuccess;

    private string _enteredCode = "";
    private bool _success = false;

    private float _motionTime = 0;

    [SerializeField] private float _moveTime = .5f;

    public void EnterDigit(string digit) {
        if (_success) return;

        if (_enteredCode.Length >= 4) {
            _enteredCode = "";
            _codeBackground.color = defaultColor;
        }
        
        _enteredCode += digit;
        string display = _enteredCode;
        for (int i = display.Length; i < 4; i++) {
            if (i == 0) {
                display += "*";
            } else display += " *";
        }
        _codeText.text = display;

        if (_enteredCode.Length >= 4) {
            if (_enteredCode == _code) {
                _success = true;
                _codeBackground.color = successColor;
                if (_onSuccess.GetPersistentEventCount() > 0) _onSuccess.Invoke();
            } else {
                _codeBackground.color = failureColor;
            }
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _startPos = transform.position;
        _startRot = transform.rotation;
        _startParent = transform.parent;

        defaultColor = _codeBackground.color;
    }

    // Update is called once per frame
    void Update()
    {
        bool isSelected = (Keypad._kp == this);
        
        _motionTime = Mathf.MoveTowards(_motionTime, isSelected ? _moveTime : 0, Time.deltaTime);

        float motionProgress = Mathf.SmoothStep(0,1,_motionTime / _moveTime);

        transform.position = Vector3.Lerp(_startPos, InteractionManager._im.keypadPos.position, motionProgress);

        transform.rotation = Quaternion.Lerp(_startRot, InteractionManager._im.keypadPos.rotation, motionProgress);
        
        transform.parent = _motionTime == 0 ? _startParent : InteractionManager._im.transform;
    }
}

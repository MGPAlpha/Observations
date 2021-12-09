using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager _im {
        get;
        private set;
    }

    private Camera _ca;

    [SerializeField] private LayerMask interactionLayers;

    [SerializeField] public Transform keypadPos;

    private void Awake()
    {
        _im = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _ca = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseManager.paused && Input.GetKeyUp(KeyCode.E)) {
            if (Keypad._kp) {
                Keypad._kp = null;
                Cursor.lockState = CursorLockMode.Locked;
            } else {
                RaycastHit r;
                if (Physics.Raycast(transform.position, transform.forward, out r, interactionLayers)) {
                    Keypad possibleNewKeypad;
                    if (r.transform.gameObject.TryGetComponent<Keypad>(out possibleNewKeypad)) {
                        Keypad._kp = possibleNewKeypad;
                        Cursor.lockState = CursorLockMode.None;
                    }
                }
            }
        }
    }
}

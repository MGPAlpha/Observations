using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnScan : MonoBehaviour
{
    [SerializeField] private UnityEvent onScan;

    private bool activated = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate() {
        if (activated) return;
        activated = true;
        if (onScan.GetPersistentEventCount() > 0) onScan.Invoke();
    }
}

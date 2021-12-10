using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hallucination : MonoBehaviour
{
    private Renderer _re;

    [SerializeField] private float maxTime = 10f;
    [SerializeField] private float maxHallucination = .5f;

    private float timer = 0;

    private bool doHallucination = false;

    public void StartHallucination() {
        doHallucination = true;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _re = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        timer = Mathf.MoveTowards(timer, doHallucination ? maxTime : 0, Time.deltaTime);
        _re.material.SetFloat("_EyeDensity", Mathf.Lerp(0, maxHallucination, Mathf.InverseLerp(0, maxTime, timer)));
    }
}

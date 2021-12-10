using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PostAdjuster : MonoBehaviour
{
    private Volume _pp;

    [SerializeField] private float maxTime = 10f;

    private float initialWeight;

    private float timer = 0;

    private bool doHallucination = false;

    public void StartAdjustment() {
        doHallucination = true;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _pp = GetComponent<Volume>();
        initialWeight = _pp.weight;
    }

    // Update is called once per frame
    void Update()
    {
        timer = Mathf.MoveTowards(timer, doHallucination ? maxTime : 0, Time.deltaTime);
        _pp.weight = Mathf.Lerp(initialWeight, 1, Mathf.InverseLerp(0, maxTime, timer));
    }
}

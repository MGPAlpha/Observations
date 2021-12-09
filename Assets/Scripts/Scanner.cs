using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Scanner : MonoBehaviour
{
    public Datapad datapad;

    public static Scanner _sc {
        get;
        private set;
    }

    private Transform lastTargeted;
    [SerializeField] private GameObject scanBeam;
    [SerializeField] private float maxScanDistance = 20;
    private Renderer scanBeamRen;
    private AudioSource audio;

    [SerializeField] private Image target;

    [SerializeField] private float scanTime = 3;
    private float timeComplete = 0;
    private float targetFill = 0;
    [SerializeField] private float targetEmptySpeed = 1;

    [SerializeField] private PauseManager pause;

    private void Awake()
    {
        _sc = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        scanBeamRen = scanBeam.GetComponent<Renderer>();
        Vector3 correctRelativePos = Quaternion.Inverse(transform.parent.rotation) * (transform.parent.position - scanBeam.transform.position);
        // scanBeamRen.material.SetVector("_EndDisplacement", transform.parent.position - scanBeam.transform.position);
        scanBeamRen.material.SetVector("_EndDisplacement", correctRelativePos);
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseManager.paused) return;
        if (Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Keypad._kp) {
            RaycastHit r;
            Physics.Raycast(transform.parent.position, transform.parent.forward, out r, 100, 64 + 8);
            if (r.transform && r.transform.gameObject.layer == 6 && r.distance <= maxScanDistance) {
                if (!lastTargeted || r.transform != lastTargeted) {
                    lastTargeted = r.transform;
                    timeComplete = 0;
                    targetFill = 0;
                } else {
                    timeComplete = Mathf.MoveTowards(timeComplete, scanTime, Time.deltaTime);
                    targetFill = timeComplete / scanTime;
                    if (timeComplete >= scanTime) {
                        datapad.FindLog(r.transform.gameObject);
                    }
                }
            } else {
                timeComplete = 0;
                lastTargeted = null;
            }
            scanBeamRen.enabled = true;
            scanBeamRen.material.SetFloat("_Length", r.transform && r.distance < maxScanDistance ? r.distance : maxScanDistance);
            if (!audio.isPlaying) audio.Play();
        } else {
            timeComplete = 0;
            lastTargeted = null;
            scanBeamRen.enabled = false;
            if (audio.isPlaying) audio.Stop();
        }

        if (!lastTargeted) {
            targetFill = Mathf.MoveTowards(targetFill, 0, targetEmptySpeed * Time.deltaTime);
        }

        target.material.SetFloat("_TargetFill", targetFill);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Datapad : MonoBehaviour
{
    [SerializeField] private Transform inactivePosition;
    [SerializeField] private Transform activePosition;
    [SerializeField] private float moveTime = .5f;

    [SerializeField] private GameObject rightClickIcon;
    [SerializeField] private GameObject helpScreen;
    [SerializeField] private GameObject logScreen;
    [SerializeField] private GameObject logEntryPrototype;
    [SerializeField] private GameObject playbackScreen;
    [SerializeField] private GameObject radarScreen;
    [SerializeField] private GameObject radarPoint;
    [SerializeField] private GameObject helpPrompt;
    [SerializeField] private GameObject radarPrompt;
    [SerializeField] private GameObject stopPrompt;
    [SerializeField] private GameObject logsPrompt;

    [SerializeField] private float initialHelpScreenTime = 3f;
    [SerializeField] private float sleepTime = 5f;


    [SerializeField] float maxRadarDistance = 100;
    [SerializeField] float radarShrinkDistance = 150;
    [SerializeField] Color visitedRadarColor;
    [SerializeField] Color newLogHighlight;
    [SerializeField] float highlightFrequency = 1;

    [SerializeField] private Log[] logs;

    [SerializeField] private AudioClip notification;
    private GameObject[] logEntries;
    private AudioSource audioSource;

    private int[] logStatuses;
    
    private float motionTime = 0;

    private bool firstActivated = false;
    private float timeSinceFirstActivation = 0;
    private float timeSinceActive;

    private bool playing = false;

    private bool radarOn = false;
    private float radarWidth;
    private GameObject[] radarPoints;

    [SerializeField] private PauseManager pause;

    

    public void FindLog(GameObject go) {
        for (int i = 0; i < logs.Length; i++) {
            Log l = logs[i];
            if (l.scannable && go == l.scannable && logStatuses[i] < 1) {
                logStatuses[i] = 1;
                LogEntry le = logEntries[i].GetComponent<LogEntry>();
                le.titleText.text = l.title;
                le.indexText.text = "[" + (i + 1) + "]";
                radarOn = false;
                timeSinceActive = 0;
                radarPoints[i].GetComponent<Image>().color = visitedRadarColor;
                audioSource.PlayOneShot(notification, .5f);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        logStatuses = new int[logs.Length];
        
        logEntries = new GameObject[logs.Length];
        for (int i = 0; i < logs.Length; i++) {
            GameObject newLogEntry = Instantiate(logEntryPrototype, logScreen.transform);
            newLogEntry.GetComponent<RectTransform>().anchoredPosition += Vector2.down * 100 * i;
            LogEntry le = newLogEntry.GetComponent<LogEntry>();
            le.indexText.text = "[?]";
            le.titleText.text = "?????";
            logEntries[i] = newLogEntry;
            
        }
        logEntryPrototype.SetActive(false);

        timeSinceActive = sleepTime;

        radarPoints = new GameObject[logs.Length];
        for (int i = 0; i < logs.Length; i++) {
            if (!logs[i].scannable) continue;
            GameObject newRadarPoint = Instantiate(radarPoint, radarScreen.transform);
            radarPoints[i] = newRadarPoint;
        }
        radarPoint.SetActive(false);

        radarWidth = radarScreen.GetComponent<RectTransform>().sizeDelta.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (pause && pause.paused) return;
        
        bool active = Input.GetMouseButton(1);
        motionTime = Mathf.MoveTowards(motionTime, active ? moveTime : 0, Time.deltaTime);

        float motionProgress = Mathf.SmoothStep(0,1,motionTime / moveTime);

        transform.position = Vector3.Lerp(inactivePosition.position, activePosition.position, motionProgress);

        transform.rotation = Quaternion.Lerp(inactivePosition.rotation, activePosition.rotation, motionProgress);
        
        if (!firstActivated && active) firstActivated = true;
        if (firstActivated) timeSinceFirstActivation += Time.deltaTime;

        if (!active) timeSinceActive += Time.deltaTime;
        else timeSinceActive = 0;

        if (playing && (!audioSource.isPlaying || (active && Input.GetKeyUp(KeyCode.E)) && !Input.GetKey(KeyCode.Q))) {
            playing = false;
            audioSource.Stop();
            timeSinceActive = 0;
        } else if (active && !playing && Input.GetKeyUp(KeyCode.E) && !Input.GetKey(KeyCode.Q)) {
            radarOn = !radarOn;
        }

        if (active) {
            for (int i = 0; i < Mathf.Min(logs.Length, 9); i++) {
                if (!playing && !radarOn && Input.GetKeyUp("" + (i + 1)) && logStatuses[i] > 0) {
                    logStatuses[i] = 2;
                    playing = true;
                    audioSource.PlayOneShot(logs[i].audio);
                    playbackScreen.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = logs[i].title;
                }
            }
        }

        for (int i = 0; i < logs.Length; i++) {
            if (radarPoints[i]) {
                Vector3 distance = (logs[i].scannable.transform.position - transform.parent.position);
                Vector2 flatDistance = new Vector2(distance.x, distance.z);
                Vector2 rotatedDistance = Quaternion.Euler(0,0,transform.parent.rotation.eulerAngles.y) * flatDistance;
                Vector2 radarPos = rotatedDistance / maxRadarDistance * radarWidth / 2;
                if (radarPos.magnitude > radarWidth / 2) radarPos = radarPos.normalized * radarWidth / 2;
                radarPoints[i].GetComponent<RectTransform>().anchoredPosition = radarPos;
                float radarPointSize = Mathf.Lerp(1, .5f, Mathf.InverseLerp(maxRadarDistance, radarShrinkDistance, rotatedDistance.magnitude));
                radarPoints[i].GetComponent<RectTransform>().sizeDelta = radarPoint.GetComponent<RectTransform>().sizeDelta * radarPointSize;
            }
        }

        for (int i = 0; i < logEntries.Length; i++) {
            if (logStatuses[i] == 1) {
                logEntries[i].GetComponent<Image>().color = Color.Lerp(newLogHighlight, logEntryPrototype.GetComponent<Image>().color, (Mathf.Sin(Time.time * highlightFrequency) / 2 + .5f));
            } else {
                logEntries[i].GetComponent<Image>().color = logEntryPrototype.GetComponent<Image>().color;
            }
        }

        if (timeSinceActive > sleepTime && !playing) {
            rightClickIcon.SetActive(true);
            helpScreen.SetActive(false);
            logScreen.SetActive(false);
            playbackScreen.SetActive(false);
            radarScreen.SetActive(false);
            helpPrompt.SetActive(false);
            radarPrompt.SetActive(false);
            stopPrompt.SetActive(false);
            logsPrompt.SetActive(false);
        } else if (timeSinceFirstActivation < initialHelpScreenTime || Input.GetKey(KeyCode.Q) && active) {
            rightClickIcon.SetActive(false);
            helpScreen.SetActive(true);
            logScreen.SetActive(false);
            playbackScreen.SetActive(false);
            radarScreen.SetActive(false);
            helpPrompt.SetActive(false);
            radarPrompt.SetActive(false);
            stopPrompt.SetActive(false);
            logsPrompt.SetActive(false);
        } else if (!playing && !radarOn) {
            rightClickIcon.SetActive(false);
            helpScreen.SetActive(false);
            logScreen.SetActive(true);
            playbackScreen.SetActive(false);
            radarScreen.SetActive(false);
            helpPrompt.SetActive(true);
            radarPrompt.SetActive(true);
            stopPrompt.SetActive(false);
            logsPrompt.SetActive(false);
        } else if (playing) {
            rightClickIcon.SetActive(false);
            helpScreen.SetActive(false);
            logScreen.SetActive(false);
            playbackScreen.SetActive(true);
            radarScreen.SetActive(false);
            helpPrompt.SetActive(true);
            radarPrompt.SetActive(false);
            stopPrompt.SetActive(true);
            logsPrompt.SetActive(false);
        } else if (radarOn) {
            rightClickIcon.SetActive(false);
            helpScreen.SetActive(false);
            logScreen.SetActive(false);
            playbackScreen.SetActive(false);
            radarScreen.SetActive(true);
            helpPrompt.SetActive(true);
            radarPrompt.SetActive(false);
            stopPrompt.SetActive(false);
            logsPrompt.SetActive(true);
        }
    }
}

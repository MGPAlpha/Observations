using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private float deathTime = 55;

    private float deathTimer = 0;
    private bool deathStarted = false;

    public static bool paused {
        get;
        private set;
    } = false;

    public void StartDeath() {
        deathStarted = true;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        SetPaused(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && !deathStarted) {
            Toggle();
        }

        if (deathStarted) {
            deathTimer += Time.deltaTime;
        }

        if (deathScreen) {
            deathScreen.SetActive(deathTimer > deathTime);
        }

        if (deathTimer > deathTime) {
            Time.timeScale = 0;
            paused = true;
            Cursor.lockState = CursorLockMode.None;
        }

        // Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
        // Cursor.visible = !paused;
        // Debug.Log(Cursor.visible);
    }

    public void SetPaused(bool p) {
        paused = p;
        Time.timeScale = p ? 0 : 1;
        Cursor.lockState = (p || Keypad._kp) ? CursorLockMode.None : CursorLockMode.Locked;
        // Cursor.visible = !p;
        pauseMenu.SetActive(p);
    }

    public void Pause() {
        SetPaused(true);
    }

    public void Unpause() {
        SetPaused(false);
    }

    public void Toggle() {
        SetPaused(!paused);
    }

    public void Quit() {
        Application.Quit();
    }
}

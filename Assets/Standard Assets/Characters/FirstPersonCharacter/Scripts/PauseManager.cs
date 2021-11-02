using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    
    public bool paused {
        get;
        private set;
    } = false;
    
    // Start is called before the first frame update
    void Start()
    {
        SetPaused(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)) {
            Toggle();
        }
        // Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
        // Cursor.visible = !paused;
        // Debug.Log(Cursor.visible);
    }

    public void SetPaused(bool p) {
        paused = p;
        Time.timeScale = p ? 0 : 1;
        Cursor.lockState = p ? CursorLockMode.None : CursorLockMode.Locked;
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

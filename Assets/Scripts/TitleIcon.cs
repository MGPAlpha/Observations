using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleIcon : MonoBehaviour
{
    [SerializeField] private TargetUI targetIcon;
    [SerializeField] private float visibleTime = 3;
    [SerializeField] private float fadeTime = 5;

    private Image image;

    private float timer = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.unscaledDeltaTime;
        Color col = image.color;
        image.color = new Color(col.r, col.g, col.b, Mathf.InverseLerp(fadeTime, visibleTime, timer));;
        if (!targetIcon.titleDone && timer >= fadeTime) {
            targetIcon.titleDone = true;
        }
    }
}

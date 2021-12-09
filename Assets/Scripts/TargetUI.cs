using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetUI : MonoBehaviour
{
    [SerializeField] private float fadeTime = .3f;

    public bool titleDone = false;

    private Image image;
    
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.MoveTowards(image.color.a, !titleDone || Keypad._kp || Input.GetMouseButton(1) ? 0 : 1, 1 / fadeTime * Time.deltaTime)); 
    }
}

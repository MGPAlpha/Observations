using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderDisplay : MonoBehaviour
{
    [SerializeField] private GameObject player;
    
    private Renderer re;

    // Start is called before the first frame update
    void Start()
    {
        re = GetComponent<Renderer>();
        re.material.SetFloat("_MinOpacity", 0);
    }

    // Update is called once per frame
    void Update()
    {
        re.material.SetVector("_PlayerPos", player.transform.position);
    }
}

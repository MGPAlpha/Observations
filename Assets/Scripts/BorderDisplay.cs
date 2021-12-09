using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderDisplay : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float _minOpacity = 0;
    [SerializeField] private float _deactivationTime = 1f;

    private Renderer re;

    private bool _deactivating = false;
    private float _deactivationTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        re = GetComponent<Renderer>();
        re.material.SetFloat("_MinOpacity", _minOpacity);
    }

    public void Deactivate() {
        GetComponent<Collider>().enabled = false;
        _deactivating = true;
    }

    // Update is called once per frame
    void Update()
    {
        re.material.SetVector("_PlayerPos", player.transform.position);
        if (_deactivating) {
            _deactivationTimer += Time.deltaTime;
            _deactivationTimer = Mathf.MoveTowards(_deactivationTimer, _deactivationTimer, Time.deltaTime);
            re.material.SetFloat("_MinOpacity", Mathf.Lerp(0, _minOpacity, Mathf.InverseLerp(_deactivationTime, 0, _deactivationTimer)));
            re.material.SetFloat("_MaxOpacity", Mathf.InverseLerp(1,0,_deactivationTimer/_deactivationTime));
        }
    }
}

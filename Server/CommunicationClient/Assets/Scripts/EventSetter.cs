using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSetter : MonoBehaviour
{
    Camera mainCam;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        GetComponent<Canvas>().worldCamera = mainCam;
    }

    private void FixedUpdate()
    {
        transform.LookAt(mainCam.transform);
    }
}

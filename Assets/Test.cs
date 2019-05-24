using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    RectTransform rectT;
    // Start is called before the first frame update
    void Start()
    {
        rectT = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("TEST POSITION = " + rectT.anchoredPosition);
    }
}

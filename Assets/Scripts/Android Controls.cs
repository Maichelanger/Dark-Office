using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidControls : MonoBehaviour
{
    private GameObject stick, actionButton;

#if UNITY_ANDROID || UNITY_IOS
    private void Start()
    {
        stick = GameObject.Find("MoveJoystick");
        actionButton = GameObject.Find("Action Button");

        stick.SetActive(true);
        actionButton.SetActive(true);
    }
#else
    private void Start()
    {
        stick = GameObject.Find("MoveJoystick");
        actionButton = GameObject.Find("Action Button");

        stick.SetActive(false);
        actionButton.SetActive(false);
    }
#endif
}

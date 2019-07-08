using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeDisplay : MonoBehaviour
{
    [SerializeField]
    private Text timerText;
    [SerializeField]
    private MapTime mapTime;
    private float timeLeft;
    private string minutes;
    private string seconds;

    private void Start()
    {
        mapTime = FindObjectOfType<MapTime>();
    }

    private void Update()
    {
        if (mapTime == null) {
            mapTime = FindObjectOfType<MapTime>();
        }
        timeLeft = mapTime.timeLeft;
        minutes = ((int)timeLeft / 60).ToString();
        seconds = (timeLeft % 60).ToString("f0");
        timerText.text = minutes + ":" + seconds;
    }
}

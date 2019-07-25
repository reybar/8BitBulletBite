using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadBar : MonoBehaviour
{
    public bool reload;
    public Transform tick;
    public Slider slider;
    public int startPos = 0;
    public int endPos = 2;

    void Update()
    {
        if (reload) {
            slider.value += Time.deltaTime/2;
            if (slider.value >= 1) {
                slider.value = 0;
                reload = false;
            }
        } else {
            slider.value = 0;
            gameObject.SetActive(false);
        }
    }
}

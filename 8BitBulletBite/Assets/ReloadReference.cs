using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadReference : MonoBehaviour
{
    public ReloadBar reloadBar;

    public void Reload()
    {
        reloadBar.gameObject.SetActive(true);
        reloadBar.reload = true;
    }
    public void Stop()
    {
        reloadBar.reload = false;
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SetupLocalPlayer : NetworkBehaviour {

    [SyncVar]
    public string pname = "player";

    [SyncVar]
    public Color playerColor = Color.white;

    ColorSwap colorSwap;
    /*void OnGUI()
	{
		if(isLocalPlayer)
		{
			pname = GUI.TextField (new Rect (25, Screen.height - 40, 100, 30), pname);
			if(GUI.Button(new Rect(130,Screen.height - 40,80,30),"Change"))
			{
				CmdChangeName(pname);
			}

			//string ip = MasterServer.ipAddress;
			//GUI.TextField (new Rect (250, Screen.height - 40, 100, 30), ip);
		}
	}
	[Command]
	public void CmdChangeName(string newName)
	{
		pname = newName;
		this.GetComponentInChildren<TextMesh>().text = pname;
	}*/



    // Use this for initialization
    void Start() {
        colorSwap = GetComponent<ColorSwap>();


        if(playerColor == Color.blue) {
            colorSwap.colorNum = 0;
        }
        else if(playerColor == Color.magenta) {
            colorSwap.colorNum = 1;
        }
        else if(playerColor == Color.green) {
            colorSwap.colorNum = 2;
        }
        else if(playerColor == Color.black) {
            colorSwap.colorNum = 3;
        }


        Renderer[] rends = GetComponentsInChildren<Renderer>();
        foreach(Renderer r in rends)
            r.material.color = playerColor;

    }

    /*void Update()
	{
		this.GetComponentInChildren<TextMesh>().text = pname;
	}*/
}






//SmoothCameraFollow.target = this.transform;

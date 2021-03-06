﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSwap : MonoBehaviour
{
    Transform body;
    public Material colorer;
    public Color[] colorPattern1;
    public Color[] colorPattern2;
    public Color[] colorPattern3;
    public Color[] colorPattern4;
    public int colorNum;

    void Start()
    {
        body = transform.Find("Body");
        colorer = body.GetComponent<Renderer>().material;
        switch (colorNum) {
            case 0:
                ColorIn(colorPattern1);
                break;
            case 1:
                ColorIn(colorPattern2);
                break;
            case 2:
                ColorIn(colorPattern3);
                break;
            case 3:
                ColorIn(colorPattern4);
                break;
        }
    }

    void ColorIn(Color[] palette)
    {
        colorer.SetColor("_Color1out", palette[0]);
        colorer.SetColor("_Color2out", palette[1]);
        colorer.SetColor("_Color3out", palette[2]);
    }
}

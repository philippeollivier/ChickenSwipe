using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Style
{
    public Sprite IMG_Background;
    public Sprite SPR_Coin;
    public Sprite SPR_Player;
    public Sprite FloorNormal;
    public Sprite FloorS1;
    public Sprite FloorS2;
    public Sprite FloorS3;
    public Sprite FloorS4;
    public Sprite FloorBroken;
    public Sprite thumbnail;
    public Sprite dead;
    public bool isLocked;
    public int price;
}


public class StyleContainer : MonoBehaviour
{
    public static StyleContainer Instance;

    public Style[] Styles;

    private void Awake()
    {
        Instance = this;
    }
    private void OnDestroy()
    {
        Instance = null;
    }
}

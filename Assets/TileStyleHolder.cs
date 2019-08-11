using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class TileStyle
{
    public gridState TileState;
    public Color32 TileColor;
}


public class TileStyleHolder : MonoBehaviour
{
    //SINGLETON
    public static TileStyleHolder Instance;

    public TileStyle[] TileStyles;

    private void Awake()
    {
        Instance = this;
    }
}

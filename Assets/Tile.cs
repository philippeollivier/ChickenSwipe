using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum gridState
{
    Normal, Warm, Hot, Broken, Wall, Bugged
}

public class Tile : MonoBehaviour
{
    public gridState TileState;
    private Image TileImage;
    private Animator anim;
    public float TileWarmth = 0.0f;
    public float hotTime = 2.0f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        TileImage = transform.Find("TileImage").GetComponent<Image>();
    }

    public void PlayExplodeAnimation()
    {
        anim.SetTrigger("Explode");
    }

    void ApplyStyleFromHolder(int index)
    {
        switch (TileState)
        {
            case gridState.Normal:
                TileImage.sprite = StyleContainer.Instance.Styles[PlayerPrefs.GetInt("Style", 0)].FloorNormal;
                TileWarmth = 0.0f;
                break;
            case gridState.Warm:
                if (TileWarmth < 0.25f * hotTime)
                {
                    TileImage.sprite = StyleContainer.Instance.Styles[PlayerPrefs.GetInt("Style", 0)].FloorS1;
                }
                else if (TileWarmth < 0.50f * hotTime)
                {
                    TileImage.sprite = StyleContainer.Instance.Styles[PlayerPrefs.GetInt("Style", 0)].FloorS2;
                }
                else if (TileWarmth < 0.75 * hotTime) 
                {
                    TileImage.sprite = StyleContainer.Instance.Styles[PlayerPrefs.GetInt("Style", 0)].FloorS3;
                }
                else
                {
                    TileImage.sprite = StyleContainer.Instance.Styles[PlayerPrefs.GetInt("Style", 0)].FloorS4;
                }
                break;
            case gridState.Hot:
                TileImage.sprite = StyleContainer.Instance.Styles[PlayerPrefs.GetInt("Style", 0)].FloorBroken;
                break;
            case gridState.Broken:
                //These are maybe for later
                break;
            case gridState.Wall:
                //These are maybe for later
                break;
            default:
                TileImage.sprite = StyleContainer.Instance.Styles[PlayerPrefs.GetInt("Style", 0)].FloorNormal;
                break;
        }
    }

    public void ApplyStyle()
    {
        switch (TileState)
        {
            case gridState.Normal:
                ApplyStyleFromHolder(0);
                break;
            case gridState.Warm:
                ApplyStyleFromHolder(1);
                break;
            case gridState.Hot:
                ApplyStyleFromHolder(2);
                break;
            case gridState.Broken:
                ApplyStyleFromHolder(3);
                break;
            case gridState.Wall:
                ApplyStyleFromHolder(4);
                break;
            default:
                ApplyStyleFromHolder(5);
                break;
        }
    }
}

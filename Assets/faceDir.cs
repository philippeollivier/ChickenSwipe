using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class faceDir : MonoBehaviour
{
    //face left
    public void faceLeft()
    {
        transform.forward = -Vector3.forward;
    }

    //face right
    public void faceRight()
    {
        transform.forward = Vector3.forward;
    }

}

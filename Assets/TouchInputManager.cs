using UnityEngine;
using System.Collections;

public class TouchInputManager : MonoBehaviour {
	
	// https://pfonseca.com/swipe-detection-on-unity

	private float fingerStartTime  = 0.0f;
	private Vector2 fingerStartPos = Vector2.zero;
	
	private bool isSwipe = false;
	private float minSwipeDist  = 50.0f;
	private float maxSwipeTime = 1.5f;

    private bool isSwiping = false;
    private float minSwipingDist = 1000.0f;

    private GameManager gm;
	
	void Awake()
	{
		gm = GameObject.FindObjectOfType<GameManager> ();
	}

	// Update is called once per frame
	void Update () {

        foreach (Touch touch in Input.touches)
        {
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    /* this is a new touch */
                    isSwipe = true;
                    fingerStartTime = Time.time;
                    fingerStartPos = touch.position;
                    break;

                case TouchPhase.Canceled:
                    /* The touch is being canceled */
                    isSwipe = false;
                    break;

                case TouchPhase.Moved:
                    if (isSwipe)
                    {
                        Vector2 direction = touch.position - fingerStartPos;

                        if (direction.x * direction.x > minSwipingDist || direction.y * direction.y > minSwipingDist)
                        {

                            Vector2 swipeType = Vector2.zero;
                            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                            {
                                // the swipe is horizontal:
                                swipeType = Vector2.right * Mathf.Sign(direction.x);
                            }
                            else
                            {
                                // the swipe is vertical:
                                swipeType = Vector2.up * Mathf.Sign(direction.y);
                            }

                            if (swipeType.x != 0.0f)
                            {
                                if (swipeType.x > 0.0f)
                                {
                                    // MOVE RIGHT
                                    gm.Move(MoveDirection.Right);
                                }
                                else
                                {
                                    // MOVE LEFT
                                    gm.Move(MoveDirection.Left);
                                }
                                isSwipe = false;
                            }

                            if (swipeType.y != 0.0f)
                            {
                                if (swipeType.y > 0.0f)
                                {
                                    // MOVE UP
                                    gm.Move(MoveDirection.Up);
                                }
                                else
                                {
                                    // MOVE DOWN
                                    gm.Move(MoveDirection.Down);
                                }
                                isSwipe = false;
                            }
                        }
                    }
                   
                    break;
            }
        }

    }
		
}
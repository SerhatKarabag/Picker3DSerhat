using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public bool GetStartPos;
    public bool swipeLeft;
    public bool swipeRight;
    public bool moveThreshold;
    private Vector2 touchStartPos; // The position which we start touching.
    public float distanceX;

    private void Update()
    {
        GetPlayerInput();
    }
    private void GetPlayerInput()
    {
        CheckPlayerTouchScreen();
    }
    private void CheckPlayerTouchScreen()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GetStartPos = true; // We got StartPos so we are ready to take CurrentPos.
            touchStartPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y); // Get touch start coordinates.
        }
        else if (Input.GetMouseButton(0) && GetStartPos) // If camera is on its default position and if we have a StartPos. 
        {
            Vector2 touchCurrentPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y); // Get CurrentPos while player draging finger.
            distanceX = touchCurrentPos.x - touchStartPos.x; // Find x axis distance between 2 positions.  

            if (Mathf.Abs(distanceX) > 1f) // If distance is enough.
            {
                moveThreshold = true;
                if (distanceX > 0) // left trigger
                {
                    swipeLeft = true;
                    swipeRight = false;
                }
                else // right trigger
                {
                    swipeRight = true;
                    swipeLeft = false;
                }
                touchStartPos.x = Input.mousePosition.x;
            }
            else
            {
                moveThreshold = false;
            }
        }
        CheckFingerLeftFromScreen();
    }

    private void CheckFingerLeftFromScreen()
    {
        if (Input.GetMouseButtonUp(0))
        {
            GetStartPos = false;
            swipeLeft = false;
            swipeRight = false;
            distanceX = 0;
            moveThreshold = false;
        }
    }
}

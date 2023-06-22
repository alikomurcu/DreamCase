using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    // public members
    public bool IsDragging { get; private set; }
    public Vector2 DragStartPosition { get; private set; }
    public Vector2 DragEndPosition { get; private set; }
    public Vector2 DragDirection { get; private set; }
    [SerializeField] private Camera mainCamera;
    
    public void OnEndDrag(PointerEventData eventData)
    {
        /*
         * This method is responsible for setting the drag(swipe) end position
         * and the drag direction
         */
        // eventData.position is the screen position of the pointer when it is released
        // eventData.pressPosition is the screen position of the pointer when it is pressed
        // So, we will use the position - pressposition vector as the drag(swipe) direction
        Vector2 swipeStartPosition = mainCamera.ScreenToWorldPoint(eventData.pressPosition);
        Vector2 dragDirection = (eventData.position - eventData.pressPosition).normalized;
        Debug.Log("Swipe Press Position in World Coordinates: " + swipeStartPosition);
        if (Mathf.Abs(dragDirection.x) > Mathf.Abs(dragDirection.y))
        {
            // if the drag direction is horizontal
            if (dragDirection.x > 0)
            {
                // if the drag direction is right
                DragDirection = Vector2.right;
                Debug.Log("RIGHT");
            }
            else
            {
                // if the drag direction is left
                DragDirection = Vector2.left;
                Debug.Log("LEFT");
            }
        }
        else
        {
            // if the drag direction is vertical
            if (dragDirection.y > 0)
            {
                // if the drag direction is up
                DragDirection = Vector2.up;
                Debug.Log("UP");
            }
            else
            {
                // if the drag direction is down
                DragDirection = Vector2.down;
                Debug.Log("DOWN");
            }
        }
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        // throw new NotImplementedException();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // throw new NotImplementedException();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // throw new NotImplementedException();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // throw new NotImplementedException();
    }
}

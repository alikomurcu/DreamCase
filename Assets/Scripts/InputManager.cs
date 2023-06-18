using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    // This is a singleton class which handles inputs by means of the EventSystem.
    public static InputManager Instance { get; private set; }
    // public members
    public bool IsDragging { get; private set; }
    public Vector2 DragStartPosition { get; private set; }
    public Vector2 DragEndPosition { get; private set; }
    public Vector2 DragDirection { get; private set; }

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            // set the instance
            Instance = this;
        }
    }

    // Event handlers for the inputs
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
        Debug.Log("eventData.position: " + eventData.position + " eventData.pressPosition: " + eventData.pressPosition);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("OnPointerUp");
    }

    public void OnPointerLeft(PointerEventData eventData)
    {
        Debug.Log("OnPointerLeft");
    }

    public void OnPointerRight(PointerEventData eventData)
    {
        Debug.Log("OnPointerRight");
    }
}

// Just add this script to your camera. It doesn't need any configuration.

using System;
using UnityEngine;

public class TouchCamera : MonoBehaviour
{
    private Vector2?[] oldTouchPositions = { null, null };
    private Vector2 oldTouchVector;
    private float oldTouchDistance;
    public mainGame_Sam maineGame;
    private void Update()
    {
        InputNone();
        InputSingle();
        InputMulti();
    }

    private void InputMulti()
    {
        if (Input.touchCount != 1) return;

        if (oldTouchPositions[1] == null)
        {
            oldTouchPositions[0] = Input.GetTouch(0).position;
            oldTouchPositions[1] = Input.GetTouch(1).position;
            oldTouchVector = (Vector2)(oldTouchPositions[0] - oldTouchPositions[1]);
            oldTouchDistance = oldTouchVector.magnitude;
        }
        else
        {
            Vector2 screen = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);

            Vector2[] newTouchPositions = {
                    Input.GetTouch(0).position,
                    Input.GetTouch(1).position
                };
            Vector2 newTouchVector = newTouchPositions[0] - newTouchPositions[1];
            float newTouchDistance = newTouchVector.magnitude;

            transform.position += transform.TransformDirection((Vector3)((oldTouchPositions[0] + oldTouchPositions[1] - screen) * Camera.main.orthographicSize / screen.y));
            transform.localRotation *= Quaternion.Euler(new Vector3(0, 0, Mathf.Asin(Mathf.Clamp((oldTouchVector.y * newTouchVector.x - oldTouchVector.x * newTouchVector.y) / oldTouchDistance / newTouchDistance, -1f, 1f)) / 0.0174532924f));
            Camera.main.orthographicSize *= oldTouchDistance / newTouchDistance;
            transform.position -= transform.TransformDirection((newTouchPositions[0] + newTouchPositions[1] - screen) * Camera.main.orthographicSize / screen.y);

            oldTouchPositions[0] = newTouchPositions[0];
            oldTouchPositions[1] = newTouchPositions[1];
            oldTouchVector = newTouchVector;
            oldTouchDistance = newTouchDistance;
        }

    }

    private void InputSingle()
    {
        if (Input.touchCount != 1) return;

        if (oldTouchPositions[0] == null || oldTouchPositions[1] != null)
        {
            oldTouchPositions[0] = Input.GetTouch(0).position;
            oldTouchPositions[1] = null;
        }
        else
        {
            Vector2 newTouchPosition = Input.GetTouch(0).position;
            transform.position += transform.TransformDirection((Vector3)((oldTouchPositions[0] - newTouchPosition) * Camera.main.orthographicSize / Camera.main.pixelHeight * 2f));
            oldTouchPositions[0] = newTouchPosition;
        }

    }

    private void InputNone()
    {
        if (Input.touchCount != 0) return;

        oldTouchPositions[0] = null;
        oldTouchPositions[1] = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform cameraPivot;

    [Header("Settings")]
    [SerializeField] private float lookAtHeight = 2f;
    [SerializeField] private float rotationSpeed = 0.2f;
    [SerializeField] private Vector2 verticalClamp = new Vector2(-15f, 30f);
    [SerializeField] private Vector3 cameraOffset = new Vector3(1f, 2f, -4f);

    private Vector2 lastTouchPos;
    private float currentX, currentY;
    private bool pressed => CameraDrag.Instance.Pressed;

    void LateUpdate()
    {
        if (pressed)
        {
            Vector2 delta = GetTouchDelta();
            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                currentX += delta.x * rotationSpeed;
            else
                currentY = Mathf.Clamp(currentY - delta.y * rotationSpeed, verticalClamp.x, verticalClamp.y);
        }

        UpdateCamera();
    }

    private Vector2 GetTouchDelta()
    {
        Vector2 currentPos = Input.mousePosition;

        if (!pressed)
            return Vector2.zero;

        if (lastTouchPos == Vector2.zero)
            lastTouchPos = currentPos;

        Vector2 delta = currentPos - lastTouchPos;
        lastTouchPos = currentPos;
        return delta;
    }

    private void UpdateCamera()
    {
        // Xoay camera quanh player
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        cameraPivot.position = player.position;
        cameraPivot.rotation = rotation;

        // set pos camera
        transform.position = cameraPivot.position + rotation * cameraOffset;
        transform.LookAt(cameraPivot.position + Vector3.up * lookAtHeight);

        // Xoay player theo dir camera (chi truc Y)
        Vector3 euler = player.eulerAngles;
        euler.y = cameraPivot.eulerAngles.y;
        player.eulerAngles = euler;

        if (!pressed) lastTouchPos = Vector2.zero; // Reset khi ngung vuot
    }
}

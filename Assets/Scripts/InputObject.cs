using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class InputObject : MonoBehaviour
{
    private RaycastHit hit;

    private Vector2 one, two;

    [SerializeField]
    private float speed;
    private bool onZoom;

    public static bool test = true;

    private void Update()
    {
        StartCoroutine(Zoom());
    }
    public void OnSwipe(InputAction.CallbackContext callback)
    {
        Debug.Log(callback.ReadValue<Vector2>());
        if (callback.performed)
        {
            Ray ray = Camera.main.ScreenPointToRay(callback.ReadValue<Vector2>());
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit))
            {
                if (raycastHit.transform.tag == "ARObj")
                {
                    Debug.Log(raycastHit.collider.gameObject.name);
                    hit = raycastHit;
                    test = false;
                }
            }
            else
            {
                hit = new RaycastHit();
                test = true;
            }
        }
    }

    public void OnDelta(InputAction.CallbackContext context)
    {
        float X = context.ReadValue<Vector2>().x;
        hit.transform.rotation *= Quaternion.AngleAxis(X, Vector2.up);
    }

    public void OnScroll(InputAction.CallbackContext callback)
    {
        hit.transform.localScale += Vector3.one * callback.ReadValue<Vector2>().y * speed;
    }
    
    public void OnFirstFinger(InputAction.CallbackContext callback)
    {
        one = callback.ReadValue<Vector2>();
    }

    public void OnSecondFinger(InputAction.CallbackContext callback)
    {
        two = callback.ReadValue<Vector2>();
        if (callback.started) onZoom = true;
        if (callback.canceled) StopAllCoroutines();
    }

    public void OnScreenshot()
    {
        var dataPath = Application.persistentDataPath + "/Screenshot.png";
        ScreenCapture.CaptureScreenshot(dataPath);
        Debug.Log(dataPath);
    }

    private IEnumerator Zoom()
    {
        float distance = 0f;
        float prevDistance = 0f;
        while (onZoom)
        {
            distance = Vector2.Distance(one, two);

            if (distance < prevDistance)
            {
                hit.transform.localScale -= Vector3.one * prevDistance * speed;
            }else if(distance > prevDistance)
            {
                hit.transform.localScale += Vector3.one * prevDistance * speed;
            }
            prevDistance = distance;

            yield return null;
        }
    }
}

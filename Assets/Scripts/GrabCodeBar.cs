using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabCodeBar : MonoBehaviour
{
    public CodeBar MyCodeBar;

    private Camera mainCam;

    private Vector3 initialPosition;
    private Transform codeBar;

    private bool returning;
    private float currentTime = 0f;

    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(MyCodeBar != null)
                MyCodeBar.OnBarGrabbed();
        }

        if(Input.GetMouseButton(0))
        {
            Vector3 currentMousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(currentMousePos, Vector2.zero, Mathf.Infinity);
            if(hit && hit.collider.GetComponent<CodeBar>() != null)
            {
                returning = false;
                if (codeBar == null)
                {
                    codeBar = hit.collider.transform;
                    initialPosition = codeBar.transform.position;
                }
                currentMousePos.z = 0f;
                hit.collider.transform.position = currentMousePos;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            returning = true;
            currentTime = 0f;
            MyCodeBar.OnBarReleased();
        }

        if(returning && codeBar != null)
        {
            currentTime += Time.deltaTime;
            codeBar.transform.position = Vector2.Lerp(codeBar.transform.position, initialPosition, currentTime);
        }
    }
}

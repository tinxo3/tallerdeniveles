using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassClipToView : MonoBehaviour
{
    Renderer renderer;

    private void Start()
    {
        renderer = gameObject.GetComponent<Renderer>();
    }
    private void Update()
    {
        renderer.material.SetMatrix("ClipToView",GL.GetGPUProjectionMatrix(Camera.main.projectionMatrix,true).inverse);
    }
}

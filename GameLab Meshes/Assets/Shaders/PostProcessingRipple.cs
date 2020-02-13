using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class PostProcessingRipple : MonoBehaviour
{
    [SerializeField] Material postProcessingMat;
    [SerializeField] float rippleSpeed = 1;
    private Renderer rend;

    private float rippleDistance = 0.1f;
    private bool rippleActive;

    private void Start()
    {
        Camera cam = GetComponent<Camera>();
        cam.depthTextureMode = cam.depthTextureMode | DepthTextureMode.Depth;
        rend = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && rippleActive == false)
        {
            rippleActive = true;
        }

        if (rippleActive)
        {
            rippleDistance += rippleSpeed * Time.deltaTime;

            if (rippleDistance > 500)
            { 
                rippleActive = false;
                rippleDistance = 0.1f;
            }
            Debug.Log(rippleDistance);
            postProcessingMat.SetFloat("_WaveDistance", rippleDistance);
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, postProcessingMat);
    }
}

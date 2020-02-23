using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class PostProcessingRipple : MonoBehaviour
{
    [SerializeField] Material postProcessingMat;
    [SerializeField] float rippleSpeed = 1;
    Camera cam;
    private Renderer rend;

    public Transform scanOrigin;
    private float scanDistance = 5;
    private bool isScanning;

    private void OnEnable()
    {
        cam = GetComponent<Camera>();
        cam.depthTextureMode = cam.depthTextureMode | DepthTextureMode.Depth;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            //Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            //RaycastHit hit;
            //Debug.Log("click");
            //if (Physics.Raycast(ray, out hit))
            //{
               // Debug.Log(hit);
                isScanning = true;
                scanDistance = 5;
                //scanOrigin = hit.point;
            //}
           // scanOrigin = cam.transform.position;
        }

        if (isScanning)
        {
            scanDistance += rippleSpeed * Time.deltaTime;

            if (scanDistance > 500)
            { 
                isScanning = false;
                scanDistance = 5;
            }
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        postProcessingMat.SetVector("_WorldSpaceScannerPos", scanOrigin.position);
        postProcessingMat.SetFloat("_WaveDistance", scanDistance);
        Graphics.Blit(source, destination, postProcessingMat);
    }
}

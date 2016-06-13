using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CaptureCameraScript : MonoBehaviour {

    public RawImage rawImage;

    // Use this for initialization
    void Start ()
    {
        /*WebCamDevice[] devices = WebCamTexture.devices;
        
        foreach (WebCamDevice cam in devices)
        {
            if (cam.isFrontFacing)
            {
                WebCamTexture camTexture = new WebCamTexture(cam.name);
                rawImage.texture = camTexture;
                rawImage.material.mainTexture = camTexture;
                camTexture.Play();
            }
        } */    
    }	
    

    void Update()
    {
        
    }
}

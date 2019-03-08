using UnityEngine;
using UnityEngine.UI;
using OpenCvSharp;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] private RawImage rawImage;
    [SerializeField] private RawImage scanImage;
    [SerializeField] private GameObject fish;
    private WebCamController webCamController;
    private WebCamTexture webcamTexture;
    private Texture scanTexture;

    private void Start()
    {
        webCamController = rawImage.GetComponent<WebCamController>();
    }

    private void Update()
    {
        webcamTexture = webCamController.webcamTexture;
        scanTexture = scanImage.GetComponent<RawImage>().texture;
    }

    public void OnStopClick()
    {
        webCamController.OnStop();
    }

    public void OnPlayClick()
    {
        webCamController.OnPlay();
    }

    public void OnSetTextureClick()
    {
        if (scanTexture != null)
        {
            fish.GetComponent<Renderer>().material.SetTexture("_MainTex", scanTexture);
        }
    }

    public void OnScanClick()
    {
        if (webcamTexture != null)
        {
            scanImage.GetComponent<Scanner>().ScanWebCamera(webcamTexture);
        }
    }

}
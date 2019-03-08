using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using OpenCvSharp;

public class WebCamController : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private int fps;
    private Scanner scanner = new Scanner();
    public WebCamTexture webcamTexture { get; private set; }

    Quaternion baseRotation;

    //void Start()
    //{
    //    WebCamDevice[] devices = WebCamTexture.devices;
    //    webcamTexture = new WebCamTexture(devices[0].name, this.width, this.height, this.fps);
    //    //GetComponent<Renderer>().material.mainTexture = webcamTexture;
    //    GetComponent<RawImage>().texture = webcamTexture;
    //    webcamTexture.Play();
    //}

    void Awake()
    {
        baseRotation = transform.rotation;
    }

    private IEnumerator Start()
    {
        if (WebCamTexture.devices.Length == 0)
        {
            yield break;
        }

        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            yield break;
        }

        WebCamDevice[] devices = WebCamTexture.devices;
        webcamTexture = new WebCamTexture(devices[0].name, this.width, this.height, this.fps);
        gameObject.GetComponent<RawImage>().texture = webcamTexture;
        webcamTexture.Play();

    }

    void Update()
    {
        if (webcamTexture != null && webcamTexture.isPlaying) 
        {
            setScanFrame();
            transform.rotation = baseRotation * Quaternion.AngleAxis(webcamTexture.videoRotationAngle, Vector3.forward);
        }

    }

    void setScanFrame()
    {
    
        gameObject.GetComponent<RawImage>().texture = scanner.getScanFrame(webcamTexture);
    }


    public void OnPlay()
    {
        if (webcamTexture == null)
        {
            return;
        }

        if (webcamTexture.isPlaying)
        {
            return;
        }

        webcamTexture.Play();
    }

    public void OnStop()
    {
        if (webcamTexture == null)
        {
            return;
        }

        if (!webcamTexture.isPlaying)
        {
            return;
        }

        webcamTexture.Stop();
    }

}
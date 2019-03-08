using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestCamera : MonoBehaviour
{

    [SerializeField] private int m_width = 1920;
    [SerializeField] private int m_height = 1080;
    [SerializeField] private RawImage m_displayUI;
    private WebCamTexture m_webCamTexture;

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        if(WebCamTexture.devices.Length == 0)
        {
            Debug.Log("No camera!");
            yield break;
        }

        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);

        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            Debug.LogFormat("No Allow!");
            yield break;
        }

        WebCamDevice userCameraDevice = WebCamTexture.devices[0];
        m_webCamTexture = new WebCamTexture(userCameraDevice.name, m_width, m_height);
    } 

    // Update is called once per frame
    void Update()
    {
        
    }
}

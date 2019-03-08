namespace OpenCvSharp
{
	using UnityEngine;
	using UnityEngine.UI;

	public class Scanner : MonoBehaviour
	{
		private PaperScanner scanner = new PaperScanner();

        public Texture getScanFrame(WebCamTexture inputTexture)
        {
            Mat original = Unity.TextureToMat(inputTexture);
            Size inputSize = new Size(original.Width, original.Height);
            scanner.Input = Unity.TextureToMat(inputTexture);

            if (!scanner.Success)
                scanner.Settings.GrayMode = PaperScanner.ScannerSettings.ColorMode.HueGrayscale;
                
            Point[] detectedContour = scanner.PaperShape;

            var matCombinedFrame = new Mat(new Size(inputSize.Width , inputSize.Height), original.Type(), Scalar.FromRgb(64, 64, 64));

            original.CopyTo(matCombinedFrame.SubMat(0, inputSize.Height, 0, inputSize.Width));
            if (null != detectedContour && detectedContour.Length > 2)
                matCombinedFrame.DrawContours(new Point[][] { detectedContour }, 0, Scalar.FromRgb(255, 255, 0), 3);
                

            return Unity.MatToTexture(matCombinedFrame);
        }




        // Use this for initialization
        public void Process(string name)
		{	
			var rawImage = gameObject.GetComponent<RawImage>();
			rawImage.texture = null;

			Texture2D inputTexture = (Texture2D)Resources.Load("DocumentScanner/" + name);

			scanner.Settings.NoiseReduction = 0.7;											// real-world images are quite noisy, this value proved to be reasonable
			scanner.Settings.EdgesTight = 0.9;												// higher value cuts off "noise" as well, this time smaller and weaker edges
			scanner.Settings.ExpectedArea = 0.2;											// we expect document to be at least 20% of the total image area
			scanner.Settings.GrayMode = PaperScanner.ScannerSettings.ColorMode.Grayscale;	// color -> grayscale conversion mode

			// process input with PaperScanner
			Mat result = null;
			scanner.Input = Unity.TextureToMat(inputTexture);//★scanner.Successで使用する

            // should we fail, there is second try - HSV might help to detect paper by color difference
            if (!scanner.Success)
				// this will drop current result and re-fetch it next time we query for 'Success' flag or actual data
				scanner.Settings.GrayMode = PaperScanner.ScannerSettings.ColorMode.HueGrayscale;

            // now can combine Original/Scanner image
            //result = CombineMats(scanner.Input, scanner.Output, scanner.PaperShape);
            result = scanner.Output;

            // apply result or source (late for a failed scan)
            //rawImage.texture = Unity.MatToTexture(result);
            rawImage.texture =　Unity.MatToTexture(result);

            var transform = gameObject.GetComponent<RectTransform>();
        }



        // Use this for initialization
        public void ProcessWebCam(WebCamTexture webCamTexture)
        {
            var rawImage = gameObject.GetComponent<RawImage>();
            rawImage.texture = null;

            scanner.Settings.NoiseReduction = 0.7;                                          // real-world images are quite noisy, this value proved to be reasonable
            scanner.Settings.EdgesTight = 8;                                              // higher value cuts off "noise" as well, this time smaller and weaker edges
            scanner.Settings.ExpectedArea = 0.05;                                            // we expect document to be at least 20% of the total image area
            scanner.Settings.GrayMode = PaperScanner.ScannerSettings.ColorMode.Grayscale;   // color -> grayscale conversion mode

            // process input with PaperScanner
            Mat result = null;
            scanner.Input = Unity.TextureToMat(webCamTexture);//★scanner.Successで使用する


            // should we fail, there is second try - HSV might help to detect paper by color difference
            if (!scanner.Success)
                    scanner.Settings.GrayMode = PaperScanner.ScannerSettings.ColorMode.HueGrayscale;

            result = scanner.Output;

            rawImage.texture = Unity.MatToTexture(result);
            var transform = gameObject.GetComponent<RectTransform>();
        }

        public void Scan() {
            Process("fish");
        }

        public void ScanWebCamera(WebCamTexture webCamTexture)
        {
            ProcessWebCam(webCamTexture);
        }


    }
}
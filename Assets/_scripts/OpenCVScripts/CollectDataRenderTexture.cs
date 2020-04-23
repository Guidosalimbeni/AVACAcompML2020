using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.UnityUtils;
using System.IO;
using OpenCVForUnity.ImgprocModule;

public class CollectDataRenderTexture: MonoBehaviour
{
    // this was probably only to store on the database some information about the image that has been evaluated. currently only scaled 10 times ... does it make any sense? try to increase?

    public ImageMatrixData imageMatrixData;
    public float resizefactorMainCam = 10.0f;
    [SerializeField]
    private RenderTexture CompositionRenderTexture; // 240 * 180  // mobile net works with 224, 224
    //private OpenCVManager opencvmanager;
    [SerializeField]
    private RenderTexture ImageNNFrontViewRT; // 20 x 20
    [SerializeField]
    private RenderTexture ImageNNtopViewRT; // 20 x 20

    private void Awake()
    {
       // opencvmanager = FindObjectOfType<OpenCVManager>();
        //CompositionRenderTexture = opencvmanager.camRenderTexture;
        imageMatrixData.ImagePixelsListMainPaintView = "";
        imageMatrixData.ImageNNFrontView = "";
        imageMatrixData.ImageNNtopView = "";
    }

    // to commnet out for eventually not lose this bit of code
    public void CollectPixelsValuesFromImageForMainViewRecordInDatabase() // NOT USING THIS
    {
        Texture2D imgTexture = ToTexture2D(CompositionRenderTexture);
        Mat ImageMatrixOpenCV = new Mat(imgTexture.height, imgTexture.width, CvType.CV_8UC1); // 180 * 240 * cv_8uc1
        
        Utils.texture2DToMat(imgTexture, ImageMatrixOpenCV);

        int rows = ImageMatrixOpenCV.rows();
        int cols = ImageMatrixOpenCV.cols();
        int ch = ImageMatrixOpenCV.channels();
        Size sz = new Size((int)(cols / resizefactorMainCam),(int)(rows/ resizefactorMainCam)); // left as 18 * 24 and i have another render texture just for the mobile net

        Imgproc.resize(ImageMatrixOpenCV, ImageMatrixOpenCV, sz);
        Imgproc.cvtColor(ImageMatrixOpenCV, ImageMatrixOpenCV, Imgproc.COLOR_RGB2GRAY);
        string dataFromMat = ImageMatrixOpenCV.reshape(1, 1).dump();
        imageMatrixData.ImagePixelsListMainPaintView = dataFromMat;
    }

    public void CollectPixelsValuesFromImageForNeuralNetworkDNNOfflineTraining() // NOT USING THIS
    {
        Texture2D ImageNNFrontViewTexture = ToTexture2D(ImageNNFrontViewRT);
        Mat ImageMatrixNNfrontView = new Mat(ImageNNFrontViewTexture.height, ImageNNFrontViewTexture.width, CvType.CV_8UC1); // 180 * 240 * cv_8uc1
        Utils.texture2DToMat(ImageNNFrontViewTexture, ImageMatrixNNfrontView);
        
        Imgproc.cvtColor(ImageMatrixNNfrontView, ImageMatrixNNfrontView, Imgproc.COLOR_RGB2GRAY);
        string dataFromMatFrontView = ImageMatrixNNfrontView.reshape(1, 1).dump();
        imageMatrixData.ImageNNFrontView = dataFromMatFrontView;

        Texture2D ImageNNtopViewTexture = ToTexture2D(ImageNNtopViewRT);
        Mat ImageMatrixNNtopView = new Mat(ImageNNtopViewTexture.height, ImageNNtopViewTexture.width, CvType.CV_8UC1); // 180 * 240 * cv_8uc1
        Utils.texture2DToMat(ImageNNtopViewTexture, ImageMatrixNNtopView);

        Imgproc.cvtColor(ImageMatrixNNtopView, ImageMatrixNNtopView, Imgproc.COLOR_RGB2GRAY);
        string dataFromMatTopView = ImageMatrixNNtopView.reshape(1, 1).dump();
        imageMatrixData.ImageNNtopView = dataFromMatTopView;

    }


    public byte[] CollectPNGFrontViewNN() // 20 x 20
    {
        Texture2D tex = ToTexture2D(ImageNNFrontViewRT);
        // Read screen contents into the texture
        tex.ReadPixels(new UnityEngine.Rect(0, 0, CompositionRenderTexture.width, CompositionRenderTexture.height), 0, 0);
        tex.Apply();

        // Encode texture into PNG
        byte[] bytes = tex.EncodeToPNG();
        Destroy(tex);

        return bytes;

    }

    public byte[] CollectPNGFTopViewNN() // 20 x 20 
    {
        Texture2D tex = ToTexture2D(ImageNNtopViewRT);
        // Read screen contents into the texture
        tex.ReadPixels(new UnityEngine.Rect(0, 0, CompositionRenderTexture.width, CompositionRenderTexture.height), 0, 0);
        tex.Apply();

        // Encode texture into PNG
        byte[] bytes = tex.EncodeToPNG();
        Destroy(tex);

        return bytes;

    }

    public byte[] CollectPNGMainPaintCameraFullResolution() // 240 x 180
    {
        Texture2D tex = ToTexture2D(CompositionRenderTexture);

        // Read screen contents into the texture
        tex.ReadPixels(new UnityEngine.Rect(0, 0, CompositionRenderTexture.width, CompositionRenderTexture.height), 0, 0);
        tex.Apply();

        // Encode texture into PNG
        byte[] bytes = tex.EncodeToPNG();
        Destroy(tex);

        return bytes;
    }

    private Texture2D ToTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGBA32, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new UnityEngine.Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }

}

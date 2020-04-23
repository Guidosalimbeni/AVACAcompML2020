using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.UnityUtils;
using System.IO;
using OpenCVForUnity.ImgprocModule;
using System;
//using UnityEngine.EventSystems;

public class OpenCVManager : MonoBehaviour
{
    public RenderTexture camRenderTexture;
    public int ScaleFactorScreenBalance = 4;
    public event Action<float> OnPixelsCountBalanceChanged;


    // call from leantouch and population manager one during breeding and one for last move
    // also called from the AGENTCompAi to update the reward on decision on demand..
    public void CallForOpenCVCalculationUpdates()  /// call from EVENT
    {
        CallTOCalculateVisualScoreBalancePixelsCount(); // need to be called SCREEN so it makes more sense for the paper





    }

    private void CallTOCalculateVisualScoreBalancePixelsCount()
    {
        Texture2D imgTexture = ToTexture2D(camRenderTexture);
        CalculateAreaLeftRightPixelsCountBalance(imgTexture);
    }


    private Texture2D ToTexture2D(RenderTexture rTex) ///
    {
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGBA32, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new UnityEngine.Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }

    private void CalculateAreaLeftRightPixelsCountBalance(Texture2D srcTexture) 
    {
        Mat imgMat = new Mat(srcTexture.height, srcTexture.width, CvType.CV_8UC1);
        Utils.texture2DToMat(srcTexture, imgMat);
        Imgproc.threshold(imgMat, imgMat, 1, 255, Imgproc.THRESH_BINARY);

        Size sz = new Size((int) (srcTexture.height / ScaleFactorScreenBalance), (int)(srcTexture.width / ScaleFactorScreenBalance));
        Imgproc.resize(imgMat, imgMat, sz);

        int rows = imgMat.rows(); //Calculates number of rows
        int cols = imgMat.cols(); //Calculates number of columns
        int ch = imgMat.channels(); //Calculates number of channels (Grayscale: 1, RGB: 3, etc.)

        //int totalPixelsCount = PixelCounting(imgMat, rows, 0, cols, ch);
        int totalPixelsCountLeft = PixelCounting(imgMat, rows, 0, (int)cols/2, ch);
        int totalPixelsCountRight = PixelCounting(imgMat, rows, (int)cols / 2 , cols, ch);
        float DifferenceBetweenLeftandRight = Mathf.Abs(totalPixelsCountLeft - totalPixelsCountRight);
        float visualScoreBalancePixelsCount = 1 - ((DifferenceBetweenLeftandRight) / (totalPixelsCountRight + totalPixelsCountLeft));

        if (OnPixelsCountBalanceChanged != null)
            OnPixelsCountBalanceChanged(visualScoreBalancePixelsCount);
    }

    private static int PixelCounting(Mat imgMat, int rows,int startCols, int cols, int ch)
    {
        int totalPixelsCount = 0;

        for (int i = 0; i < rows; i++)
        {
            for (int j = startCols; j < cols; j++)
            {
                double[] data = imgMat.get(i, j);
                for (int k = 0; k < ch; k++)
                {
                    if ((float)data[k] > 0)
                    {
                        totalPixelsCount += 1;
                    }
                }
            }
        }

        return totalPixelsCount;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Image Matrix", menuName = "CNN Data", order = 51)]

public class ImageMatrixData : ScriptableObject
{
    [SerializeField]
    private string imagePixelsListMainPaintWiew; // 240 x 180
    [SerializeField]
    private string imageNNtopView; // 20 x 20
    [SerializeField]
    private string imageNNFrontView; // 20 x 20

    public string ImagePixelsListMainPaintView { get { return imagePixelsListMainPaintWiew; } set { imagePixelsListMainPaintWiew = value; } }
    public string ImageNNtopView { get { return imageNNtopView; } set { imageNNtopView = value; } }
    public string ImageNNFrontView { get { return imageNNFrontView; } set { imageNNFrontView = value; } }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CompositionImage", menuName = "Main Comp Image", order = 51)]

public class CompositionImage : ScriptableObject
{
    [SerializeField]
    private Texture2D compImage;

    public Texture2D CompImage { get { return compImage; } set { compImage = value; } }
}

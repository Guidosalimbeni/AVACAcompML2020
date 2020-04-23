using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.


/*
 For a true pixel percentage of an irregular shaped object, you can render the object to a texture against some know background color 
 that would not be in the object. Rendering to a texture can be done in Pro using a RenderTexture. 
 For Unity Free, you wold have to render only the object and a background for
 a single frame and use Texture2D.ReadPixels() to get the screen pixels.
 Once you have the texture you can count the number of pixels that are not background and divide the result by 
 the total number of pixels on a screen.
 https://www.reddit.com/r/Unity3D/comments/5yj1ky/has_anyone_worked_with_opencv_unity_asset/
If you can live with an approximation, you can take corners of the bounds of the mesh, convert to world positions (Transform.TransformPoint()), then convert the result to Screen or Viewport points. Taking the minimum and maximum x and y values will give an XY axes aligned bounding rect that can be used to calculate a percentage. Here is an example script:
*/




public class CompScreenAreaCalculation : MonoBehaviour
{

    public float percentageScreenOccupiedByItem { get; set; } // used by ml-agents NOT USED

    public TypeOfScreenData CalcScreenPercentage()
    {
        TypeOfScreenData typeofscreendata = new TypeOfScreenData();

        float minX = Mathf.Infinity;
        float minY = Mathf.Infinity;
        float maxX = -Mathf.Infinity;
        float maxY = -Mathf.Infinity;

        Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;
        var v3Center = mesh.bounds.center;
        var v3Extents = mesh.bounds.extents;

        Vector3[] corners = new Vector3[8];

        corners[0] = new Vector3(v3Center.x - v3Extents.x, v3Center.y + v3Extents.y, v3Center.z - v3Extents.z);  // Front top left corner
        corners[1] = new Vector3(v3Center.x + v3Extents.x, v3Center.y + v3Extents.y, v3Center.z - v3Extents.z);  // Front top right corner
        corners[2] = new Vector3(v3Center.x - v3Extents.x, v3Center.y - v3Extents.y, v3Center.z - v3Extents.z);  // Front bottom left corner
        corners[3] = new Vector3(v3Center.x + v3Extents.x, v3Center.y - v3Extents.y, v3Center.z - v3Extents.z);  // Front bottom right corner
        corners[4] = new Vector3(v3Center.x - v3Extents.x, v3Center.y + v3Extents.y, v3Center.z + v3Extents.z);  // Back top left corner
        corners[5] = new Vector3(v3Center.x + v3Extents.x, v3Center.y + v3Extents.y, v3Center.z + v3Extents.z);  // Back top right corner
        corners[6] = new Vector3(v3Center.x - v3Extents.x, v3Center.y - v3Extents.y, v3Center.z + v3Extents.z);  // Back bottom left corner
        corners[7] = new Vector3(v3Center.x + v3Extents.x, v3Center.y - v3Extents.y, v3Center.z + v3Extents.z);  // Back bottom right corner

        for (var i = 0; i < corners.Length; i++)
        {
            var corner = transform.TransformPoint(corners[i]);
            corner = Camera.main.WorldToScreenPoint(corner);
            if (corner.x > maxX) maxX = corner.x;
            if (corner.x < minX) minX = corner.x;
            if (corner.y > maxY) maxY = corner.y;
            if (corner.y < minY) minY = corner.y;
            minX = Mathf.Clamp(minX, 0, Screen.width);
            maxX = Mathf.Clamp(maxX, 0, Screen.width);
            minY = Mathf.Clamp(minY, 0, Screen.height);
            maxY = Mathf.Clamp(maxY, 0, Screen.height);
        }

        float areaLeft;
        var width = maxX - minX;
        var height = maxY - minY;
        var area = width * height;
        percentageScreenOccupiedByItem = area / (Screen.width * Screen.height) * 100.0f;

        typeofscreendata.percentageObject = percentageScreenOccupiedByItem;

        typeofscreendata.ScreenAreaLeftObject = width * height;

        if (maxX > Screen.width / 2)
        {
            var leftmaxX = Screen.width / 2;
            areaLeft = (leftmaxX - minX) * height;
            typeofscreendata.ScreenAreaLeftObject = areaLeft;
        }
        if (minX > Screen.width / 2)
        {
            typeofscreendata.ScreenAreaLeftObject = 0.0f;
        }

        typeofscreendata.ScreenAreaRightObject = area - typeofscreendata.ScreenAreaLeftObject;


        return typeofscreendata;
    }

}

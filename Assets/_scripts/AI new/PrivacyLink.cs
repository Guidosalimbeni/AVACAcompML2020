using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrivacyLink : MonoBehaviour
{
    // http://www.guidosalimbeni.it/avaca-privacy-policy/

    public void OpenLinkPrivacy()
    {
        Application.OpenURL("http://www.guidosalimbeni.it/avaca-privacy-policy/");
    }
}

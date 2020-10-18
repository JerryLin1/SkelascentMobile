using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Advertisement.Initialize("1234567", true);
    }
    public void ShowInterstitialAd() {
        if (Advertisement.IsReady()) {
            Advertisement.Show();
        } 
        else {
            Debug.Log("Interstitial ad not ready at the moment! Please try again later!");
        }
    }
}

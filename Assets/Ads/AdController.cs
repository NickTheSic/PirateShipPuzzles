using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdController : MonoBehaviour
{

    public string gameID = "3566750";
    public string bannerID = "MainBanner";

    // Start is called before the first frame update
    void Start()
    {
        Advertisement.Initialize(gameID, true);

        StartCoroutine(ShowBanner());
    }

    IEnumerator ShowBanner()
    {
        while (!Advertisement.IsReady(bannerID))
        {
            yield return new WaitForSeconds(0.5f);
        }
        Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);
        Advertisement.Banner.Show(bannerID);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PurchasesTab : MonoBehaviour
{
    Transform tab;
    Transform purchasesScreen;
    float offset = -365;
    void Start()
    {
        tab = transform.Find("Tab");
        purchasesScreen = transform.Find("Purchases");
        
        #if UNITY_ANDROID
        purchasesScreen.Find("Restore Purchases").gameObject.SetActive(false);
        #endif
    }
    public void TogglePurchases()
    {
        transform.DOLocalMoveX(transform.localPosition.x + offset, 0.5f);
        offset *= -1;
    }
}

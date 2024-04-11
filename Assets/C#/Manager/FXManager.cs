using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXManager : MonoBehaviour
{
    public GameObject smokeEffect;
    #region Component
    public static FXManager instance;
    #endregion

    private void Awake()
    {
        instance = GetComponent<FXManager>();
    }

    public void FxSpawn()
    {
        Instantiate(smokeEffect);
    }

    IEnumerator DestroyEffect()
    {
        yield return new WaitForSeconds(0.3f);
        smokeEffect.gameObject.SetActive(false);
        yield return null;
    }
}

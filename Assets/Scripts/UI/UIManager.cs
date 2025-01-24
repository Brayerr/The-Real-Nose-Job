using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Image chargingBar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UpdateChargeBar(float current, float max)
    {
        chargingBar.fillAmount = current / max;
        if (chargingBar.fillAmount > .33f) chargingBar.color = Color.green;
        else chargingBar.color = Color.red;
    }
}

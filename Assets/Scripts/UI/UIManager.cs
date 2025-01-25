using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Image snotBar;
    [SerializeField] Image chargingBar;
    [SerializeField] Image[] sticks;
    [SerializeField] Image[] hearts;
    [SerializeField] PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UpdateChargeBar(float current, float max)
    {
        chargingBar.fillAmount = current / max;
        //if (chargingBar.fillAmount > .33f) chargingBar.color = Color.green;
        //else chargingBar.color = Color.red;
        if (current > player.getMinChargeAmount()) chargingBar.color = Color.green;
        else chargingBar.color = Color.red;
    }

    public void UpdateSnotBar(float current, float max)
    {
        snotBar.fillAmount = current / max;
    }

    public void updateStickIcons(int currentStickAmount)
    {
        sticks[currentStickAmount - 1].color = Color.black;
    }
    public void updateHeartsAmount(int health)
    {
        sticks[health].color = Color.black;
    }
}

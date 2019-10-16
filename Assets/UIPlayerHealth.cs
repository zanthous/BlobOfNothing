using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHealth : MonoBehaviour
{
    private Image image;

    [SerializeField]
    private Health playerHealth;
    [SerializeField]
    private Text text;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        image.fillAmount = (playerHealth.CurrentHealth / playerHealth.GetMaxHealth());
        text.text = playerHealth.CurrentHealth.ToString() + " / " + playerHealth.GetMaxHealth().ToString();
    }

}

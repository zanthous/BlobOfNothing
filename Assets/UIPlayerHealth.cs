using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHealth : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Text text;

    private Image image;

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public TextMeshProUGUI textField;
    public void UpdateText() {
        textField.text = gameObject.GetComponent<Slider>().value.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DisplayMessage : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;

    #region Observer
    private void OnEnable()
    {
        Signatures.DisplayMessage.AddListener(DisplayMessageOnCanvas);
    }
    private void OnDisable()
    {
        Signatures.DisplayMessage.RemoveListener(DisplayMessageOnCanvas);
    }
    #endregion

    private void DisplayMessageOnCanvas(string msg)
    {
        this.text.text = msg;
    }
}

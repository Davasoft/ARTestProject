using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class Asset3DModel : MonoBehaviour,IControlHandler
{
    public void TurnOffControllers()
    {
        // Disable ArTranslationInteractable component
        ARTranslationInteractable translationInteractable = GetComponent<ARTranslationInteractable>();
        if (translationInteractable != null)
        {
            translationInteractable.enabled = false;
        }

        // Disable ARScaleInteractable component
        ARScaleInteractable scaleInteractable = GetComponent<ARScaleInteractable>();
        if (scaleInteractable != null)
        {
            scaleInteractable.enabled = false;
        }

        // Disable ARSelectionInteractable component
        ARSelectionInteractable selectionInteractable = GetComponent<ARSelectionInteractable>();
        if (selectionInteractable != null)
        {
            selectionInteractable.enabled = false;
        }

        // Disable ArRotationInteractable component
        ARRotationInteractable rotationInteractable = GetComponent<ARRotationInteractable>();
        if (rotationInteractable != null)
        {
            rotationInteractable.enabled = false;
        }
    }
}

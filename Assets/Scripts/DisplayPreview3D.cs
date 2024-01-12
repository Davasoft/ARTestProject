using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPreview3D : MonoBehaviour
{
    private GameObject previewModel;
    [SerializeField] Transform spawnPosition;
    [SerializeField] Slider slider;
    private Coroutine sliderAnimationCoroutine;

    #region Observer
    private void OnEnable()
    {
        Signatures.LoadPreviewAsset.AddListener(LoadPreviewAsset);
        Signatures.DeletePreviewAsset.AddListener(DeletePreviewAsset);
        Signatures.LoadPreviewProgress.AddListener(LoadPreviewProgress);
        Signatures.SetPreviewProgress.AddListener(SetPreviewProgress);
    }

    private void OnDisable()
    {
        Signatures.LoadPreviewAsset.RemoveListener(LoadPreviewAsset);
        Signatures.DeletePreviewAsset.RemoveListener(DeletePreviewAsset);
        Signatures.LoadPreviewProgress.RemoveListener(LoadPreviewProgress);
        Signatures.SetPreviewProgress.RemoveListener(SetPreviewProgress);
    }
    #endregion

    void Update()
    {
        // Check if previewModel is not null and rotate it around the y-axis
        if (previewModel != null)
        {
            RotatePreviewModel();
        }
    }

    void RotatePreviewModel()
    {
        // Rotate the previewModel around the y-axis continuously
        float rotationSpeed = 30f; // Adjust the rotation speed as needed
        previewModel.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    void LoadPreviewAsset(GameObject obj)
    {
        if (previewModel)
            DeletePreviewAsset();

        // Example: Instantiate a model prefab (replace "YourModelPrefab" with the actual prefab)
        previewModel = Instantiate(obj, spawnPosition.position, Quaternion.identity);

        // Check if the spawned model implements IControls interface
        IControlHandler controlsInterface = previewModel.GetComponent<IControlHandler>();
        if (controlsInterface != null)
        {
            // Call the TurnOffControllers function from the IControls interface
            controlsInterface.TurnOffControllers();
        }
        // Set the layer for the main object and its children recursively
        SetLayerRecursively(previewModel, LayerMask.NameToLayer("Preview"));

    }

    void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }

    void DeletePreviewAsset()
    {
        // Check if there is a spawned model
        if (previewModel != null)
        {
            // Destroy the spawned model
            Destroy(previewModel);
        }
    }
    void SetPreviewProgress(float targetvalue)
    {
        slider.value = targetvalue;
    }
    void LoadPreviewProgress(float targetValue)
    {
        // Stop the existing coroutine if it's running
        if (sliderAnimationCoroutine != null)
        {
            StopCoroutine(sliderAnimationCoroutine);
        }

        // Start a new coroutine for the slider animation
        sliderAnimationCoroutine = StartCoroutine(AnimateSlider(targetValue));
    }

    IEnumerator AnimateSlider(float targetValue)
    {
        float duration = 0.2f;
        float startTime = Time.time;
        float startValue = slider.value;

        while (Time.time < startTime + duration)
        {
            float progress = (Time.time - startTime) / duration;
            slider.value = Mathf.Lerp(startValue, targetValue, progress);

            yield return null;
        }

        // Ensure the slider reaches the exact target value
        slider.value = targetValue;

        // Reset the coroutine reference
        sliderAnimationCoroutine = null;
    }
}

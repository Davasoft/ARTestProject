using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

/// <summary>
/// This class handles loading addressable objects from the Digital Ocean cloud, providing progress tracking.
/// </summary>
public class AddressableLoader : MonoBehaviour
{
    // Singleton instance
    public static AddressableLoader instance;

    // Event for download progress
    public class DownloadProgressEvent : UnityEngine.Events.UnityEvent<float> { }
    public DownloadProgressEvent onDownloadProgress = new DownloadProgressEvent();

    /// <summary>
    /// Initializes the singleton instance and ensures it persists across scenes.
    /// </summary>
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Loads an addressable object asynchronously with progress tracking.
    /// </summary>
    /// <param name="addressableReference">The AssetReference pointing to the addressable object.</param>
    /// <param name="onComplete">Callback function invoked when the object is fully loaded.</param>
    /// <param name="onProgress">Callback function invoked with the download progress.</param>
    public void LoadAddressableObject(AssetReference addressableReference, Action<GameObject> onComplete = null, Action<float> onProgress = null)
    {
        StartCoroutine(TrackDownloadProgress(addressableReference, onComplete, onProgress));
    }

    /// <summary>
    /// Coroutine that tracks the download progress of an addressable object.
    /// </summary>
    /// <param name="addressableReference">The AssetReference pointing to the addressable object.</param>
    /// <param name="onComplete">Callback function invoked when the object is fully loaded.</param>
    /// <param name="onProgress">Callback function invoked with the download progress.</param>
    private IEnumerator TrackDownloadProgress(AssetReference addressableReference, Action<GameObject> onComplete, Action<float> onProgress)
    {
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(addressableReference);

        // Keep checking the download progress until it's complete
        while (!handle.IsDone)
        {
            // Calculate and invoke the download progress event
            float progress = handle.PercentComplete;
            onDownloadProgress.Invoke(progress);

            // Invoke the provided progress callback
            onProgress?.Invoke(progress);

            yield return null;
        }

        // Once the download is complete, invoke the completion callback
        if (onComplete != null)
        {
            onComplete.Invoke(handle.Result);
        }
    }
}

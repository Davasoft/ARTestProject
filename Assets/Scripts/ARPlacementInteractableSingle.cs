using UnityEngine.XR.Interaction.Toolkit.AR;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

/// <summary>
/// Handles AR object placement and asset loading in an AR environment using Unity's XR Interaction Toolkit.
/// </summary>
public class ARPlacementInteractableSingle : ARBaseGestureInteractable
{
    [SerializeField]
    private List<AssetReference> assetPrefabs;

    [SerializeField]
    private List<Button> buttons;

    [SerializeField]
    private Button deleteButton;

    [SerializeField]
    private Button defaultPrefabButton;

    [SerializeField]
    private GameObject defaultPrefab;

    [SerializeField]
    private GameObject placementPrefab;

    [SerializeField]
    private ARObjectPlacementEvent onObjectPlaced;

    private GameObject placementObject;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private static GameObject trackablesObject;

    /// <summary>
    /// Initializes button click listeners and sets up the default prefab.
    /// </summary>
    private void Start()
    {
        defaultPrefabButton.onClick.AddListener(() => SetLoadedAsset(this.defaultPrefab));
        deleteButton.onClick.AddListener(() => DeleteObject());

        for (int i = 0; i < buttons.Count; i++)
        {
            int index = i;  // Capture the current value of i in a local variable for the callback
            buttons[i].onClick.AddListener(() => ChangePlacementPrefab(assetPrefabs[index]));
        }
    }

    #region Object Placement
    /// <summary>
    /// Determines if manipulation (placement) can start for a tap gesture.
    /// </summary>
    /// <param name="gesture">The tap gesture to check.</param>
    /// <returns>True if manipulation can start, false otherwise.</returns>
    protected override bool CanStartManipulationForGesture(TapGesture gesture)
    {
        if (gesture.targetObject == null)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Handles the end of manipulation (tap gesture) to place an object in the AR environment.
    /// </summary>
    /// <param name="gesture">The tap gesture that ended.</param>
    protected override void OnEndManipulation(TapGesture gesture)
    {
        if (gesture.targetObject != null)
        {
            Debug.Log("targetobject != null");
            return;
        }

        if (GestureTransformationUtility.Raycast(gesture.startPosition, hits, xrOrigin, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;

            // Allow a new game object for AR placement object
            if (placementObject == null)
            {
                placementObject = PlaceObject(hitPose);

                Debug.Log("Placement object " + placementObject.name);
                if (placementObject.GetComponent<ARTranslationInteractable>() != null)
                {
                    Debug.Log("===== ARTranslationInteractable =======");
                }
                else
                {
                    Debug.Log("The component is not available");
                    // The component is not available
                }
            }
        }
    }

    /// <summary>
    /// Places an AR object at the specified pose in the AR environment.
    /// </summary>
    /// <param name="pose">The pose (position and rotation) for object placement.</param>
    /// <returns>The instantiated placement object.</returns>
    protected virtual GameObject PlaceObject(Pose pose)
    {
        if (placementPrefab == null)
        {
            Debug.Log("placementPrefab is null");
        }
        else
        {
            Debug.Log("Not null");
        }

        var placementObject = Instantiate(placementPrefab, pose.position, pose.rotation);

        // Create anchor to track reference point and set it as the parent of placementObject.
        var anchor = new GameObject("PlacementAnchor").transform;
        anchor.position = pose.position;
        anchor.rotation = pose.rotation;
        placementObject.transform.parent = anchor;

        // Use Trackables object in the scene to use as parent
        var trackablesParent = xrOrigin != null
            ? xrOrigin.TrackablesParent
            : (xrOrigin != null ? xrOrigin.TrackablesParent : null);

        if (trackablesParent != null)
        {
            anchor.parent = trackablesParent;
        }

        Debug.Log("Object placed");
        return placementObject;
    }
    #endregion

    #region Load Asset
    /// <summary>
    /// Changes the placement prefab based on the selected asset reference.
    /// </summary>
    /// <param name="obj">The asset reference for the new placement prefab.</param>
    private void ChangePlacementPrefab(AssetReference obj)
    {
        Signatures.SetPreviewProgress.Invoke(0);
        AddressableLoader.instance?.LoadAddressableObject(obj, SetLoadedAsset, DisplayLoadProgress);
    }

    /// <summary>
    /// Sets the loaded asset as the new placement prefab.
    /// </summary>
    /// <param name="obj">The loaded GameObject asset.</param>
    private void SetLoadedAsset(GameObject obj)
    {
        this.placementPrefab = obj;
        Signatures.LoadPreviewAsset.Invoke(obj);
        Signatures.LoadPreviewProgress.Invoke(1);
    }

    /// <summary>
    /// Displays the load progress of the asset.
    /// </summary>
    /// <param name="progress">The load progress value between 0 and 1.</param>
    private void DisplayLoadProgress(float progress)
    {
        Signatures.LoadPreviewProgress.Invoke(progress);
    }

    /// <summary>
    /// Deletes the placed object from the AR environment.
    /// </summary>
    private void DeleteObject()
    {
        if (this.placementObject != null)
        {
            Addressables.ReleaseInstance(this.placementObject);
            Destroy(this.placementObject);
        }
    }
    #endregion
}

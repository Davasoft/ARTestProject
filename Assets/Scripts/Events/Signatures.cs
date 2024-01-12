using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains event signatures for various stages in the application.
/// </summary>
public static class Signatures
{
    #region Preview

    public static readonly Subject<GameObject> LoadPreviewAsset = new Subject<GameObject>();
    public static readonly Subject<float> LoadPreviewProgress = new Subject<float>();
    public static readonly Subject<float> SetPreviewProgress = new Subject<float>();
    public static readonly Subject DeletePreviewAsset = new Subject();

    #endregion

    #region Display Message

    public static readonly Subject<string> DisplayMessage = new Subject<string>();

    #endregion
}

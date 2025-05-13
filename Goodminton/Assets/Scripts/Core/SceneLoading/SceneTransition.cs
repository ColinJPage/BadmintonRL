using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
/*!Struct that stores a SceneTransitionStyleSO and scenefield, used for sceneloading
 */
public struct SceneTransition
{
    public SceneTransitionStyleSO style;
    public SceneField sceneField;

    public SceneTransition(SceneField _sceneField, SceneTransitionStyleSO _style)
    {
        style = _style;

        sceneField = _sceneField;
    }
}

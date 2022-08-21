using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Reflection;

/// <summary>
/// Class that helps in editor mode. Will activate the current menu selected in the Hierarchy,
/// while deactivating all the other menus.
/// </summary>
[ExecuteAlways]
public class Menu : MonoBehaviour
{
    [ReadOnly]
    [SerializeField]
    private Button[] buttons;

    private void OnValidate ()
    {
        Selection.selectionChanged += SelectMenu;

        buttons = GetComponentsInChildren<Button>(true);
    }

    private void SelectMenu ()
    {
        if (!this) {
            Selection.selectionChanged -= SelectMenu;
            return;
        }

        if (!Application.isEditor)
            return;

        if (Application.isPlaying)
            return;

        if (!Selection.activeGameObject)
            return;

        if (!gameObject)
            return;

        if (Selection.activeGameObject != gameObject)
            return;

        CloseAllButMe();
    }

    public void Open ()
    {
        CloseAllButMe();
    }

    private void CloseAllButMe ()
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        if (!canvas)
            return;
        
        Menu[] menus = canvas.GetComponentsInChildren<Menu>(true);

        foreach (var item in menus) {
            item.gameObject.SetActive (false);
        }
        gameObject.SetActive (true);
    }
}
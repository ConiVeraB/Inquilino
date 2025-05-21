using UnityEditor;
using UnityEngine;

public class SetNavigationStatic : MonoBehaviour
{
    [ContextMenu("Marcar como Navigation Static")]
    void MarcarStatic()
    {
        
        GameObject obj = this.gameObject;
       // GameObjectUtility.SetStaticEditorFlags(obj, StaticEditorFlags.NavigationStatic);
        Debug.Log($"{obj.name} marcado como Navigation Static.");
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public class KitchenObjectSO : using UnityEngine;

// [CreateAssetMenu(fileName = "KitchenObjectSO", menuName = "~/Documents/Unity/KitchenChoas/KitchenChaos/Assets/Scripts/KitchenObjectSO.cs/KitchenObjectSO", order = 0)]
[CreateAssetMenu()]
public class KitchenObjectSO : ScriptableObject {
    
    public Transform prefab;
    public Sprite sprite;
    public string objectName;

} 

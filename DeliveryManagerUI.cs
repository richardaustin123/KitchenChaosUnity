using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour {

    [SerializeField] private Transform container;
    [SerializeField] private Transform recipeTemplate;

    // Awake()
    private void Awake() {
        recipeTemplate.gameObject.SetActive(false);
    }

    // Start()
    private void Start() {
        DeliveryManager.Instance.OnRecipeSpawned    += DeliveryManager_OnRecipeSpawned;
        DeliveryManager.Instance.OnRecipeCompleted  += DeliveryManager_OnRecipeCompleted;

        UpdateVisual();
    }

    // DeliveryManager_OnRecipeSpawned()
    private void DeliveryManager_OnRecipeSpawned(object sender,   System.EventArgs e) {
        UpdateVisual();
    }

    // DeliveryManager_OnRecipeCompleted()
    private void DeliveryManager_OnRecipeCompleted(object sender, System.EventArgs e) {
        UpdateVisual();
    }

    // UpdateVisual()
    private void UpdateVisual() {
        foreach(Transform child in container) {
            if (child == recipeTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach(RecipeSO recipeSO in DeliveryManager.Instance.GetWaitingRecipeSOList()) {
            Transform recipeTransform = Instantiate(recipeTemplate, container);
            recipeTransform.gameObject.SetActive(true);
            recipeTransform.GetComponent<DeliveryManagerSingleUI>().SetRecipeSO(recipeSO);
        }
    }
}

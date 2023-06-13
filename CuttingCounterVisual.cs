using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour {

    private const string CUT = "Cut";

    [SerializeField] private CuttingCounter cuttingCounter;

    private Animator animator;

    // Awake()
    private void Awake() {
        animator = GetComponent<Animator>();
    }

    // Start()
    private void Start() {
        cuttingCounter.OnCut += CuttingCounter_OnCut;
    }

    // CuttingCounter_OnCut()
    private void CuttingCounter_OnCut(object sender, System.EventArgs e) {
        animator.SetTrigger(CUT);
    }
}

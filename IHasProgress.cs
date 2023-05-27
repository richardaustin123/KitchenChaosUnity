using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasProgress {
    
    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;
    public class OnProgressChangedEventArgs : System.EventArgs {
        public float progressNormalized;
    }
}
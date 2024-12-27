using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISavable
{
    object CaptureState();
    void RestoreState(object state);
}
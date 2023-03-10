using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVesselState
{
    void OnStart(GalleonStateMachine galleon);
    void OnUpdate();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasFish
{
    public void AddFish(Fish fish);

    public void RemoveFish(Fish fish);

    public void SetFish(Fish fish);

    public Transform GetParentTransform();

    public Transform GetTransform();
}

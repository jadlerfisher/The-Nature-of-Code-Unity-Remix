using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface  IMoveable<T,U,V,W>
{


    void move(T location, U velocity, V acceleration, W topSpeed);


}

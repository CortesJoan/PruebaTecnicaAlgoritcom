using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseParentOnStart : MonoBehaviour
{
     void Start()
     {
         this.transform.parent = null;
     }

    
}

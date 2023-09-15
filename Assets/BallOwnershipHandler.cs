using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallOwnershipHandler : MonoBehaviour
{
   [Header("References")]
   private PlayerBallHandler playerOwner;
   private PlayerBallHandler previousOwner;

   [SerializeField] private float minimumDistanceToLoseOwner = 1f;
   public bool HasOwner()
   {
      return playerOwner != null;
   }

   public void SetOwner(PlayerBallHandler playerOwner)
   {
      previousOwner = this.playerOwner;
      this.playerOwner  = playerOwner;
   }

   public PlayerBallHandler GetOwner()
   {
      return playerOwner;
   }
   public PlayerBallHandler GetPreviousOwner()
   {
      return playerOwner;
   }

   private void OnTriggerEnter(Collider other)
   {
      if (playerOwner)
      {
         var ownerDistance = Vector3.Distance(playerOwner.transform.position, transform.position);
         if (ownerDistance >= minimumDistanceToLoseOwner)
         {
            SetOwner(null);
         }
      }
   }
}

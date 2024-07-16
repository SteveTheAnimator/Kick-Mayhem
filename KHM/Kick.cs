using Cinemachine.Utility;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static KickMayhem.Plugin;

namespace KickMayhem
{
    public class Kick : MonoBehaviour
    {
        public float Delay = 0f;
        public void OnTriggerEnter(Collider other)
        {
            if(this.gameObject.GetComponent<PhotonView>().IsMine == false)
            {
                if(other.TryGetComponent(out GorillaTriggerColliderHandIndicator component))
                {
                    PhotonNetwork.Disconnect();
                }
            }
        }
    }
}

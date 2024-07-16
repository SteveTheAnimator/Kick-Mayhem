using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BepInEx;
using Photon.Pun;
using UnityEngine;
using Utilla;

namespace KickMayhem
{
    [ModdedGamemode("Kick Mayhem", "Kick Mayhem", Utilla.Models.BaseGamemode.Casual)]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
	[BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
	public class Plugin : BaseUnityPlugin
	{
		bool inRoom;
		List<GameObject> Hammers = new List<GameObject>();

        void Start()
		{
			Utilla.Events.GameInitialized += OnGameInitialized;
		}

		void OnEnable()
		{
			HarmonyPatches.ApplyHarmonyPatches();
		}

		void OnDisable()
		{
			HarmonyPatches.RemoveHarmonyPatches();
		}

		void OnGameInitialized(object sender, EventArgs e)
		{
            Debug.Log("Kick Mayhem Loaded!");
		}

        void Update()
		{
			if(inRoom)
			{
                foreach (var VRRig in GameObject.FindObjectsOfType<VRRig>())
                {
                    if (VRRig.rightHand.rigTarget.transform.gameObject.transform.FindChild("Hammer") == null && !VRRig.isLocal)
                    {
						GameObject Hammer = GameObject.CreatePrimitive(PrimitiveType.Cube);
						Hammer.AddComponent<Kick>();
                        Hammer.AddComponent<PhotonView>();
						Hammer.GetComponent<PhotonView>().TransferOwnership(VRRig.Creator);
                        Hammer.GetComponent<BoxCollider>().isTrigger = true;
                        UnityEngine.Object.Destroy(Hammer.GetComponent<Rigidbody>());
                        Hammer.transform.parent = VRRig.rightHand.rigTarget.transform;
                        Hammer.transform.position = VRRig.rightHand.rigTarget.transform.position;
						Hammer.layer = 18;
                        Hammers.Add(Hammer);
						Hammer.name = "Hammer";
                        Hammer.GetComponent<Renderer>().material.shader = Shader.Find("GorillaTag/UberShader");
                        Hammer.GetComponent<Renderer>().material.color = VRRig.playerColor;
                        Hammer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                    }
					else
					{
						if(VRRig.isLocal && GorillaTagger.Instance.rightHandTransform.FindChild("Hammer") == null)
						{
							GameObject Hammer = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            UnityEngine.Object.Destroy(Hammer.GetComponent<Rigidbody>());
                            UnityEngine.Object.Destroy(Hammer.GetComponent<BoxCollider>());
							Hammer.transform.parent = GorillaTagger.Instance.rightHandTransform;
							Hammer.transform.localPosition = Vector3.zero; 
                            Hammer.name = "Hammer";
                            Hammer.GetComponent<Renderer>().material.shader = Shader.Find("GorillaTag/UberShader");
							Hammer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                            Hammers.Add(Hammer);
                            Hammer.GetComponent<Renderer>().material.color = GorillaTagger.Instance.offlineVRRig.playerColor;
                            Hammer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                        }
					}
                }
            }
			else
			{
				foreach (GameObject skibid in Hammers)
				{
					if(skibid != null)
					{
						Hammers.Remove(skibid);
                        GameObject.Destroy(skibid);
                    }
				}
			}
		}

		[ModdedGamemodeJoin]
		public void OnJoin(string gamemode)
		{
			inRoom = true;
		}

		[ModdedGamemodeLeave]
		public void OnLeave(string gamemode)
		{
			inRoom = false;
		}
	}
}

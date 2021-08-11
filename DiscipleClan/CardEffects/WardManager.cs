﻿using HarmonyLib;
using Trainworks;
using Trainworks.Managers;
using ShinyShoe;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace DiscipleClan.CardEffects
{
    public class WardManager
    {
        public static List<List<WardState>> wardStates;
        public static List<List<WardState>> incomingWards;
        public static WardUI ui;
        public static RoomCapacityObject prefab;

        public static void SetupUI()
        {
            // Extremely janky construction
            GameObject wardUI = new GameObject("Ward Container");
            ProviderManager.TryGetProvider<RoomManager>(out RoomManager roomManager);
            wardUI.transform.SetParent(roomManager.GetRoomUI().GetRoomCapacityUI().transform.parent);
            wardUI.transform.SetScale(1, 1, 1);
            wardUI.transform.SetLocalPosition(-700, 175, 0);

            ui = wardUI.AddComponent<WardUI>();
        }

        public static void ResetWards()
        {
            wardStates = new List<List<WardState>>
            {
                new List<WardState>(),
                new List<WardState>(),
                new List<WardState>(),
                new List<WardState>(),
            };

            incomingWards = new List<List<WardState>>
            {
                new List<WardState>(),
                new List<WardState>(),
                new List<WardState>(),
                new List<WardState>(),
            };
        }

        public static void AddWard(WardState ward, int floor)
        {
            if (0 <= floor && floor <= 3)
            {
                wardStates[floor].Add(ward);
                ward.OnAdd(floor);

                ui.SetupWardIcons(floor);
            }
        }

        public static void AddWardLater(WardState ward, int floor)
        {
            if (0 <= floor && floor <= 3)
            {
                incomingWards[floor].Add(ward);
            }
        }

        public static List<WardState> GetWards(int floor)
        {
            return wardStates[floor];
        }

        public static IEnumerator TriggerWards(string ID = "", int floor = -1, List<CharacterState> targets = null)
        {
            if (floor != -1)
            {
                foreach (var ward in wardStates[floor].ToArray())
                {
                    if (ID != "")
                    {
                        if (ward.ID == ID)
                        {
                            yield return ward.OnTrigger(targets);
                        }
                    }
                    else
                    {
                        yield return ward.OnTrigger(targets);
                    }
                }
            }
            else
            {
                foreach (var floorWards in wardStates)
                {
                    foreach (var ward in floorWards)
                    {
                        if (ID != "")
                        {
                            if (ward.ID == ID)
                            {
                                yield return ward.OnTrigger(targets);
                            }
                        }
                        else
                        {
                            yield return ward.OnTrigger(targets);
                        }
                    }
                }
            }

            // Avoids list breaking issues
            int i = 0;
            foreach (var wardFloor in incomingWards)
            {
                foreach (var ward in wardFloor.ToArray())
                {
                    AddWard(ward, i);
                }
                i++;
                wardFloor.Clear();
            }
            yield break;
        }

        public static void TriggerWardsNow(string ID = "", int floor = -1, List<CharacterState> targets = null)
        {
            if (floor != -1)
            {
                foreach (var ward in wardStates[floor])
                {
                    if (ID != "")
                    {
                        if (ward.ID == ID)
                        {
                            ward.OnTriggerNow(targets);
                        }
                    }
                    else
                    {
                         ward.OnTriggerNow(targets);
                    }
                }
            }
            else
            {
                foreach (var floorWards in wardStates)
                {
                    foreach (var ward in floorWards)
                    {
                        if (ID != "")
                        {
                            if (ward.ID == ID)
                            {
                                 ward.OnTriggerNow(targets);
                            }
                        }
                        else
                        {
                             ward.OnTriggerNow(targets);
                        }
                    }
                }
            }

            // Avoids list breaking issues
            int i = 0;
            foreach (var floorWards in incomingWards)
            {
                foreach (var ward in floorWards)
                {
                    AddWard(ward, i);
                }
                i++;
                floorWards.Clear();
            }

        }
    }
    
    public class WardUI : MonoBehaviour
    {
        private float fixedBufferWidth = 52f;
        private float pipIndicatorWidth = 48f;
        private float pipIndicatorBufferWidth = 52f;

        public void SetupWardIcons(int floor)
        {
            // Destroy children
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            // Now we recreate children based on the floor
            int i = 0;
            foreach (WardState ward in WardManager.GetWards(floor))
            {
                // Create a new GameObject
                /*GameObject icon = UnityEngine.Object.Instantiate(WardManager.prefab, Vector3.zero, Quaternion.identity, base.transform).gameObject;
                GameObject.Destroy(icon.GetComponent<RoomCapacityObject>());
                foreach (Transform child in icon.transform)
                {
                    Destroy(child.gameObject);
                }*/
                GameObject icon = new GameObject("Ward Icon");
                icon.transform.SetParent(base.transform);
                Image iconImage = icon.AddComponent<Image>();

                // Set the image
                // Image iconImage = icon.GetComponent<Image>();
                iconImage.sprite = ward.wardIcon;

                // Set the Image Offset 
                icon.transform.localPosition = Vector3.zero;

                RectTransform component = icon.GetComponent<RectTransform>();
                Vector2 offsetMax = component.offsetMax;
                offsetMax.x = GetContainerWidth(i);
                component.offsetMax = offsetMax;
                component.sizeDelta = new Vector2 { x = 0.48f, y = 0.82f };
                i++;

                // Add the tooltip
                var tooltip = icon.AddComponent<LocalizedTooltipProvider>();
                var t = Traverse.Create(tooltip);
                t.Field("tooltipTitleKey").SetValue(ward.tooltipTitleKey);
                t.Field("tooltipBodyKey").SetValue(ward.tooltipBodyKey);
                icon.AddComponent<GameUISelectable>();
            }
        }

        private float GetContainerWidth(int count)
        {
            return -(fixedBufferWidth * 2f + pipIndicatorWidth * (float)count + pipIndicatorBufferWidth * (float)(count - 1));
        }
    }

    // Reset the Ward List
    [HarmonyPatch(typeof(ScenarioManager), "SetupRooms")]
    class WardManagerReset
    {
        static void Postfix(RoomManager __instance)
        {
            WardManager.ResetWards();
        }
    }

    // Add the Ward UI on construction of the RoomUI
    [HarmonyPatch(typeof(RoomUI), "Start")]
    class WardManagerUISetup
    {
        static void Postfix(RoomUI __instance)
        {
            WardManager.SetupUI();
        }
    }

    // Setup the Ward Icons on floor change
    [HarmonyPatch(typeof(RoomUIDisplayBase), "Show")]
    class WardManagerSetup
    {
        static void Postfix(RoomUIDisplayBase __instance, RoomState room)
        {
            if (room == null || !room.IsRoomEnabled()) { return; }
            WardManager.ui.gameObject.SetActive(true);
            WardManager.ui.SetupWardIcons(room.GetRoomIndex());
        }
    }

    [HarmonyPatch(typeof(RoomUIDisplayBase), "Hide")]
    class WardManagerFloorHide
    {
        static void Postfix(RoomUIDisplayBase __instance)
        {
            if (WardManager.ui != null)
            {
                WardManager.ui.gameObject.SetActive(false);
            }
        }
    }
}

using System;
using System.Collections;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Reflection;
using UnityEditor;
using MonsterTrainModdingAPI.Managers;
using MonsterTrainModdingAPI.Utilities;


// Prefab is just a unity assetbundle named 'prefab' with a single prefab in it called 'canvas', located in the plugin top level directory


namespace AnnoyingDoge
{
    // Credit to Rawsome, Stable Infery for the base of this method.
    [BepInPlugin("io.github.crazyjackel.ADP", "Annoying Dog Plugin", "1.0")]
    [BepInProcess("MonsterTrain.exe")]
    [BepInProcess("MtLinkHandler.exe")]
    [BepInDependency("api.modding.train.monster")]
    public class TestPlugin : BaseUnityPlugin
    {
        public GameObject Canvas;
        public GameObject AnnoyingDoge;
        public RectTransform AnnoyingDogeTransform;
        void Awake()
        {
            var harmony = new Harmony("io.github.crazyjackel.ADP");
            harmony.PatchAll();


            Canvas = GameObject.Instantiate(AssetBundleUtils.LoadAssetFromPath<GameObject>("prefab", "canvas"));
            AnnoyingDoge = Canvas.transform.Find("Image").gameObject;
            AnnoyingDogeTransform = (RectTransform)AnnoyingDoge.transform;
            DontDestroyOnLoad(Canvas);


        }

        void Start()
        {
            foreach (string s in PluginManager.GetAllPluginGUIDs())
            {
                Console.WriteLine(s);
            }
        }

        void Update()
        {

            AnnoyingDogeTransform.position = new Vector3(Screen.width / 2 + (Screen.width / 2) * (float)Math.Sin(4 * Time.time / 7), Screen.height / 2 + (Screen.height / 2) * (float)Math.Cos(4 * Time.time / 11), 0);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace FS
{

    public class ResourceManager : MonoBehaviour
    {
        private const string PREFIX_BUNDLE = "Assets/Bundles/";
        private static Dictionary<string, UnityEngine.Object> resources = new Dictionary<string, UnityEngine.Object>();

        private List<string> loadAssetStatus = new List<string>();

        public static ResourceManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<ResourceManager>();
                    DontDestroyOnLoad(_instance.gameObject);
                    _instance.Init();
                }
                return _instance;
            }
        }
        private static ResourceManager _instance;
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                DestroyImmediate(gameObject);
                return;
            }
            else if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            this.Init();
        }

        private void Init()
        {
        }

        public static string GetAssetKey(string assetPath)
        {
            return PREFIX_BUNDLE + assetPath;
        }

        public void ClearCacheResource()
        {
            foreach (KeyValuePair<string, UnityEngine.Object> data in resources)
            {
                Addressables.Release(data.Value);
            }
            resources.Clear();
        }

        public static void Release(string key)
        {
            if (resources.ContainsKey(key))
            {
                Addressables.Release(resources[key]);
                resources.Remove(key);
            }
        }

        /// <summary>
        /// GetAsset async.
        /// https://docs.unity3d.com/Packages/com.unity.addressables@0.7/manual/AddressableAssetsAsyncOperationHandle.html
        /// </summary>
        /// <typeparam name="T">UnityEngine.Object</typeparam>
        /// <param name="assetPath">asset path with out "Assets/Bundles/"</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public void GetAsset<T>(string assetPath, Action<T> callback) where T : UnityEngine.Object
        {
            StartCoroutine(IEGetAsset(assetPath, callback));
        }

        /// <summary>
        /// GetAsset async.
        /// https://docs.unity3d.com/Packages/com.unity.addressables@0.7/manual/AddressableAssetsAsyncOperationHandle.html
        /// </summary>
        /// <typeparam name="T">UnityEngine.Object</typeparam>
        /// <param name="assetPath">asset path with out "Assets/Bundles/"</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public IEnumerator IEGetAsset<T>(string assetPath, Action<T> callback) where T : UnityEngine.Object
        {
            if (assetPath == null)
                assetPath = "";
            if (resources.ContainsKey(assetPath))
            {
                callback?.Invoke(resources[assetPath] as T);
                yield break;
            }
            else
            {
                // loadAssetStatus is use for prevent load asset multiple times.
                // I need to load object just one time.
                if (loadAssetStatus.Contains(assetPath))
                {
                    yield return new WaitUntil(() => loadAssetStatus.Contains(assetPath) == false);
                    callback?.Invoke(resources[assetPath] as T);
                    yield break;
                }
                else
                {
                    loadAssetStatus.Add(assetPath);

                    AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(ResourceManager.GetAssetKey(assetPath));
                    yield return handle;
                    loadAssetStatus.Remove(assetPath);
                    if (handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        if (resources.ContainsKey(assetPath) == false)
                        {
                            resources.Add(assetPath, handle.Result);
                        }
                        callback?.Invoke(handle.Result);
                    }
                    else
                    {
                        Debug.LogError("asset not found." + " " + assetPath);
                        callback?.Invoke(null);
                    }
                }
            }
        }

    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace FS
{

    public class LoaderManager : MonoBehaviour
    {
        [SerializeField] private ProgressBarUI progressBar;
        // Start is called before the first frame update
        IEnumerator Start()
        {
            UpdateProgress(0);
            yield return InitAddressable();
            yield return DownloadBundle("default", UpdateProgress);

            ResourceManager.Instance.GetAsset<GameConfig>("Data/GameConfig.asset", (config) =>
            {
                if (config != null)
                {
                    DataManager.Instance.SetConfig(config);
                    SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
                }
            });
        }
        private void UpdateProgress(float progress)
        {
            this.progressBar.SetProgress(progress);
        }
        IEnumerator InitAddressable(Action onComplete = null)
        {
            AsyncOperationHandle<IResourceLocator> handle = Addressables.InitializeAsync();
            Debug.Log("InitAddressable");
            yield return handle;
            onComplete?.Invoke();
        }

        IEnumerator DownloadBundle(string label, Action<float> OnProgress = null, Action onComplete = null)
        {
            AsyncOperationHandle loadDependencyHandle = Addressables.DownloadDependenciesAsync(label);
            while (loadDependencyHandle.IsDone == false)
            {
                OnProgress?.Invoke(loadDependencyHandle.PercentComplete);
                yield return null;
            }
            OnProgress?.Invoke(1);
            onComplete?.Invoke();
        }
    }

}
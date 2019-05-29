using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A Singleton that controls loading between different scenes.
/// </summary>
public class LoadingManager : Singleton<LoadingManager>
{
	[SerializeField]
	private string loadingSceneName;
    [SerializeField]
    private string defaultSceneName;

	private bool waitingForLoadingConnection = false;
	private bool loading = false;
	private LoadingController currentLoadingController;

	private string currentSceneToLoad;
	private bool currentlyUnloading = false;
	private Scene currentSceneToUnload;

	/// <summary>
	/// Call this to load a scene using the loading manager and controller.
	/// </summary>
	/// <param name="sceneToLoad">The scene to load as a string.</param>
	public void LoadScene(string sceneToLoad)
	{
		if(!waitingForLoadingConnection && !loading && !SceneManager.GetSceneByName(loadingSceneName).isLoaded)
		{
			SceneManager.LoadScene(loadingSceneName, LoadSceneMode.Additive);
			waitingForLoadingConnection = true;
			loading = true;

			currentSceneToLoad = sceneToLoad;
			currentlyUnloading = false;
		}
	}

	/// <summary>
	/// Call this to load a scene using the loading manager and controller.
	/// </summary>
	/// <param name="sceneToLoad">The scene to load as a string.</param>
	/// <param name="sceneToUnload">The scene to unload as a scene.</param>
	public void LoadScene(string sceneToLoad, Scene sceneToUnload)
	{
		if(!waitingForLoadingConnection && !loading && !SceneManager.GetSceneByName(loadingSceneName).isLoaded)
		{
			SceneManager.LoadScene(loadingSceneName, LoadSceneMode.Additive);
			waitingForLoadingConnection = true;
			loading = true;

			currentSceneToLoad = sceneToLoad;
			currentlyUnloading = true;
			currentSceneToUnload = sceneToUnload;
		}
	}

	/// <summary>
	/// Call this connect the loading controller to the manager.
	/// </summary>
	/// <param name="loadingController">The current loading controller.</param>
	public void ConnectLoadingController(LoadingController loadingController)
	{
		if(waitingForLoadingConnection && loading)
		{
			currentLoadingController = loadingController;
			waitingForLoadingConnection = false;

			if(currentlyUnloading)
			{
				currentLoadingController.LoadScene(currentSceneToLoad, currentSceneToUnload);
			} else
			{
				currentLoadingController.LoadScene(currentSceneToLoad);
			}
		} else
		{
            loadingController.LoadScene(defaultSceneName);
		}
	}

	/// <summary>
	/// We are done loading.
	/// </summary>
	public void DoneLoading()
	{
		loading = false;
	}
}

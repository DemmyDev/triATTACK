using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class LoadingController : MonoBehaviour
{
	private AsyncOperation asyncOperation;
	private string targetScene;
	private Scene pastScene;
	private Scene loadingScene;
	private LoadingStateType state = LoadingStateType.Waiting;


	/// <summary>
	/// Grab the loading scene.
	/// </summary>
	private void Awake()
	{
		loadingScene = gameObject.scene;
        Application.targetFrameRate = 60;
	}

	/// <summary>
	/// Get the Loading Manager. If the Loading Manager has a current scene to load, it will pass the values over. If not, it will load the default scene.
	/// </summary>
	private void Start()
	{
		LoadingManager.Instance.ConnectLoadingController(this);
	}

	/// <summary>
	/// Go through the stages of loading.
	/// </summary>
	void Update ()
    {
        switch(state)
		{
			case LoadingStateType.Close:
				if(SplashScreen.isFinished)
				{
					state = LoadingStateType.Closing;
                    Closed();
				}
				break;

			case LoadingStateType.Unload:
				if(pastScene.isLoaded && SplashScreen.isFinished && asyncOperation == null)
				{
					asyncOperation = SceneManager.UnloadSceneAsync(pastScene);
					state = LoadingStateType.Unloading;
				}
				break;

			case LoadingStateType.Unloading:
				if (asyncOperation.isDone)
				{
					asyncOperation = null;
					state = LoadingStateType.Load;
				}
				break;

			case LoadingStateType.Load:
				if(SplashScreen.isFinished && asyncOperation == null)
				{
					asyncOperation = SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Additive);
					state = LoadingStateType.Loading;

					Debug.Log("Started load.");
				}
				break;

			case LoadingStateType.Loading:
				if (asyncOperation.isDone)
				{
                    SceneManager.SetActiveScene(SceneManager.GetSceneByName(targetScene));

                    state = LoadingStateType.Open;

					Debug.Log("Loading done.");
				}
				break;

			case LoadingStateType.Open:
				state = LoadingStateType.Opening;
                Opened();
				break;

			case LoadingStateType.Done:
				SceneManager.UnloadSceneAsync(loadingScene);
				LoadingManager.Instance.DoneLoading();
				state = LoadingStateType.Deleting;
				break;
		}
}

	/// <summary>
	/// Call this to load a scene through Loading Controller.
	/// </summary>
	/// <param name="sceneToLoad">The scene to load as a string.</param>
	public void LoadScene(string sceneToLoad)
	{
		if(state == LoadingStateType.Waiting)
		{
			targetScene = sceneToLoad;

            state = LoadingStateType.Load;
        }
	}

	/// <summary>
	/// Call this to load a scene through Loading Controller.
	/// </summary>
	/// <param name="sceneToLoad">The scene to load as a string.</param>
	/// <param name="sceneToUnload">The scene to unload as a scene.</param>
	public void LoadScene(string sceneToLoad, Scene sceneToUnload)
	{
		if(state == LoadingStateType.Waiting)
		{
			targetScene = sceneToLoad;
			pastScene = sceneToUnload;

            state = LoadingStateType.Close;
        }
	}

	/// <summary>
	/// This progresses the state machine after the background has covered the screen.
	/// </summary>
	private void Closed()
	{
		state = LoadingStateType.Unload;
	}

	/// <summary>
	/// This progresses the state machine after the background has uncovered the screen.
	/// </summary>
	private void Opened()
	{
		state = LoadingStateType.Done;
	}
}

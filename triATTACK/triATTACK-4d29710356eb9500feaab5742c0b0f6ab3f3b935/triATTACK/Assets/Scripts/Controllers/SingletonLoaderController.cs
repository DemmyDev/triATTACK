using UnityEngine;
using UnityEngine.SceneManagement;

public class SingletonLoaderController : MonoBehaviour
{
	[SerializeField]
	private string singletonScene;

	void Awake()
	{
		if(!SceneManager.GetSceneByName(singletonScene).isLoaded)
		{
			SceneManager.LoadScene(singletonScene, LoadSceneMode.Additive);
		}
	}
}
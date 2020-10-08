using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
	public void MoveToMenu()
	{
		SceneManager.LoadScene("Start", LoadSceneMode.Single);
	}

	public void MoveToPlay()
	{
		SceneManager.LoadScene("Play", LoadSceneMode.Single);
	}

	public void MoveToResults()
	{
		SceneManager.LoadScene("End", LoadSceneMode.Single);
	}
}

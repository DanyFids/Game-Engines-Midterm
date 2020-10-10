using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBehavior : MonoBehaviour
{
	private int score;
	private float time;
	public int SCORE_START = 1000;
	public int SCORE_DECAY = 5;
	public int SCORE_BOOST = 100;

	public Text display;

	// Start is called before the first frame update
	void Start()
	{
		score = SCORE_START;
		display.text = "Score: " + score.ToString();
	}

    // Update is called once per frame
    void Update()
    {
		time += Time.deltaTime;

		if (time >= 1.0f)
		{
			time -= 1.0f;
			score -= SCORE_DECAY;

			if (score < 0)
				score = 0;

			display.text = "Score: " + score.ToString();
		}
    }

	public void BoostScore()
	{
		score += SCORE_BOOST;
	}
}

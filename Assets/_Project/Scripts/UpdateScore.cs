using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Christophe.Fanchamps
{
public class UpdateScore : MonoBehaviour
{
        TMP_Text textMeshPro;
        int actualScore = 0;

        private void Awake()
        {
            textMeshPro = GetComponent<TMP_Text>();
            textMeshPro.text = actualScore.ToString();
        }

        private void OnEnable()
        {
            BallScore.onShot += UpdateScoreUI;
        }

        private void OnDisable()
        {
            BallScore.onShot -= UpdateScoreUI;
        }


        void UpdateScoreUI(int scoreToAdd)
    {
            actualScore += scoreToAdd;
            textMeshPro.text = (actualScore).ToString();
    }
}

}

using DG.Tweening;
using UnityEngine;

namespace NJ
{
    public class IrisDoor : MonoBehaviour
    {
        [SerializeField] Transform[] m_doors;
        public int m_maxBallInHole = 5;
        private int nbBall = 0;

        void Start()
        {
            Open();
            BallEnterHole.OnBallEnterHole += OnBallEnterHole;
        }

        private void OnBallEnterHole(bool _wasFlying = false)
        {
Debug.Log("Enter hole _wasFlying"+ _wasFlying +" - nbBall hole " + nbBall);
            nbBall++;
            if (nbBall >= m_maxBallInHole)
                Close();
        }

        // Update is called once per frame
        void Update()
        {

        }
        public void Open()
        {
            var sequence = DOTween.Sequence();
            sequence.AppendInterval(5f);
            foreach (var door in m_doors)
            {
                sequence.Join(door.DORotate(new Vector3(-90, 0, -90), 5f).SetEase(Ease.OutBounce));
            }

        }
        public void Close()
        {
            var sequence = DOTween.Sequence();
            sequence.AppendInterval(5f);
            foreach (var door in m_doors)
            {
                sequence.Join(door.DORotate(new Vector3(0, 0, 0), 5f).SetEase(Ease.OutBounce));
            }
            //Debug.Log("Close hole " + nbBall);
        }

    }
}
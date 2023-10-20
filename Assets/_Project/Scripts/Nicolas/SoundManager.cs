using UnityEngine;

namespace NJ {

    public class SoundManager : MonoBehaviour
    {
        public AudioClip m_ballEnterSound;
        public AudioClip m_ballExitSound;
        public AudioClip m_holeAppearSound;
        public AudioClip m_ballAppearSound;
        public AudioClip m_ballPitchSound;
        public AudioClip m_ballHardPitchSound;
        public AudioClip m_ballHitWallSound;

            private AudioSource audioSource;

        private void OnEnable()
        {
            BallEnterHole.OnBallEnterHole += PlayBallEnterSound;
            BallEnterHole.OnBallExitHole += PlayBallExitSound;
            //VerticalWall.OnBallHitWall += PlayBallHitWallSound;
            BouncingBallGame.OnHoleAppear += PlayHoleAppearSound;
            BouncingBallGame.OnBallAppear += PlayBallAppearSound;
            BouncingBallGame.OnBallPitch += PlayBallPitchSound;
        }

        private void OnDisable()
        {
            BallEnterHole.OnBallEnterHole -= PlayBallEnterSound;
            BallEnterHole.OnBallExitHole -= PlayBallExitSound;
            //VerticalWall.OnBallHitWall -= PlayBallHitWallSound;
            BouncingBallGame.OnHoleAppear -= PlayHoleAppearSound;
            BouncingBallGame.OnBallAppear -= PlayBallAppearSound;
            BouncingBallGame.OnBallPitch -= PlayBallPitchSound;
        }

            private void Start()
            {
                audioSource = GetComponent<AudioSource>();
            }
            private void PlayHoleAppearSound()
            {
                if (m_holeAppearSound != null)
                {
                    audioSource.PlayOneShot(m_holeAppearSound);
                }
            }
            private void PlayBallHitWallSound()
            {
                if (m_ballHitWallSound != null)
                {
                    audioSource.PlayOneShot(m_ballHitWallSound);
                }
            }
            private void PlayBallAppearSound()
            {
                if (m_ballAppearSound != null)
                {
                    audioSource.PlayOneShot(m_ballAppearSound);
                }
            }
            private void PlayBallPitchSound(bool hardPitch)
            {
                if (m_ballHardPitchSound != null && hardPitch)
                    audioSource.PlayOneShot(m_ballHardPitchSound);
                else if (m_ballPitchSound != null)
                    audioSource.PlayOneShot(m_ballPitchSound);
            
            }

            private void PlayBallEnterSound(bool _ballWasFlying)
        {
            if (m_ballEnterSound != null)
            {
                audioSource.PlayOneShot(m_ballEnterSound);
            }
        }

        private void PlayBallExitSound()
        {
            if (m_ballExitSound != null)
            {
                audioSource.PlayOneShot(m_ballExitSound);
            }
        }
    }
}
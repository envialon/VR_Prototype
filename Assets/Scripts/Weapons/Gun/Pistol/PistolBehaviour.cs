using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.OpenXR.Input;
using UnityEngine.VFX;

namespace VR_Prototype
{
    public class PistolBehaviour : MonoBehaviour,  GunBehaviour
    {
        public ParticleSystem[] particles;
        public VisualEffect[] effects;

        public AudioClip[] shotClips;

        public Transform shotOrigin;
        public Rigidbody rb;

        [Range(0.1f, 1)]
        public float vibrationAmplitude = 1;
        public float vibrationDuration = 1;

        [Space]
        public bool alreadyShot = false;
        
        
        private int layerMask = 1 << 3; // Enemigos es la layer 3
        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = gameObject.GetComponent<AudioSource>();
            rb = gameObject.GetComponent<Rigidbody>();
        }

        public void OnEnable()
        {
            alreadyShot = false;
        }

        public void PlayEffects()
        {
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].Play();
            }
            for (int i = 0; i < effects.Length; i++)
            {
                effects[i].Play();
            }
        }

        public void PlaySound()
        {
            int randomClip = Random.Range(0, shotClips.Length);
            AudioSource.PlayClipAtPoint(shotClips[randomClip], shotOrigin.position);
        }

        public void Shoot(ActivateEventArgs args)
        {
            // if (!alreadyShot)
            // {
                if (args.interactorObject is XRBaseControllerInteractor controllerInteractor)
                {
                    TriggerHaptic(controllerInteractor.xrController);
                }
                PlayEffects();
                PlaySound();
                if (Physics.Raycast(shotOrigin.position, shotOrigin.forward, out RaycastHit hit, Mathf.Infinity, layerMask))
                {
                    Debug.DrawRay(shotOrigin.position, shotOrigin.forward * hit.distance, Color.red);
                    EnemyPool.instance.EnemyHit(hit.transform.gameObject.GetComponent<Enemy>().id);
                }
                alreadyShot = true;
            // }

        }

        private void TriggerHaptic(XRBaseController controller)
        {
            controller.SendHapticImpulse(vibrationAmplitude, vibrationDuration);
        }
    }
}
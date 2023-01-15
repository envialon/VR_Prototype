using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using VR_Prototype;

public class PotionCannon : MonoBehaviour, GunBehaviour
{
    public Collider loadingChamber;

    public PotionNames? LoadedPotion = null;

    public ParticleSystem[] particles;
    public Transform shotOrigin;
    public Rigidbody rb;

    public float muzzleVelocity = 50;

    [Range(0.1f, 1)]
    public float vibrationAmplitude = 1;
    public float vibrationDuration = 1;

    private GameObject potionVisuals;
    private MeshRenderer potionRenderer;
    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        potionVisuals = transform.Find("PotionVisuals").gameObject;
        potionRenderer = transform.Find("Potion").GetComponent<MeshRenderer>();
    }

    public void OnEnable()
    {
    }

    public void PlayEffects()
    {
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].Play();
        }
    }

    public void Shoot(ActivateEventArgs args)
    {
        if (LoadedPotion != null)
        {
            if (args.interactorObject is XRBaseControllerInteractor controllerInteractor)
            {
                TriggerHaptic(controllerInteractor.xrController);
            }
            PlayEffects();

            ///SHOOT POTION LOGIC
            GameObject shotPotion = PotionManager.instance.InstantiatePotion(LoadedPotion, shotOrigin.position, shotOrigin.rotation);
            shotPotion.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, muzzleVelocity), ForceMode.Impulse);
            LoadedPotion = null;
        }
    }

    private void UnloadPotion()
    {

    }

    public void LoadPotion(PotionNames potionToLoad)
    {
        // ACTIVATE MESH OBJECT
        potionVisuals.SetActive(true);

        // CHANGE MESH OBJECT'S POTION MATERIAL TO WHATEVER IT SHOULD BE
        PotionComponents potionComponent = PotionManager.instance.potionComponents.potionComponents[(int)potionToLoad];
        potionRenderer.material = potionComponent.potionMaterial;

        // ASIGN LOAD POTION NAME
        LoadedPotion = potionToLoad;
    }

    private void TriggerHaptic(XRBaseController controller)
    {
        controller.SendHapticImpulse(vibrationAmplitude, vibrationDuration);
    }
}

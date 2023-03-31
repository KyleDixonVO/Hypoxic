using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror
{
    public class ReactorButton : Interactable
    {
        [SerializeField] string tooltip;
        [SerializeField] bool reactorOn;
        [SerializeField] AudioSource reactorSoundHolder;
        // Start is called before the first frame update
        void Start()
        {
            reactorOn = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (reactorOn)
            {
                AudioManager.audioManager.PlaySound(reactorSoundHolder, AudioManager.audioManager.reactorOn);
                reactorSoundHolder.loop = true;
            }
        }

        public override void OnFocus()
        {
            UI_Manager.ui_Manager.ActivatePrimaryInteractText(tooltip);
        }

        public override void OnLoseFocus()
        {
            UI_Manager.ui_Manager.DisablePrimaryInteractText();
        }

        public override void OnInteract()
        {
            Objective_Manager.objective_Manager.UpdateObjectiveCompletion((int)Objective_Manager.Objectives.activateReactor);
            if (reactorOn == false) AudioManager.audioManager.PlaySound(reactorSoundHolder, AudioManager.audioManager.reactorStartUp);
            if (reactorSoundHolder.isPlaying == false) reactorOn = true;
        }
    }
}

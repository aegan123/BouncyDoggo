using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
		public AudioSource superMode;
        public AudioSource backgroudMusic;
        public AudioSource gameOver;
        public AudioSource efxSound;
        public static SoundManager instance = null;     //Allows other scripts to call functions from SoundManager.

        void Awake ()
        {
            //Check if there is already an instance of SoundManager
            if (instance == null)
                //if not, set it to this.
                instance = this;
            //If instance already exists:
            else if (instance != this)
                //Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
                Destroy (gameObject);

            //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
            DontDestroyOnLoad (gameObject);
        }


        //Used to play single sound clips.
        public void PlaySingle(AudioClip clip)
        {
            //Set the clip of our efxSource audio source to the clip passed in as a parameter.
            efxSound.clip = clip;

            //Play the clip.
            efxSound.Play ();
        }

}

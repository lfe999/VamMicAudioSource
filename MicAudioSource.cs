using System;
using UnityEngine;

namespace LFE.MicrophoneAudioSource {
    public class Plugin : MVRScript {

        JSONStorableStringChooser StorableDevices;
        AudioSource AudioSource;

        public override void Init() {

            if(!containingAtom.type.Equals("Person")) {
                SuperController.LogError("Must add MicrophoneTest to a person");
            }

            AudioSource = containingAtom.GetComponentInChildren<AudioSource>();

            var devices = Microphone.devices.ToList();
            devices.Add(String.Empty);
            StorableDevices = new JSONStorableStringChooser("microphones", devices, String.Empty, "microphones", (string device) => {
                ListenToDevice(device);
            });
            CreatePopup(StorableDevices);
        }


        public void OnEnable() {
            ListenToDevice(StorableDevices.val);
        }

        public void OnDisable() {
            ListenToDevice(String.Empty);
        }

        private void ListenToDevice(string device) {
            if(AudioSource != null) {
                if(device.Equals(String.Empty)) {
                    AudioSource.Stop();
                    AudioSource.clip = null;
                }
                else {
                    AudioSource.clip = Microphone.Start(device, true, 10, 44100);
                    AudioSource.loop = true;
                    while(!(Microphone.GetPosition(null) > 0)){}
                    //SuperController.LogMessage($"mixer group: {AudioSource.outputAudioMixerGroup}");
                    AudioSource.Play();
                }
            }
        }
    }
}
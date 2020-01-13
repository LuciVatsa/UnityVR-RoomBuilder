using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRGait.Research;

namespace VRGait.Data
{
    public class FrameData
    {
        public float elapsedTime { get; private set; }

        public List<Vector3> trackPositions { get; private set; }

        public List<Vector3> trackRotations { get; private set; }

        public float elevator_height { get; private set; }

        public FrameData(float _elapsedTime, float _elevator_height, List<Vector3> _trackPositions, List<Vector3> _trackRotations)
        {
            this.elapsedTime = _elapsedTime;
            this.elevator_height = _elevator_height;
            this.trackPositions = _trackPositions;
            this.trackRotations = _trackRotations;
        }
    }

    public class csvExport : MonoBehaviour
    {
        public List<GameObject> trackGameObjects;

        public List<FrameData> frameDatas { get; set; }

        public Transform elevator;

        public ResearchUIController researchUIController;

        public Vector3[] cornerPositions { get; internal set; }

        private bool _active;

        private float _cachedStartTime;

        private Vector3 _hmdPosInOrigin;

        private List<Transform> _cachedTrackTransforms;

        private void Awake()
        {
            if (this.researchUIController == null)
            {
                throw new Exception("Please asign the research UI Controller.");
            }
            this.cornerPositions = new Vector3[4];
        }

        private void Start()
        {
            _active = false;
            this.frameDatas = new List<FrameData>();
            _cachedTrackTransforms = new List<Transform>();
            _cachedTrackTransforms.Clear();
            for (int i = 0; i < this.trackGameObjects.Count; i++)
            {
                _cachedTrackTransforms.Add(this.trackGameObjects[i].transform);
            }
        }

        public void TriggerRecording()
        {
            if (_active)
            {
                this.Deactivate();
                this.researchUIController.txt_recordingButton.text = "Start Recording";
                this.researchUIController.ToggleRecordingText();
            }
            else
            {
                if (!this.researchUIController.CheckInputValid())
                {
                    Debug.LogError("Please check that if all input are valid or not.");
                    return;
                }
                this.Activate();
                this.researchUIController.txt_recordingButton.text = "Stop Recording";
                this.researchUIController.ToggleRecordingText();
            }
        }

        public void Activate()
        {
            if (_active)
            {
                return;
            }
            _active = true;
            this.frameDatas.Clear();
            _cachedStartTime = Time.realtimeSinceStartup;
            Debug.Log("Start Recording at the time: " + string.Format("{0:HH:mm:ss tt}", DateTime.Now));
        }

        public void Deactivate()
        {
            if (!_active)
            {
                return;
            }
            _active = false;
            if (!CSVExporter.Write(
                this.cornerPositions,
                this.researchUIController.subjectNum,
                this.researchUIController.trialNum,
                this.researchUIController.elevation,
                this.researchUIController.trialType,
                this.frameDatas,
                this.trackGameObjects
                ))
            {
                throw new System.Exception("There is something wrong with the csv exporter.");
            }
            this.researchUIController.ResetUI();
            Debug.Log("Stop Recording at the time: " + string.Format("{0:HH:mm:ss tt}", DateTime.Now) + " and Exports .csv file successfully, please check it in " + Application.persistentDataPath + "/Exports/...");
        }

        private void FixedUpdate()
        {
            if (_active)
            {
                List<Vector3> trackPositions = new List<Vector3>();
                List<Vector3> trackRotations = new List<Vector3>();
                for (int i = 0; i < _cachedTrackTransforms.Count; i++)
                {
                    var trackTransform = _cachedTrackTransforms[i];
                    trackPositions.Add(trackTransform.position);
                    trackRotations.Add(trackTransform.rotation.eulerAngles);
                }
                this.frameDatas.Add(new FrameData(Time.realtimeSinceStartup - _cachedStartTime, this.elevator.transform.position.y, trackPositions, trackRotations));
            }
        }
        private void OnDestroy()
        {
            this.Deactivate();
        }

        public void Quit()
        {
            Application.Quit();
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
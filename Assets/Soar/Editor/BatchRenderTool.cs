#if HAS_UNITY_RECORDER && UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using UnityEditor;
using UnityEngine;

using UnityEditor.Recorder;
using UnityEditor.Recorder.Input;
using UnityEngine.SceneManagement;

namespace Assets.Editor
{
    [InitializeOnLoadAttribute]
    class BatchRenderTool : UnityEditor.EditorWindow
    {
        [Serializable]
        class Config : ScriptableObject
        {
            [HideInInspector]
            public string[]   paths;
            public string     outputDir;
            public GameObject target;

            [Header("Playback Settings")]
            public bool HideOnComplete;
            public bool Loop;
            [SoarSDK.EnabledIf("Loop")]
            public float Duration;
            [SoarSDK.EnabledIf("Loop", true)]
            public float endPadding = 1.0f;

            [Header("Video Settings")]
            public bool RecordTransparent = false;
            [SoarSDK.EnabledIf("RecordTransparent")]
            public Color ClearColor = Color.white;
            [HideInInspector]
            public MovieRecorderSettings VideoSettings;
        }

        class Stub : MonoBehaviour { }

        [SerializeField]
        private Config config_;

        [SerializeField]
        private bool isPathsOpen_;

        private static GUIContent pathsLabel_;
        private static GUIContent openFile_;
        private static GUIContent openFolder_;

        static BatchRenderTool()
        {
            EditorApplication.playModeStateChanged += onPlayModeChanged;

            pathsLabel_ = new GUIContent("Input Paths");

            openFile_ = new GUIContent("*", "Open a m3u8 master file");
            openFolder_ = new GUIContent("[]", "Import all captures in folder");
        }

        [MenuItem("Window/Soar/Batch Render")]
        static void ShowWindow()
        {
            BatchRenderTool window = (BatchRenderTool)EditorWindow.GetWindow(typeof(BatchRenderTool));
            window.titleContent = new GUIContent("Soar Batch Render Utility");
            window.Show();
        }

        private void InitConfig()
        {
            if (config_ == null || config_.VideoSettings == null)
            {
                config_ = CreateInstance<Config>();

                string blob = EditorPrefs.GetString("Soar.Editor.BatchSettings");
                JsonUtility.FromJsonOverwrite(blob, config_);

                config_.VideoSettings = ScriptableObject.CreateInstance<MovieRecorderSettings>();
                string blob2 = EditorPrefs.GetString("Soar.Editor.VideoSettings");
                JsonUtility.FromJsonOverwrite(blob2, config_.VideoSettings);
            }
        }

        private void SaveConfig()
        {
            var blob = JsonUtility.ToJson(config_, false);
            EditorPrefs.SetString("Soar.Editor.BatchSettings", blob);

            string blob2 = JsonUtility.ToJson(config_.VideoSettings);
            EditorPrefs.SetString("Soar.Editor.VideoSettings", blob2);
        }

        private void OnEnable()
        {
            InitConfig();
        }

        private void OnGUI()
        {
            InitConfig();

            SerializedObject so = new SerializedObject(config_);
            SerializedProperty paths = so.FindProperty("paths");

            isPathsOpen_ = EditorGUILayout.BeginFoldoutHeaderGroup(isPathsOpen_, pathsLabel_);
            if (isPathsOpen_)
            {
                EditorGUI.indentLevel++;
                EditorGUI.BeginChangeCheck();
                for (int ix = 0; ix < paths.arraySize; ix++)
                {
                    SerializedProperty element = paths.GetArrayElementAtIndex(ix);

                    Rect bounds = EditorGUILayout.GetControlRect(true);
                    Rect propBounds = bounds;
                    propBounds.width -= propBounds.height * 2;

                    GUI.SetNextControlName("batchrender.inputs_" + ix);
                    EditorGUI.PropertyField(propBounds, element);

                    {
                        Rect buttonBounds = bounds;
                        buttonBounds.xMin = bounds.xMax - bounds.height;
                        buttonBounds.width = bounds.height;

                        
                        if (GUI.Button(buttonBounds, openFolder_))
                        {
                            string path = EditorUtility.OpenFolderPanel("Select folder", Application.streamingAssetsPath, "");

                            if (!string.IsNullOrWhiteSpace(path))
                            {
                                if (path.StartsWith(Application.streamingAssetsPath))
                                {
                                    element.stringValue = path.Replace(Application.streamingAssetsPath + "/", "");
                                }
                                else
                                {
                                    EditorUtility.DisplayDialog("Warning", "Path must be under the StreamingAssets folder", "ok");
                                }
                            }
                        }
                    }
                    {
                        Rect buttonBounds = bounds;
                        buttonBounds.xMin = bounds.xMax - bounds.height * 2;
                        buttonBounds.width = bounds.height;

                        if (GUI.Button(buttonBounds, openFile_))
                        {
                            string path = EditorUtility.OpenFilePanel("Select file", Application.streamingAssetsPath, "m3u8");

                            if (!string.IsNullOrWhiteSpace(path))
                            {
                                if (path.StartsWith(Application.streamingAssetsPath))
                                {
                                    if (path.EndsWith("_master.m3u8", StringComparison.InvariantCultureIgnoreCase)) { 
                                        element.stringValue = path.Replace(Application.streamingAssetsPath + "/", "");
                                    }
                                    else  {
                                        EditorUtility.DisplayDialog("Warning", "Please select the master manifest file", "ok");
                                    }
                                }
                                else {
                                    EditorUtility.DisplayDialog("Warning", "Path must be under the StreamingAssets folder", "ok");
                                }
                            }
                        }
                    }
                }

                Rect bottomRegion = EditorGUILayout.GetControlRect(true);

                Rect addButton = bottomRegion;
                addButton.width = bottomRegion.height * 2;
                addButton.x = bottomRegion.xMax - bottomRegion.height * 4;

                Rect removeButton = bottomRegion;
                removeButton.width = bottomRegion.height * 2;
                removeButton.x = bottomRegion.xMax - bottomRegion.height * 2;

                if (GUI.Button(addButton, "+"))
                {
                    paths.arraySize++;
                }
                if (GUI.Button(removeButton, "-"))
                {
                    if (paths.arraySize > 0)
                    {
                        string focus = GUI.GetNameOfFocusedControl();

                        if (focus.StartsWith("batchrender.inputs_"))
                        {
                            int ix = int.Parse(focus.Replace("batchrender.inputs_", ""));

                            paths.DeleteArrayElementAtIndex(ix);
                        }
                        else
                        {
                            paths.arraySize--;
                        }
                    }
                }

                EditorGUILayout.Separator();

                EditorGUI.EndChangeCheck();

                if (GUI.changed)
                {
                    EditorUtility.SetDirty(config_);
                }
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            so.ApplyModifiedProperties();

            EditorGUI.BeginDisabledGroup(Application.isPlaying);
            UnityEditor.Editor editor = UnityEditor.Editor.CreateEditor(config_);
            editor.OnInspectorGUI();

            UnityEditor.Editor editor2 = UnityEditor.Editor.CreateEditor(config_.VideoSettings);
            editor2.OnInspectorGUI();

            if (GUILayout.Button("Start"))
            {
                LaunchProcess();
            }
            EditorGUI.EndDisabledGroup();
        }

        private void OnDisable()
        {
            SaveConfig();
        }

        private void LaunchProcess()
        {
            if (config_.target == null)
            {
                Debug.LogError("Cannot record without a target instance!");
                return;
            }
            SoarSDK.VolumetricRender volumeInstance = config_.target.GetComponentInChildren<SoarSDK.VolumetricRender>();
            if (volumeInstance == null)
            {
                Debug.LogError("Instance does not have a volumetric playback component!");
                return;
            }

            SaveConfig();

            EditorPrefs.SetBool("Soar.Editor.BatchRender.WantsBatch", true);

            AssetDatabase.SaveAssets();

            EditorApplication.isPlaying = true;
        }

        private IEnumerator BatchProcess()
        {
            InitConfig();

            GameObject target = config_.target;
            if (target == null)
            {
                Debug.LogError("Cannot record without a target instance!");
                yield break;
            }
            SoarSDK.VolumetricRender volumeInstance = target.GetComponentInChildren<SoarSDK.VolumetricRender>();
            if (volumeInstance == null)
            {
                Debug.LogError("Instance does not have a volumetric playback component!");
                yield break;
            }
            Vector3 pos    = target.transform.position;
            Quaternion rot = target.transform.rotation;
            Vector3 scale  = target.transform.localScale;

            Rigidbody body = target.GetComponent<Rigidbody>();

            volumeInstance.fileName = "";
            volumeInstance.autoPlay = false;
            volumeInstance.HideOnComplete = config_.HideOnComplete;
            volumeInstance.DisposeOnComplete = false;
            volumeInstance.autoLoop = config_.Loop;

            GameObject.DontDestroyOnLoad(target);

            List<string> toProcess = new List<string>();
            foreach (string path in config_.paths)
            {
                string absPath = Path.Combine(Application.streamingAssetsPath, path);
                if (File.Exists(absPath) || File.Exists(Path.ChangeExtension(absPath, ".m3u8")))
                {
                    if (path.EndsWith("_master.m3u8"))
                    {
                        toProcess.Add(path);
                    }
                    else if (path.EndsWith("_master"))
                    {
                        toProcess.Add(path + ".m3u8");
                    }
                    else
                    {
                        Debug.LogWarningFormat("\"{0}\" is not a valid file");
                    }
                }
                else if (Directory.Exists(absPath))
                {
                    string[] itPaths = Directory.GetFiles(absPath, "*_master.m3u8", SearchOption.AllDirectories);
                    toProcess.AddRange(itPaths.Select(x => x.Replace(Application.streamingAssetsPath + "/", "")));
                }
                else
                {
                    Debug.LogWarningFormat("\"{0}\" is not a valid path or file", path);
                }
            }

            var controllerSettings = ScriptableObject.CreateInstance<RecorderControllerSettings>();
            var controller = new RecorderController(controllerSettings);

            MovieRecorderSettings vidRecorder = config_.VideoSettings;

            if (config_.RecordTransparent && !vidRecorder.ImageInputSettings.SupportsTransparent)
            {
                Debug.LogWarning("Transparency requested but not supported");
            }


            controllerSettings.FrameRate = config_.VideoSettings.FrameRate;

            RecorderOptions.VerboseMode = false;

            GameObject.DontDestroyOnLoad(vidRecorder);
            GameObject.DontDestroyOnLoad(controllerSettings);

            Debug.Log("Starting batch processing");
            foreach (string path in toProcess)
            {
                Debug.LogFormat("Processing \"{0}\"", path);

                foreach (GameObject gameObj in GameObject.FindObjectsOfType<GameObject>()) {
                    if (gameObj.name == target.name && gameObj != target) {
                        gameObj.SetActive(false);
                    }
                }
                volumeInstance.Instance.Pause();
                volumeInstance.Instance.enabled = true;

                if (config_.RecordTransparent && Camera.main.clearFlags != CameraClearFlags.SolidColor)
                {
                    // We clear to a known color in case there's alpha blended stuff that wants some background color
                    Camera.main.backgroundColor = new Color(config_.ClearColor.r, config_.ClearColor.g, config_.ClearColor.b, 0);
                    Camera.main.clearFlags = CameraClearFlags.SolidColor;
                }

                target.transform.position = pos;
                target.transform.rotation = rot;
                target.transform.localScale = scale;

                if (body != null) { 
                    body.angularVelocity = Vector3.zero;
                    body.velocity = Vector3.zero;
                }

                volumeInstance.LoadNewClip(path);
                if (volumeInstance.Instance != null)
                {

                    controllerSettings.RemoveRecorder(vidRecorder);
                    string outPath = Path.Combine(config_.outputDir, Path.GetFileNameWithoutExtension(path));
                    int ix = 0;
                    while (File.Exists(outPath))
                    {
                        outPath = Path.Combine(config_.outputDir, Path.GetFileNameWithoutExtension(path) + ix);
                        ix++;
                    }
                    vidRecorder.OutputFile = outPath;
                    controllerSettings.AddRecorderSettings(vidRecorder);

                    controller.PrepareRecording();
                    controller.StartRecording();
                    volumeInstance.Instance.Play();

                    if (config_.Loop)
                    {
                        yield return new WaitForSeconds(config_.Duration);
                    }
                    else
                    {
                        while (
                            volumeInstance.Instance.enabled &&
                            volumeInstance.Instance.PlayState != SoarSDK.PlaybackInstancePlayState.Finished &&
                            volumeInstance.Instance.PlayState != SoarSDK.PlaybackInstancePlayState.Closed
                            )
                        {
                            yield return new WaitForSeconds(1.0f / config_.VideoSettings.FrameRate);
                        }
                        yield return new WaitForSeconds(config_.endPadding);
                    }

                    controller.StopRecording();
                }

                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
                yield return new WaitForSeconds(0.5f);      
            }

            // Give the controller some time to finish up
            yield return new WaitForSeconds(2.0f);
            Debug.Log("Processing complete");
            EditorApplication.isPlaying = false;
        }

        private static void onPlayModeChanged(PlayModeStateChange state)
        {
            if (EditorPrefs.GetBool("Soar.Editor.BatchRender.WantsBatch", false)) {
                BatchRenderTool window = (BatchRenderTool)EditorWindow.GetWindow(typeof(BatchRenderTool));

                if (state == PlayModeStateChange.EnteredPlayMode)
                {
                    EditorPrefs.SetBool("Soar.Editor.BatchRender.WantsBatch", false);
                    AssetDatabase.SaveAssets();

                    GameObject stub = new GameObject();
                    GameObject.DontDestroyOnLoad(stub);
                    Stub mono = stub.AddComponent<Stub>();
                    mono.StartCoroutine(window.BatchProcess());
                }
            }
        }
    }
}
#endif
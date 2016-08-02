using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.IO;
using System;
using System.Threading;
using UnityEngine.UI;

/**-----------------------------------------------------
* THREAD SAFE READ/WRITE BUFFER - 
*
*-----------------------------------------------------*/
public class ThreadSafeAccess
{
    //-----------------------------------------------------
    // CTOR 
    //-----------------------------------------------------
    public ThreadSafeAccess()
    {
        mLock = new ReaderWriterLock();
        mUpdate = new LibInterface.UpdateData();
        mUpdate.mImage = 0;
    }

    //-----------------------------------------------------
    // GET UPDATE 
    // return true if an update was acquired
    //-----------------------------------------------------
    public bool getUpdate(ref LibInterface.UpdateData update, int timeout = 0)
    {
        bool result = false;

        // try lock and read
        try
        {
            mLock.AcquireReaderLock(timeout);
            try
            {
                update = mUpdate;
            }
            finally
            {
                mLock.ReleaseReaderLock();
                result = true;
            }
        }
        catch(ApplicationException)
        {
            return false;            
        }
        return result;
    }

    //-----------------------------------------------------
    // SET UPDATE
    // return true if update was made and false otherwise
    //-----------------------------------------------------
    public bool setUpdate(ref LibInterface.UpdateData update, int timeout = 0)
    {
        bool result = false;

        // try lock and read
        try
        {
            mLock.AcquireWriterLock(timeout);
            try
            {
                mUpdate = update;
            }
            finally
            {
                mLock.ReleaseWriterLock();
                result = true;
            }
        }
        catch (ApplicationException)
        {
            return false;
        }
        return result;
    }

    private LibInterface.UpdateData mUpdate;
    private ReaderWriterLock mLock;
}

/**-----------------------------------------------------
* EYE-DETECTION LIBRARY INTERFACE CLASS - 
* You can init and operate library via this class
*
*
*-----------------------------------------------------*/
public class LibInterface : MonoBehaviour {

    //private GameObject playerDataGO;
    //private PlayerData playerData;
    private Text txtEyeDistance; 

    //--------------------------------------------------
    // STRUCT TO HOLD STARTUP STATUS
    //
    //--------------------------------------------------
    public enum startup_status
    {
        STARTUP_SUCCESS,
        ERR_OPENING_CONFIG_FILE,
        ERR_UNKNOWN_CLASSIFIER_FILE,
        ERR_SENSOR_MGR_NULL,
        ERR_ACCELEROMETER_SENSOR_NULL,
        ERR_SENSOR_EVENT_QUEUE_NULL,
        ERR_EVENT_LOOPER_NULL,
        ERR_ENABLE_ACCELEROMETER,
        ERR_CREATING_VIDEOCAPTURE,
        ERR_SYMBOL_INFO_UNKNOWN,
        ERR_SEARCH_MODE_SETTING_UNKNOWN,
        ERR_LOADING_LEFT_CLASSIFIER_FILE,
        ERR_LOADING_RIGHT_CLASSIFIER_FILE
    };

    //-----------------------------------------------------
    // EYE DETECTION UPDATE STATUS - 
    // Possible events that can occur during update
    //-----------------------------------------------------
    public enum update_status
    {
        U_STATUS_OK,
        U_EXCESS_ACCELERATION,
        U_CAM_CAPTURE_FAIL,
        U_CANNOT_LOCATE_SYMBOL,
        U_LOCATED_SYMBOL,
        U_CANNOT_LOCATE_EYES,
        U_LOCATED_EYES,
        U_LOCATED_BOTH,
        U_COMPUTED_DIST_SYMBOL_LOW_QUAL,
        U_COMPUTED_DIST_SYMBOL,
        U_COMPUTED_DIST_EYES_LOW_QUAL,
        U_COMPUTED_DIST_EYES,
        U_COMPUTED_DIST_SYMBOL_AND_EYES
    };

    //-----------------------------------------------------
    // EYE DETECTION CONFIGURATION DATA - 
    // Pass to the eye-detection library START function
    //-----------------------------------------------------
    [StructLayout(LayoutKind.Sequential)]
    public struct EyeDetectConfig 
    {
        [MarshalAs(UnmanagedType.LPStr)] public string leftEye;
        [MarshalAs(UnmanagedType.LPStr)] public string rightEye;
        [MarshalAs(UnmanagedType.LPStr)] public string symbolType;
        [MarshalAs(UnmanagedType.LPStr)] public string searchMode;
        public int camID;
        public int minThreshold;
        public float accelThreshold;
        public int accelSensitivity;
        public int filterBufferSize;
        public double pupillaryDistance;
        public double defaultPupillaryDistance;
        public int marginLeft;
        public int marginRight;
        public int marginTop;
        public int marginBottom;
        public int marginWeight;
    };

    //-----------------------------------------------------
    // EYE DETECTION UPDATE FRAME DATA - 
    // Pass reference to the Update function to be populated
    //-----------------------------------------------------
    [StructLayout(LayoutKind.Sequential)]
    public struct UpdateData
    {
        public uint mFrameId;
        public double mAcceleration;
        public int mStatus;
        public bool mIPDCaculatedFromSymbol;
        public double mEyeResultXPos;
        public double mEyeResultYPos;
        public uint mEyeResultWidth;
        public uint mEyeResultQuality;
        public double mEyeResultRawDistance;
        public double mEyeResultFiltDistance;
        public double mSymbResultXPos;
        public double mSymbResultYPos;
        public uint mSymbResultWidth;
        public uint mSymbResultQuality;
        public double mSymbResultRawDistance;
        public double mSymbResultFiltDistance;
        public uint mImage;
    };

#if UNITY_EDITOR

    // --------------------------------------------------------------
    // MOCK THE LIBRARY API FOR TESTING IN UNITY
    // --------------------------------------------------------------
    private int EyeDetectionModuleOnStart(ref EyeDetectConfig config)
    {
        Debug.Log("Editor: On Start called");
        return (int)startup_status.STARTUP_SUCCESS;
    }

    private void EyeDetectionModuleOnPause()
    {
        Debug.Log("Editor: On Pause called");
    }

    private void EyeDetectionModuleOnResume()
    {
        Debug.Log("Editor: On Resume called");
    }

    private void EyeDetectionModuleOnUpdate(ref UpdateData update)
    {
       // Debug.Log("Editor: On Update called");

        // simulate with some dummy values in the update 
        update.mFrameId = 1;
        update.mStatus = (int)update_status.U_COMPUTED_DIST_SYMBOL;
        update.mSymbResultRawDistance = 25;
        update.mSymbResultFiltDistance = 25;
        update.mSymbResultXPos = 320;
        update.mSymbResultYPos = 240;
        update.mSymbResultQuality = 19;
    }

    private void EyeDetectionModuleOnDestroy()
    {
        Debug.Log("Editor: On Destroy called");
    }

#else

    //-----------------------------------------------------
    // IMPORT LIBRARY FUNCTIONS - 
    //-----------------------------------------------------
    // START THE EYE-DETECTION MODULE - 
    // allocates memory and must be called first before other functions
    // pass configuration data via the EyeDetectConfig class
    //-----------------------------------------------------
    [DllImport("eyedetectionmod", CallingConvention = CallingConvention.Cdecl)]
    private static extern int EyeDetectionModuleOnStart(ref EyeDetectConfig config);

    //-----------------------------------------------------
    // PAUSE THE EYE-DETECTION MODULE - call to temporarily cease eye-detection
    //-----------------------------------------------------
    [DllImport("eyedetectionmod", CallingConvention = CallingConvention.Cdecl)]
    private static extern void EyeDetectionModuleOnPause();

    //-----------------------------------------------------
    // RESUME THE EYE-DETECTION MODULE - call to resume eye-detection after a PAUSE
    //-----------------------------------------------------
    [DllImport("eyedetectionmod", CallingConvention = CallingConvention.Cdecl)]
    private static extern void EyeDetectionModuleOnResume();

    //-----------------------------------------------------
    // RETRIEVE EYE-DETECTION DATA - eye detection update data will be populated with the latest
    // data
    //-----------------------------------------------------    
    [DllImport("eyedetectionmod", CallingConvention = CallingConvention.Cdecl)]
    private static extern void EyeDetectionModuleOnUpdate(ref UpdateData update);

    //-----------------------------------------------------
    // CLOSE THE EYE-DETECTION MODULE - releases memory. Start must be called before any further
    // function calls
    //-----------------------------------------------------
    [DllImport("eyedetectionmod", CallingConvention = CallingConvention.Cdecl)]
    private static extern void EyeDetectionModuleOnDestroy();

#endif

    //-----------------------------------------------------
    // CLASS FIELDS - 
    //-----------------------------------------------------
    private ThreadSafeAccess mSafeAccess;
    private Thread mWorker;
    private EyeDetectConfig mConfig;
    private int mStartUpCode;
    private bool mRunning;
    private bool mDoUpdate;
    private bool mPauseRequest;
    private bool mResumeRequest;
    private ArrayList mlisteners;

    public delegate void Listener(UpdateData update);

    //-----------------------------------------------------
    // CTOR
    //-----------------------------------------------------
    public LibInterface()
    {
        mSafeAccess = default(ThreadSafeAccess);
        mWorker = default(Thread);
        mStartUpCode = default(int);
        mRunning = false;
        mDoUpdate = false;
        mPauseRequest = false;
        mResumeRequest = false;
        mlisteners = new ArrayList();
    }

    //-----------------------------------------------------
    // ADD AN EVENT LISTENER
    //-----------------------------------------------------
    public void addListener(Listener listener)
    {
        mlisteners.Add(listener);
    }

    //-----------------------------------------------------
    // THREAD UPDATE FUNCTION - 
    //-----------------------------------------------------
    public void ThreadFtn()
    {
        while (mRunning)
        {
            if (mPauseRequest)
            {
                EyeDetectionModuleOnPause();
                mDoUpdate = false;
                mPauseRequest = false;
            }

            if (mResumeRequest)
            {
                EyeDetectionModuleOnResume();
                mDoUpdate = true;
                mResumeRequest = false;
            }

            if (mDoUpdate)
            {
                UpdateData update = new UpdateData();
                update.mImage = 0;
                EyeDetectionModuleOnUpdate(ref update);
                mSafeAccess.setUpdate(ref update);
            }
        }
    }

    //-----------------------------------------------------
    // START - INVOKED BY UNITY AT START-UP
    //-----------------------------------------------------
	void Start () 
    {
        //playerDataGO = GameObject.Find("PlayerData");
        //playerData = playerDataGO.GetComponent<PlayerData>();
        txtEyeDistance = GameObject.Find("eyeDistance").GetComponent<Text>();

        // symbol type either - ConcentricCircles or SquareCircleTriangle or None
        string defaultRightEyeFile = "haarcascade_lefteye_2splits.xml";
        string defaultLeftEyeFile = "haarcascade_righteye_2splits.xml";

        // either 'SquareCircleTriangle' or 'ConcentricCircles'
        string defaultSymbolType = "SquareCircleTriangle";

        // either 'symbol' or 'eyes' or 'symbol+eyes+parallel' or 'symbol+eyes+switch'
        string defaultSearchMode = "symbol";

        // back cam = 1000, front cam = 1001
        int defaultCamID = 1001;

        // a value from 0 to ?. This value is compared with the quality of the dot-market capture
        // if the quality of the capture is below 'minThreshold' then status is set to U_COMPUTED_DIST_LOW_QUAL 
        // otherwise status is U_COMPUTED_DIST. 
        // Setting 'minThreshold' to 20 worked best during testing
        // ignore U_COMPUTED_DIST_LOW_QUAL values - likely erroneous.
        int defaultMinThreshold = 10;

        // Threshold and Sensitivity for the Accelerometer
        // Accelerometer data is stored and compared in each frame
        // The abs-difference is compared with 'accelThreshold' - while exceeded the module increases
        // count of the number of frames the accelerometer data was above threshold (accelSensitivity)
        // if the number of frames above 'accelThreshold' exceeds 'accelSensitivity' then status returned will be 'U_EXCESS_ACCELERATION'
        int defaultAccelThreshold = 20;
        int defaultAccelSensitivity = 6;

        // The size of the median filter buffer - 10 to 14 seems to be reasonably good for getting a smooth filter value
        int defaultFilterBufferSize = 10;

        // The default distance between pupils in cm 
        double pupillaryDistance = 0.0;
        double defaultPupillaryDistance = 6.5;

        // Margins in % values from side of screen. Assign a higher quality to detections within margin
        int defaultMarginLeft = 20;
        int defaultMarginRight = 20;
        int defaultMarginTop = 10;
        int defaultMarginBottom = 10;

        // Margin weight to increase the quality 
        int defaultMarginWeight = 10;

        // Initialise Thread Objects
        mSafeAccess = new ThreadSafeAccess();
        mWorker = new Thread(new ThreadStart(ThreadFtn));
        EyeDetectConfig config = new EyeDetectConfig();

        config.leftEye = defaultLeftEyeFile;
        config.rightEye = defaultRightEyeFile;
        config.symbolType = defaultSymbolType;
        config.searchMode = defaultSearchMode;
        config.camID = defaultCamID; 
        config.minThreshold = defaultMinThreshold;
        config.accelThreshold = defaultAccelThreshold;
        config.accelSensitivity = defaultAccelSensitivity;
        config.filterBufferSize = defaultFilterBufferSize;
        config.pupillaryDistance = pupillaryDistance;
        config.defaultPupillaryDistance = defaultPupillaryDistance; 
        config.marginLeft = defaultMarginLeft;
        config.marginRight = defaultMarginRight;
        config.marginTop = defaultMarginTop;
        config.marginBottom = defaultMarginBottom;
        config.marginWeight = defaultMarginWeight;

        // Start the LibInterface
        startLibInterface(config);
		/*if (playerData.CameraTracking == true) 
		{
			resumeLibInterface ();
		} else {
			pauseLibInterface();
		}*/
	}

    //-----------------------------------------------------
    // CALLED WHEN THE GAME IS PAUSED AND RESUMED
    //-----------------------------------------------------
    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus) // game paused
        {
            pauseLibInterface();
        }
        else
        {
            resumeLibInterface();
        }
    }

    //-----------------------------------------------------
    // ON GUI - 
    //-----------------------------------------------------
    void OnGUI()
    {
        UpdateData update = new UpdateData();

        // try to get the next update from the library
        if (mSafeAccess.getUpdate(ref update) == false)
        {
            return;
        }

        update_status status = (update_status)update.mStatus;
        double rawDistance = 0.0;
        double filtDistance = 0.0;
        double xPos = 0;
        double yPos = 0;
        double quality = 0;

        if (status == update_status.U_COMPUTED_DIST_EYES || status == update_status.U_COMPUTED_DIST_EYES_LOW_QUAL)
        {
            rawDistance = update.mEyeResultRawDistance;
            filtDistance = update.mEyeResultFiltDistance;
            xPos = update.mEyeResultXPos;
            yPos = update.mEyeResultYPos;
            quality = update.mEyeResultQuality;
        }
        else if (status == update_status.U_COMPUTED_DIST_SYMBOL || status == update_status.U_COMPUTED_DIST_SYMBOL_LOW_QUAL)
        {
            rawDistance = update.mSymbResultRawDistance;
            filtDistance = update.mSymbResultFiltDistance;
            xPos = update.mSymbResultXPos;
            yPos = update.mSymbResultYPos;
            quality = update.mSymbResultQuality;
        }

        // output the update results to screen
        txtEyeDistance.text = filtDistance.ToString();
        /*GUI.Label(new Rect(5, 0, Screen.width, Screen.height), "Startup code " + (startup_status)mStartUpCode);
        GUI.Label(new Rect(5, 20, Screen.width, Screen.height), "Frame id: " + update.mFrameId);
        GUI.Label(new Rect(5, 40, Screen.width, Screen.height), "Update Status: " + status);
        GUI.Label(new Rect(5, 60, Screen.width, Screen.height), "Eye Raw distance: " + rawDistance);
        GUI.Label(new Rect(5, 80, Screen.width, Screen.height), "Eye Filt. distance: " + filtDistance);
        GUI.Label(new Rect(5, 100, Screen.width, Screen.height), "X Pos: " + xPos + " Y Pos: " + yPos + " quality: " + quality);
        GUI.Label(new Rect(5, 120, Screen.width, Screen.height), "Threshold: " + mConfig.minThreshold);
        GUI.Label(new Rect(5, 140, Screen.width, Screen.height), "Accel Threshold: " + mConfig.accelThreshold);
        GUI.Label(new Rect(5, 160, Screen.width, Screen.height), "Accel Sensitivity: " + mConfig.accelSensitivity);
        GUI.Label(new Rect(5, 180, Screen.width, Screen.height), "Margin L/R/T/B W: " + mConfig.marginLeft + " " + mConfig.marginRight + " " + mConfig.marginTop + " " + mConfig.marginBottom + " " + mConfig.marginWeight);        
        GUI.Label(new Rect(5, 200, Screen.width, Screen.height), "Detection Symbol: " + mConfig.symbolType);
        GUI.Label(new Rect(5, 220, Screen.width, Screen.height), "Search Mode: " + mConfig.searchMode);*/

        if (mDoUpdate)
        {
            // notify the listeners that an update has occurred
            foreach (Listener l in mlisteners)
            {
                l(update);
            }

            /*if (GUI.Button(new Rect(20, 240, 400, 100), "Stop Updating"))
            {
                pauseLibInterface();
            }*/
        }
        else
        {
            /*if (GUI.Button(new Rect(20, 240, 400, 100), "Start Updating"))
            {
                resumeLibInterface();
            }*/
        }
    }

    //-----------------------------------------------------
    // ON DESTROY - called at end
    //-----------------------------------------------------
    void OnDestroy()
    {
        terminateLibInterface();
    }

    //-----------------------------------------------------
    // UPDATE - called once per frame
    //-----------------------------------------------------
	void Update () 
	{
        //if (playerData.CameraTracking == true) {
        //    float lastDist = playerData.distance;   //stores last good result
        //    UpdateData update = new UpdateData ();
			
        //    // retrieve the update from library (blocking call)
        //    if (mSafeAccess.getUpdate (ref update) == false) {
        //        return;
        //    }
			
        //    update_status status = (update_status)update.mStatus;
        //    playerData.trackingStatus = Enum.GetName (typeof(update_status), status);
        //    // set excessAccleration flag
        //    playerData.excessAcceleration = (status == update_status.U_EXCESS_ACCELERATION ? true : false);
			
        //    // if distance was computed then update the playerData distance field
        //    if (status == update_status.U_COMPUTED_DIST_SYMBOL) {
        //        playerData.excessAcceleration = false;
        //        playerData.distance = (float)update.mSymbResultFiltDistance;
        //    }else{
        //        playerData.excessAcceleration = false;
        //        playerData.distance = 25.0f;
        //    }
        //} else {
        //    playerData.distance = 25.0f;
        //    playerData.excessAcceleration = false;
        //}
	}

    //-----------------------------------------------------
    // START THE LIB-INTERFACE 
    //-----------------------------------------------------
    public void startLibInterface(EyeDetectConfig config)
    {
        // already started
        if (mRunning == true)
        {
            return;
        }

        // Set initial state
        mDoUpdate = false;
        mPauseRequest = false;
        mResumeRequest = false;
        mStartUpCode = 0;

        // assign the config
        mConfig = config;

        // Succinct but ugly. Returning path from apk file seems to require use of yield but 
        // Coroutines need to be chained because the EyeDetectionModule can't start until 
        // the file data has been extracted and written to config - must be a better way tho
        // Load the left eye file from apk and write to persistent storage, extract leftpath...
        StartCoroutine(LoadFile(mConfig.leftEye, (leftpath) =>
        {
            // write path to config for EyeDetection library to read
            mConfig.rightEye = leftpath;

            // Start same process for right eye
            StartCoroutine(LoadFile(mConfig.rightEye, (rightpath) =>
            {
                mConfig.leftEye = rightpath;
                mStartUpCode = EyeDetectionModuleOnStart(ref mConfig);
                mRunning = true;
                mWorker.Start();
            }));
        }));
    }

    //-----------------------------------------------------
    // PAUSE THE LIB INTERFACE
    //-----------------------------------------------------
    public void pauseLibInterface()
    {
        mDoUpdate = false;
        mPauseRequest = true;            
    }

    //-----------------------------------------------------
    // RESUME THE LIB INTERFACE
    //-----------------------------------------------------    
    public void resumeLibInterface()
    {
        mDoUpdate = true;
        mResumeRequest = true;            
    }

    //-----------------------------------------------------
    // TERMINATE THE LIB INTERFACE
    //-----------------------------------------------------
    public void terminateLibInterface()
    {
        if (mRunning == true)
        {
            mRunning = false;
            mWorker.Join();
            EyeDetectionModuleOnDestroy();
        }
    }

    //-----------------------------------------------------
    // LOAD FILE - 
    //-----------------------------------------------------
    private IEnumerator LoadFile(string filename, Action<string> callback)
    {
        string contents = "";

        // Read in the contents of file
        var sourcePath = System.IO.Path.Combine(Application.streamingAssetsPath, filename);
        if (sourcePath.Contains("://")) // On Android we get jar-file:// prefix
        {
            var www = new WWW(sourcePath);
            yield return www;
            contents = www.text;
        }
        else // Not Android
        {
            contents = File.ReadAllText(sourcePath);
        }

        // Save the contents of the file in the persistent-data-path
        string destPath = Path.Combine(Application.persistentDataPath, filename);

        Debug.Log("saved from filepath: " + sourcePath);
        Debug.Log("saved to filepath: " + destPath);

        StreamWriter sw = new StreamWriter(destPath);
        sw.Write(contents);
        sw.Flush();
        sw.Close();

        callback(destPath);
    }
}

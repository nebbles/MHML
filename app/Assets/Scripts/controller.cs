using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class controller : MonoBehaviour
{
    // ----------------------------------------------------------------- 
    // Final UUIDs
    // ----------------------------------------------------------------- 

    // Decoding syntax:
    // For uint8: string finalVal8 = int.Parse(BitConverter.ToString(s), System.Globalization.NumberStyles.HexNumber).ToString();
    // For uint16: string finalVal16 = BitConverter.ToInt16(s, 0).ToString();
    // For float32: string finalVal32 = BitConverter.ToSingle(s, 0).ToString();

    // PPG Service 
    private string _ppgServiceUUID = "1a632076-8702-41b9-bcff-ea119ae68a69";

    // PPG Characteristics
    private string _HRmeasurementUUID = "00002A37-0000-1000-8000-00805f9b34fb"; // uint8 check
    private string _InterbeatIntervalUUID = "847dc27a-00f2-4c99-aebf-5eacea5474b4"; // uint16 check
    private string _Spo2MeasurementUUID = "ef4684bb-c958-40df-90be-5eaa65e07948"; // float32 check
    private string _bodySensorLocationUUID = "00002a38-0000-1000-8000-00805f9b34fb"; //uint8 check


    // GSR Service
    private string _gsrServiceUUID = "720f8954-ace5-41f7-acec-113b274bc54f";

    // GSR Characteristics
    private string _skinConductanceLevelUUID = "3f18d911-bffd-4236-b5fc-94c9bf27d0e8"; // uint16 check
    private string _bodySensorLocationGsrUUID = "00002a38-0000-1000-8000-00805f9b34fb"; // uint8 check


    // Device Information Service
    private string _DeviceInfoUUID = "0000180a-0000-1000-8000-00805f9b34fb";

    // Device Information Characteristics
    private string _FirmwareRevisionUUID = "00002a26-0000-1000-8000-00805f9b34fb"; //UTF-8 String check


    // Global Public Variables to display info
    public Transform PanelScrollContents;            // device list panel 
    public Text txtDebug;                        // debugging textbox - Redundant for integrated version. Remove for final version.
    public GameObject connectButton;               // the button to click to connect to a device 
    // public Text txtData;                        // Redunant: the text box to type in send data
    public Text txtReceive;                        // the text boxes data is being received into 1 - 7
    public Text txtReceive2;
    public Text txtReceive3;
    public Text txtReceive4;
    public Text txtReceive5;
    public Text txtReceive6;
    public Text txtReceive7;

    // Initialisation variables and booleans
    public bool isConnected = false;
    public bool _readFound = false;
    public bool _readFound2 = false;
    public bool _readFound3 = false; // Currently a redundant variable
    private bool _scanning = false;
    private bool _connecting = false;
    private bool _stuckConnecting = false;

    // characteristic subscription checks
    private bool _HR_Notification = false;
    private bool _IBI_Notification = false;
    private bool _Spo2_Notification = false;
    private bool _SkinConductance_Notification = false;
    public bool _allSubscribingComplete = false;

    // characteristic read checks
    private bool _ppgBodyCheck = false;
    private bool _gsrBodyCheck = false;
    private bool _DeviceInfoCheck = false;
    public bool _allReadingComplete = false;

    // Data storage
    private Dictionary<string, string> _peripheralList;

    private Queue<int> _HR_data = new Queue<int>();
    private Queue<int> _IBI_data = new Queue<int>();
    private Queue<float> _Spo2_data = new Queue<float>();
    private Queue<int> _ppgLocation_data = new Queue<int>();
    private Queue<int> _skinConductance_data = new Queue<int>();
    private Queue<int> _gsrLocation_data = new Queue<int>();
    private Queue<string> _deviceInfo_data = new Queue<string>();

	public bool _storeSubscribeData = false;
    public string storedAddress = null; // This is the device address that will get stored permanently, and only updated if the device is 'forgotten'. 
    public string storedName = null; // This is the device name that will get stored permanently, and only updated if the device is 'forgotten'.
    private string _connectedName;
    private string _connectedAddress;
    // private string _connectedID = null; // Currently a redundant variable
    // private string connectedName = null;  // Currently a redundant variable 

    // The three MHML M5 Device Addresses are as follows:
    public string scottSensorAddr = "B4:E6:2D:8B:92:F7"; // Scotts Hardware Wired Sensor Device
	public string felixSensorAddr = "84:0D:8E:25:91:C2"; // Felix's Testing Device
	public string benSensorAddr =  "84:0D:8E:25:96:BA"; // Ben's Testing Device
    private string _permittedDeviceName = "MHML M5";

    // Other variables
    private int devicesFound = 0;
    private int count = 0;
    private int readingCount = 0;
    private int localReadCount = 0;
    private int _reScanTimer = 0;
    private int _autoConnectTimer = 0;

    private bool _hasStoredBluetoothValues = false;
     
    private GameObject panelScan;
    private GameObject panelConnected;

    // Disconnect from all bluetooth devices.
    public void disconnectBluetooth(bool _calledThroughForgetDevice)
    {
        //txtDebug.text = "Disconnection in Progress";
        BluetoothLEHardwareInterface.DisconnectPeripheral(_connectedAddress, connectedDeviceAddress=>
        {
            txtDebug.text = "Disconnect Successful";

            // Reset subscribe and read checks
            isConnected = false;
            _readFound = false;
            _readFound2 = false;
            _readFound3 = false;
            _HR_Notification = false;
            _IBI_Notification = false;
            _Spo2_Notification = false;
            _SkinConductance_Notification = false;
            _ppgBodyCheck = false;
            _gsrBodyCheck = false;
            _DeviceInfoCheck = false;
            _connectedAddress = null;
            _connectedName = null;
            if (_calledThroughForgetDevice == true)
            {
                storedName = null;
                storedAddress = null;
            }

            // show scanning panel

            showScan();
            _scanning = false;
            scan();
        });
    }

    public void forgetDevice()
    {
        PlayerPrefs.DeleteKey("Saved Device Name");
        PlayerPrefs.DeleteKey("Saved Device Address");
        _hasStoredBluetoothValues = false;
        txtDebug.text = "Deleting Device Address";
        disconnectBluetooth(true); // This must go before clearing the Name and address, otherwise the BLE interface doesn't know what to disconnect from. 
        
    }

    // Connect to the BLE peripheral
    void connectBluetooth(string nameID, string addr)
    {
        // System.Threading.Thread.Sleep(500);
        BluetoothLEHardwareInterface.ConnectToPeripheral(addr, (address) => 
        {
            _connecting = false;
            txtDebug.text = "Connection Successful";
            isConnected = true;
            _connectedName = nameID;
            _connectedAddress = address;

            showConnected();

            _readFound = true;
            _readFound2 = true;
            _readFound3 = true;
			_storeSubscribeData = true;

            if (_hasStoredBluetoothValues == false)
            {
                PlayerPrefs.SetString("Saved Device Name", nameID);
                storedName = nameID;
                PlayerPrefs.SetString("Saved Device Address", address);
                storedAddress = address;
                _hasStoredBluetoothValues = true;
                txtDebug.text = "Device Address Stored";
            }
            //txtDebug.text = "Beginning Data Receiving";
            
        }, (address, serviceUUID) => 
        {
            //txtDebug.text = "service found";
        },

        // Callback initiated upon service characteristic discovery. Will be called 7 times in this case. 
        (address, serviceUUID, characteristicUUID) => {}, 
        (address) => 
        {

            // This will get called when the device disconnects be aware that this will also get called when the disconnect 
            // is called above.
            isConnected = false;
            _connectedAddress = null;
            _connectedName = null;
            //txtDebug.text = "Beginning Disconnect Callback";
            disconnectBluetooth(false); // Not called through device forget, so don't delete the name & address
        });
        _connecting = false;
    }

    // Connect to the device address, but not yet initiating the bluetooth connection.
    public void connectTo(string sName, string sAddress)
    {
        // stop scanning 
        // Ensure we are only connecting to one of the MHML devices. 
        // This will attempt to connect to a device if clicked. However, the scan will refresh after a certain timeout (max 12.5 seconds) if the connection attempt is unsuccessful. 
        if (sAddress==benSensorAddr || sAddress==felixSensorAddr || sAddress==scottSensorAddr)
        {
            txtDebug.text += "Beginning connecting";
            _connecting = true;
            BluetoothLEHardwareInterface.StopScan();
            connectBluetooth(sName, sAddress);
            txtDebug.text += "Tried Bluetooth Connect";
        }
        
    }

    // Scan for BLE enabled devices
    public void scan()
    {
        if (_scanning == true)
        {
            txtDebug.text = "Stop scan";
            BluetoothLEHardwareInterface.StopScan();
            _scanning = false;
            txtDebug.text = "scanning = false";
        }
        else
        if (_scanning == false)
        {
            txtDebug.text = "Scanning has begun 1";
            RemovePeripherals();
            txtDebug.text = "Peripherals removed 2";

            devicesFound = 0;

            // the first callback will only get called the first time this device is seen 
            // this is because it gets added to a list in the BluetoothDeviceScript 
            // after that only the second callback will get called and only if there is 
            // advertising data available 
            BluetoothLEHardwareInterface.ScanForPeripheralsWithServices(null, (address, name) => {
                txtDebug.text += "Scan";
                AddPeripheral(name, address);
            }, (address, name, rssi, advertisingInfo) => { });
            txtDebug.text = "Scanning = True";
            // _scanning = true;
        }
    }

    // Read a single integer characteristic once
    void readCharacteristicFromServiceInt(string serviceID, string characteristicID, Action<int> dataprocessing, Func<byte[], int> Decoder)
    {
        BluetoothLEHardwareInterface.ReadCharacteristic(_connectedAddress, serviceID, characteristicID, (characteristic, data) =>
        {
            if (data.Length == 0)
            {
                //txtDebug.text = "No Text to Read";
            }
            else
            {
                //txtDebug.text = "Read Value Found";
                int decoded_data = Decoder(data);
                dataprocessing(decoded_data);
            }
        });
    }

    // Read a single string characteristic once
    void readCharacteristicFromServiceString(string serviceID, string characteristicID, Action<string> dataprocessing, Func<byte[], string> Decoder)
    {
        BluetoothLEHardwareInterface.ReadCharacteristic(_connectedAddress, serviceID, characteristicID, (characteristic, data) =>
        {
            if (data.Length == 0)
            {
                //txtDebug.text = "No Text to Read";
            }
            else
            {
                //txtDebug.text = "Read Value Found";
                string decoded_data = Decoder(data);
                dataprocessing(decoded_data);
            }
        });
    }

    // Reads all the non-notify characteristics upon switching of a boolean value. This function must go in the update function. 
    void readNonNotifyCharacteristics ()
    {
        _allReadingComplete = false;
        localReadCount += 1;
        if (localReadCount == 3)
        {
            if (_ppgBodyCheck == false) { readCharacteristicFromServiceInt(_ppgServiceUUID, _bodySensorLocationUUID, receiveText4PpgBody, uint8_tDecode); }
        }

        if (localReadCount == 6)
        {
            if (_gsrBodyCheck == false) { readCharacteristicFromServiceInt(_gsrServiceUUID, _bodySensorLocationGsrUUID, receiveTextGsrBody, uint8_tDecode); }
        }

        if (localReadCount == 9)
        {
            if (_DeviceInfoCheck == false) { readCharacteristicFromServiceString(_DeviceInfoUUID, _FirmwareRevisionUUID, receiveTextInfo, UTF8Decode); }
        }

        if (localReadCount == 12)
        {
            localReadCount = 0;
            if (_ppgBodyCheck == true && _gsrBodyCheck == true && _DeviceInfoCheck == true)
            {
                _readFound2 = false;
                _allReadingComplete = true;
            }
        }
    }

    // Decoding byte[] array functions
    int uint8_tDecode (byte[] raw_data)
    {
        int value_data = int.Parse(BitConverter.ToString(raw_data), System.Globalization.NumberStyles.HexNumber);
        BluetoothLEHardwareInterface.Log("data: " + value_data);
        return value_data;
    }

    int uint16_tDecode(byte[] raw_data)
    {
        int value_data = BitConverter.ToInt16(raw_data, 0);
        BluetoothLEHardwareInterface.Log("data: " + value_data);
        return value_data;
    }

    float float32Decode(byte[] raw_data)
    {
        float value_data = BitConverter.ToSingle(raw_data, 0);
        BluetoothLEHardwareInterface.Log("data: " + value_data);
        return value_data;
    }

    string UTF8Decode(byte[] raw_data)
    {
        string value_data = Encoding.UTF8.GetString(raw_data);
        BluetoothLEHardwareInterface.Log("data: " + value_data);
        return value_data;
    }

    // Subscribes to the data stream of an Integer characteristic, updating when a notification is received. 
    void subscribeToCharacteristicInt(string serviceUUID, string characteristicUUID, Action<int> dataprocessing, Func<byte[], int> Decoder)
    {
        BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(_connectedAddress, serviceUUID, characteristicUUID, (deviceAddress, notification) =>
        {
            BluetoothLEHardwareInterface.Log("Notification: " + notification);

            if (characteristicUUID == _HRmeasurementUUID) { _HR_Notification = true; }
            if (characteristicUUID == _InterbeatIntervalUUID) { _IBI_Notification = true; }
            if (characteristicUUID == _Spo2MeasurementUUID) { _Spo2_Notification = true; }
            if (characteristicUUID == _skinConductanceLevelUUID) { _SkinConductance_Notification = true; }

        }, (deviceAddress2, characteristic, data) =>
        {
            BluetoothLEHardwareInterface.Log("id: " + _connectedAddress);
            BluetoothLEHardwareInterface.Log("received data: " + characteristic);
            if (deviceAddress2.CompareTo(_connectedAddress) == 0)
            {
                BluetoothLEHardwareInterface.Log(string.Format("data length: {0}", data.Length));
                if (data.Length == 0) {}
                else
                {
                    int decoded_data = Decoder(data);
                    dataprocessing(decoded_data);
                }
            }
        });
    }

    // Subscribes to the data stream of a Float characteristic, updating when a notification is received. 
    void subscribeToCharacteristicFloat(string serviceUUID, string characteristicUUID, Action<float> dataprocessing, Func<byte[], float> Decoder)
    {
        BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(_connectedAddress, serviceUUID, characteristicUUID, (deviceAddress, notification) =>
        {
            BluetoothLEHardwareInterface.Log("Notification: " + notification);
            if (characteristicUUID == _HRmeasurementUUID) { _HR_Notification = true; }
            if (characteristicUUID == _InterbeatIntervalUUID) { _IBI_Notification = true; }
            if (characteristicUUID == _Spo2MeasurementUUID) { _Spo2_Notification = true; }
            if (characteristicUUID == _skinConductanceLevelUUID) { _SkinConductance_Notification = true; }
        }, (deviceAddress2, characteristic, data) =>
        {
            BluetoothLEHardwareInterface.Log("id: " + _connectedAddress);
            BluetoothLEHardwareInterface.Log("received data: " + characteristic);
            if (deviceAddress2.CompareTo(_connectedAddress) == 0)
            {
                BluetoothLEHardwareInterface.Log(string.Format("data length: {0}", data.Length));
                if (data.Length == 0) { }
                else
                {
                    float decoded_data = Decoder(data);
                    dataprocessing(decoded_data);
                }
            }
        });
    }

    // Remove all old devices. This is only called once when starting the scan() function. 
    void RemovePeripherals()
    {
        for (int i = 0; i < PanelScrollContents.childCount; ++i)
        {
            // Destroys all buttons
            GameObject gameObject = PanelScrollContents.GetChild(i).gameObject;
            Destroy(gameObject);
        }

        if (_peripheralList != null)
        {
            _peripheralList.Clear();
        }
    }

    // Adds a button object for each peripheral discovered. This has a modification in to only discover the MHML M5 device. 
    void AddPeripheral(string name, string address)
    {
        if (_peripheralList == null)
        {
            _peripheralList = new Dictionary<string, string>();
        }
        if (!_peripheralList.ContainsKey(address))
        {
            txtDebug.text = "Found " + name + "\n";
            
            // Button prefab only created if the MHML M5 exists. 
            if (name == _permittedDeviceName)
            {
                devicesFound++;
                GameObject buttonObject = (GameObject)Instantiate(connectButton);
                connectButtonScript script = buttonObject.GetComponent<connectButtonScript>();
                script.TextName.text = name;
                // _connectedName = name;
                script.TextAddress.text = address;
                // _connectedAddress = address;
                script.controllerScript = this;

                // each button is 50 pixels high 
                // the container panel is 443 pixels high 
                var h = (675 / 2) - (100 * devicesFound) - 20;

                buttonObject.transform.SetParent(PanelScrollContents);
                buttonObject.transform.localScale = new Vector3(1f, 1f, 1f);
                buttonObject.transform.localPosition = new Vector3(0, h, 0);

                _peripheralList[address] = name;

                if (_hasStoredBluetoothValues==true && isConnected == false && _connecting == false) // Need to stop instantly assigning, and wait for a check to see if the peripheral is the one. 
                {
                    if (address == storedAddress)
                    {
                        _connectedName = storedName;
                        _connectedAddress = storedAddress;
                    }
                }
            }
        }
    }

    // Places the received text in the first textbox 
    void receiveTextHR(int s)
    {
		if (_storeSubscribeData == true) {
			_HR_data.Enqueue (s);
		}
		txtReceive.text = s.ToString();
    }

    // Places the received text in the second textbox
    void receiveTextIBI(int s)
    {
		if (_storeSubscribeData == true) {
			_IBI_data.Enqueue (s);
		}
        txtReceive2.text = s.ToString();
    }

    void receiveTextSpo2(float s)
    {
		if (_storeSubscribeData == true) {
			_Spo2_data.Enqueue (s);
		}
        txtReceive3.text = s.ToString();
    }

    void receiveText4PpgBody(int s)
    {
		_ppgLocation_data.Enqueue (s);
        txtReceive4.text = s.ToString();
        _ppgBodyCheck = true; 
    }

    void receiveTextSClvl(int s)
    {
		if (_storeSubscribeData == true) {
			_skinConductance_data.Enqueue (s);
		}
        txtReceive5.text = s.ToString();
    }

    void receiveTextGsrBody(int s)
    {
        _gsrLocation_data.Enqueue(s);
        txtReceive6.text = s.ToString();
        _gsrBodyCheck = true;
    }

    void receiveTextInfo(string s)
    {
        _deviceInfo_data.Enqueue(s);
        txtReceive7.text = s;
        _DeviceInfoCheck = true;
    }

    // Used to clear the received text in the first textbox. Currently a redundant function. Review usage later on.
    public void clearReceived()
    {
        txtReceive.text = "";
    }

    // Displays the scanning panel object. 
    void showScan()
    {
        panelConnected.SetActive(false);
        panelScan.SetActive(true);
    }

    // Displays the BLE connected panel (data streaming) object. 
    void showConnected()
    {
        panelScan.SetActive(false);
        panelConnected.SetActive(true);
    }

    // Currently a redundant function. Review usage later on.
    void showSettings()
    {
        panelScan.SetActive(false);
        panelConnected.SetActive(false);
    }

    // Initialise the BLE plugin
    void Initialise()
    {
        BluetoothLEHardwareInterface.Initialize(true, false, () => { }, (error) => { });
    }

    // Use this for initialization 
    void Start()
    {
        txtDebug.text = "No address";
        if (PlayerPrefs.HasKey("Saved Device Name"))  // check if we already save It before
        {
            storedName = PlayerPrefs.GetString("Saved Device Name");
            txtDebug.text = storedName;
        }
        if (PlayerPrefs.HasKey("Saved Device Address"))  // check if we already save It before
        {
            storedAddress = PlayerPrefs.GetString("Saved Device Address");
            _hasStoredBluetoothValues = true;
            txtDebug.text += storedAddress;
        }

        panelScan = GameObject.Find("panelScan");
        panelConnected = GameObject.Find("panelConnected");

        // set up the panels 
        showScan();

        // initialise the bluetooth library 
        Initialise();

        // start scanning after 1.5 seconds
        System.Threading.Thread.Sleep(1000);
        scan();
    }

    // Update is called once per frame. Currently unsure of the frame refresh rate. 
    void Update()
    {      
        // The counts and boolean checks are implemented to prevent synchronous subscribe attempts. This ensures each subscribe method occurs subsequently
        // Ensuring all characteristics can be subscribed to. 
        if (_readFound == true) 
        {
            _allSubscribingComplete = false;
            count += 1;
            if (count == 2)
            {
                if (_HR_Notification != true) { subscribeToCharacteristicInt(_ppgServiceUUID, _HRmeasurementUUID, receiveTextHR, uint8_tDecode); }
            }

            if (count == 5)
            {
                if (_IBI_Notification != true) { subscribeToCharacteristicInt(_ppgServiceUUID, _InterbeatIntervalUUID, receiveTextIBI, uint16_tDecode); }
            }

            if (count == 10)
            {
                if (_Spo2_Notification != true) { subscribeToCharacteristicFloat(_ppgServiceUUID, _Spo2MeasurementUUID, receiveTextSpo2, float32Decode); }
            }

            if (count == 15)
            {
                if (_SkinConductance_Notification != true) { subscribeToCharacteristicInt(_gsrServiceUUID, _skinConductanceLevelUUID, receiveTextSClvl, uint16_tDecode); }
            }

            if (count == 20)
            {
                count = 0;
                if (_HR_Notification == true && _IBI_Notification == true && _Spo2_Notification == true && _SkinConductance_Notification == true)
                {
                    _readFound = false;
                    _allSubscribingComplete = true;
                }
            }
        }

        if (_readFound2 == true)
        {
           readNonNotifyCharacteristics(); // This function works in the same way as the count loop above. 
        }

        // This autoconnect logic should only run if:
        // 1) A stored device name exists (loaded upon start).
        // 2) The device is not already in the process of connecting.
        // 3) The device is not already connected. 

        if (_connectedName != null && _connectedAddress != null && isConnected == false && _connecting == false && _hasStoredBluetoothValues == true)
        {
            _autoConnectTimer += 1; 
            if (_autoConnectTimer == 20)
            {
                txtDebug.text += "AutoConnect Entered";
                txtDebug.text = storedName;
                txtDebug.text += storedAddress;
                connectTo(storedName, storedAddress);
            }
            if (_autoConnectTimer == 40)
            {
                _autoConnectTimer = 0;
            }
        }

        // Automatic Rescanning to update peripheral list every 12.5 seconds. Only called when not connected/connecting to a device. 
        if (isConnected == false && _connecting==false && _scanning == false)
        {
            _reScanTimer += 1;
            if (_reScanTimer == 700)
            {
                txtDebug.text = "RE SCAN TIME";
                BluetoothLEHardwareInterface.StopScan();
                scan();
            }
            if (_reScanTimer == 750)
            {
                _reScanTimer = 0;
            }
        }
    }
}
#include <M5Stack.h>
#include <BLEDevice.h>
#include <BLEServer.h>
#include <BLEUtils.h>
#include <BLE2902.h>

#ifndef ble_h
#define ble_h

String firmwareRevision = "0.1.0";

// UUIDs specified by the Bluetooth GATT Specification: https://www.bluetooth.com/specifications/gatt
#define SERVICE_UUID_DEVICE_INFORMATION "0000180A-0000-1000-8000-00805f9b34fb"  // Device Information Service (official)
#define CHARACTERISTIC_UUID_FIRMWARE_REV "00002A26-0000-1000-8000-00805f9b34fb" // Firmware Revision (official)
#define CHARACTERISTIC_UUID_HR "00002A37-0000-1000-8000-00805f9b34fb"           // Heart Rate Measurement (official)
#define CHARACTERISTIC_UUID_BSL "00002A38-0000-1000-8000-00805f9b34fb"          // Body Sensor Location (official)

// For custom UUIDs, see the following for generating: https://www.uuidgenerator.net/
#define SERVICE_UUID_PPG "1a632076-8702-41b9-bcff-ea119ae68a69"       // PPG Sensor (custom)
#define SERVICE_UUID_GSR "720f8954-ace5-41f7-acec-113b274bc54f"       // GSR sensor (custom)
#define CHARACTERISTIC_UUID_O2 "ef4684bb-c958-40df-90be-5eaa65e07948" // Oxygen level of blood (custom)
#define CHARACTERISTIC_UUID_SR "3f18d911-bffd-4236-b5fc-94c9bf27d0e8" // Skin resistance level (custom)
// #define CHARACTERISTIC_UUID_OTHER "6bb32e9e-41fd-4abc-8089-f24dbe18aa61" //  (custom)

BLEServer *pServer = NULL;
BLEService *pServicePPG = NULL;
BLEService *pServiceGSR = NULL;
BLEService *pServiceDevInfo = NULL;
BLECharacteristic *pCharacteristicPPG_HR = NULL;
BLECharacteristic *pCharacteristicPPG_SPO2 = NULL;
BLECharacteristic *pCharacteristicPPG_BSL = NULL;
BLECharacteristic *pCharacteristicGSR_SR = NULL;
BLECharacteristic *pCharacteristicGSR_BSL = NULL;
BLECharacteristic *pCharacteristicFR = NULL;
bool deviceConnected = false;
bool oldDeviceConnected = false;
uint32_t dummyHeartRate = 0;

class customServerCallbacks : public BLEServerCallbacks
{
    void onConnect(BLEServer *pServer)
    {
        deviceConnected = true;
    };

    void onDisconnect(BLEServer *pServer)
    {
        deviceConnected = false;
    }
};

void bleInit()
{
    // Create the BLE Device
    BLEDevice::init("MHML M5");

    // Create the BLE Server
    pServer = BLEDevice::createServer();
    pServer->setCallbacks(new customServerCallbacks());

    /*
    Create the BLE Service
    */
    pServicePPG = pServer->createService(SERVICE_UUID_PPG);
    pServiceGSR = pServer->createService(SERVICE_UUID_GSR);
    pServiceDevInfo = pServer->createService(SERVICE_UUID_DEVICE_INFORMATION);

    /*
    Create a BLE Characteristics for each service
    */
    pCharacteristicPPG_HR = pServicePPG->createCharacteristic(
        CHARACTERISTIC_UUID_HR,
        BLECharacteristic::PROPERTY_READ | BLECharacteristic::PROPERTY_NOTIFY);

    pCharacteristicPPG_SPO2 = pServicePPG->createCharacteristic(
        CHARACTERISTIC_UUID_O2,
        BLECharacteristic::PROPERTY_READ | BLECharacteristic::PROPERTY_NOTIFY);

    pCharacteristicPPG_BSL = pServicePPG->createCharacteristic(
        CHARACTERISTIC_UUID_BSL,
        BLECharacteristic::PROPERTY_READ);

    pCharacteristicGSR_SR = pServiceGSR->createCharacteristic(
        CHARACTERISTIC_UUID_SR,
        BLECharacteristic::PROPERTY_READ | BLECharacteristic::PROPERTY_NOTIFY);

    pCharacteristicGSR_BSL = pServiceGSR->createCharacteristic(
        CHARACTERISTIC_UUID_BSL,
        BLECharacteristic::PROPERTY_READ);

    pCharacteristicFR = pServiceDevInfo->createCharacteristic(
        CHARACTERISTIC_UUID_FIRMWARE_REV,
        BLECharacteristic::PROPERTY_READ);

    /*
    All characterstics with the the Notify property must also have the CCC (Client Characteristic Configuration) descriptor.

    https://www.bluetooth.com/specifications/gatt/viewer?attributeXmlFile=org.bluetooth.descriptor.gatt.client_characteristic_configuration.xml
    */
    pCharacteristicPPG_HR->addDescriptor(new BLE2902());
    pCharacteristicPPG_SPO2->addDescriptor(new BLE2902());
    pCharacteristicGSR_SR->addDescriptor(new BLE2902());

    // Start the service and then advertise
    pServicePPG->start();
    pServiceGSR->start();
    pServiceDevInfo->start();

    BLEAdvertising *pAdvertising = BLEDevice::getAdvertising();
    pAdvertising->addServiceUUID(SERVICE_UUID_PPG);
    pAdvertising->setScanResponse(false);
    pAdvertising->setMinPreferred(0x0); // set dummyHeartRate to 0x00 to not advertise this parameter
    BLEDevice::startAdvertising();
    Serial.println("Waiting for a client connection to notify...");
}

void bleRun()
{
    M5.Lcd.setTextColor(GREEN, BLACK);
    M5.Lcd.setTextSize(2); // 20 px high
    M5.Lcd.setCursor(0, 32);
    M5.Lcd.print(dummyHeartRate);

    // notify changed dummyHeartRate
    if (deviceConnected)
    {
        int bsl = 1;
        pCharacteristicPPG_BSL->setValue((uint8_t *)&bsl, 4);
        pCharacteristicFR->setValue(firmwareRevision.c_str());
        pCharacteristicPPG_HR->setValue((uint8_t *)&dummyHeartRate, 4);
        pCharacteristicPPG_HR->notify();
        dummyHeartRate++;
        delay(5); // bluetooth stack will go into congestion if too many packets are sent; ensure loop is delayed

        Serial.print("Dummy Heart Rate: 0x");
        Serial.println(dummyHeartRate, HEX);
        delay(1000);
    }
    // disconnecting
    if (!deviceConnected && oldDeviceConnected)
    {
        delay(500);                  // give the bluetooth stack the chance to get things ready
        pServer->startAdvertising(); // restart advertising
        Serial.println("start advertising");
        oldDeviceConnected = deviceConnected;
    }
    // connecting
    if (deviceConnected && !oldDeviceConnected)
    {
        // do stuff here on connecting
        oldDeviceConnected = deviceConnected;
    }
}

#endif
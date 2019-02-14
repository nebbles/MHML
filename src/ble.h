#include <M5Stack.h>
#include <BLEDevice.h>
#include <BLEServer.h>
#include <BLEUtils.h>
#include <BLE2902.h>

#ifndef ble_h
#define ble_h

BLEServer *pServer = NULL;
BLECharacteristic *pCharacteristic = NULL;
bool deviceConnected = false;
bool oldDeviceConnected = false;
uint32_t value = 0;

// See the following for generating UUIDs: https://www.uuidgenerator.net/
#define SERVICE_UUID_PPG "1a632076-8702-41b9-bcff-ea119ae68a69"       // PPG Sensor (custom)
#define CHARACTERISTIC_UUID_HR "6bb32e9e-41fd-4abc-8089-f24dbe18aa61" // Heart rate in BPM (custom)
#define CHARACTERISTIC_UUID_O2 "ef4684bb-c958-40df-90be-5eaa65e07948" // Oxygen level of blood (custom)
#define SERVICE_UUID_GSR "720f8954-ace5-41f7-acec-113b274bc54f"       // GSR sensor (custom)
#define CHARACTERISTIC_UUID_SR "3f18d911-bffd-4236-b5fc-94c9bf27d0e8" // Skin resistance level (custom)
// #define SERVICE_UUID_HEART_RATE "0000180A-0000-1000-8000-00805f9b34fb" // Device Information Service (official)

class MyServerCallbacks : public BLEServerCallbacks
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
    pServer->setCallbacks(new MyServerCallbacks());

    // Create the BLE Service
    BLEService *pService = pServer->createService(SERVICE_UUID_PPG);

    // Create a BLE Characteristic
    pCharacteristic = pService->createCharacteristic(
        CHARACTERISTIC_UUID_HR,
        BLECharacteristic::PROPERTY_READ |
            BLECharacteristic::PROPERTY_WRITE |
            BLECharacteristic::PROPERTY_NOTIFY |
            BLECharacteristic::PROPERTY_INDICATE);

    // https://www.bluetooth.com/specifications/gatt/viewer?attributeXmlFile=org.bluetooth.descriptor.gatt.client_characteristic_configuration.xml
    // Create a BLE Descriptor
    pCharacteristic->addDescriptor(new BLE2902());

    // Start the service
    pService->start();

    // Start advertising
    BLEAdvertising *pAdvertising = BLEDevice::getAdvertising();
    pAdvertising->addServiceUUID(SERVICE_UUID_PPG);
    pAdvertising->setScanResponse(false);
    pAdvertising->setMinPreferred(0x0); // set value to 0x00 to not advertise this parameter
    BLEDevice::startAdvertising();
    Serial.println("Waiting for a client connection to notify...");
}

void bleRun()
{
    M5.Lcd.setTextColor(GREEN, BLACK);
    M5.Lcd.setTextSize(2);
    M5.Lcd.setCursor(0, 32);
    M5.Lcd.print(value);

    // notify changed value
    if (deviceConnected)
    {
        pCharacteristic->setValue((uint8_t *)&value, 4);
        pCharacteristic->notify();
        value++;
        delay(3); // bluetooth stack will go into congestion, if too many packets are sent, in 6 hours test i was able to go as low as 3ms

        Serial.print("Value: 0x");
        Serial.println(value, HEX);
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
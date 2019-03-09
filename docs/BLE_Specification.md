<h2 align="center"><br>BLE Specification</h2>

<p align="center">
Version <code>1.1</code>
</p>
<br>

### Contents

1. [Overview](#overview-)
1. [Profile Definition](#profile-definition-)
   1. [PPG Service](#service-ppg-)
      - Heart Rate Characteristic
      - Interbeat Interval Characteristic
      - SpO2 Characteristic
      - Body Sensor Location Characteristic
   1. [GSR Service](#service-gsr-)
      - Skin Conductance Level Characteristic
      - Body Sensor Location Characteristic
   1. [Device Information Service](#service-device-information-)
      - Firmware Revision
   1. [Body Sensor Location Key](#body-sensor-location-key-)
1. [References](#references-)
1. [Potential Amendments](#potential-amendments-)

### [Overview ↑](#contents)

This document sets out the full detailed specification for Bluetooth Low Energy communication between the M5Stack Fire
device (server) and iOS/Android smartphone device (client).

- BLE was chosen over Bluetooth Classic (serial) due to licensing requirements when using iOS.
- BLE was confirmed as sufficient for data transfer as communications are stateless and low-bandwidth.
- BLE Notifications were chosen (instead of BLE Indications) as confirmation of delivery is not needed, and may 
slow down communications. Dropped data is outdated data, so we focus on getting the latest readings on time.

**Important note**: all characteristics values are sent in little-endian order. 
> Supplement to Bluetooth Core Specification page 9 of 37:  
> "All numerical multi-byte entities and values associated with the following data types shall use little-endian byte order."

### [Profile Definition ↑](#contents)

Profile of server running on M5Stack Fire (base ESP32).  
Device Name: **MHML M5 [B/S/F]**  

#### [Service: PPG ↑](#contents)

Declaration: `Custom`  
UUID: `1a632076-8702-41b9-bcff-ea119ae68a69`

- Characteristic: **H.R. measurement** ([based on GATT](https://www.bluetooth.com/specifications/gatt/viewer?attributeXmlFile=org.bluetooth.characteristic.heart_rate_measurement.xml))
  - Declaration: `0x2A37`
  - UUID: `00002A37-0000-1000-8000-00805f9b34fb`
  - Value: **uint8_t (in BPM)**
  - Properties: **Read, Notify**
  - Descriptor: [**CCC(0x2902)**](https://www.bluetooth.com/specifications/gatt/viewer?attributeXmlFile=org.bluetooth.descriptor.gatt.client_characteristic_configuration.xml)

- Characteristic: **Interbeat Interval**   
  - Declaration: `Custom`
  - UUID: `847dc27a-00f2-4c99-aebf-5eacea5474b4`
  - Value: **uint16_t (in milliseconds)**
  - Properties: **Read, Notify**
  - Descriptor: [**CCC(0x2902)**](https://www.bluetooth.com/specifications/gatt/viewer?attributeXmlFile=org.bluetooth.descriptor.gatt.client_characteristic_configuration.xml)

- Characteristic: **SpO2 measurement**
  - Declaration: `Custom`
  - UUID: `ef4684bb-c958-40df-90be-5eaa65e07948`
  - Value: **float32 (% O2 level)**
  - Properties: **Read, Notify**
  - Descriptor: [**CCC(0x2902)**](https://www.bluetooth.com/specifications/gatt/viewer?attributeXmlFile=org.bluetooth.descriptor.gatt.client_characteristic_configuration.xml)

- Characteristic: **Body sensor location** ([based on GATT](https://www.bluetooth.com/specifications/gatt/viewer?attributeXmlFile=org.bluetooth.characteristic.body_sensor_location.xml))
  - Declaration: `0x2A38`
  - UUID: `00002A38-0000-1000-8000-00805f9b34fb`
  - Value: **uint8_t ([see reference](#body-sensor-location-key))**
  - Properties: **Read**

#### [Service: GSR ↑](#contents)

Declaration: `Custom`  
UUID: `720f8954-ace5-41f7-acec-113b274bc54f`  

- Characteristic: **Skin Conductance Level**
  - Declaration: `Custom`
  - UUID: `3f18d911-bffd-4236-b5fc-94c9bf27d0e8`
  - Value: **uint16_t (raw 12 bit adc)**
  - Properties: **Read, Notify**
  - Descriptor: [**CCC(0x2902)**](https://www.bluetooth.com/specifications/gatt/viewer?attributeXmlFile=org.bluetooth.descriptor.gatt.client_characteristic_configuration.xml)

- Characteristic: **Body sensor location** ([based on GATT](https://www.bluetooth.com/specifications/gatt/viewer?attributeXmlFile=org.bluetooth.characteristic.body_sensor_location.xml))
  - Declaration: `0x2A38`
  - UUID: `00002A38-0000-1000-8000-00805f9b34fb`
  - Value: **uint8_t ([see reference](#body-sensor-location-key))**
  - Properties: **Read**

#### [Service: Device Information ↑](#contents)

([based on GATT](https://www.bluetooth.com/specifications/gatt/viewer?attributeXmlFile=org.bluetooth.service.device_information.xml))  
Declaration: `0x180A`  
UUID: `0000180A-0000-1000-8000-00805f9b34fb`  

  - Characteristic: **Firmware Revision** ([based on GATT](https://www.bluetooth.com/specifications/gatt/viewer?attributeXmlFile=org.bluetooth.characteristic.firmware_revision_string.xml))  
    - Declaration: `0x2A26`
    - UUID: `00002A26-0000-1000-8000-00805f9b34fb`
    - Value: **UTF-8 String**
    - Properties: **Read**

#### [Body Sensor Location Key ↑](#contents)

| Key | Value |
| --- | ----- |
| 0   | Other |
| 1   | Chest |
| 2   | Wrist |
| 3   | Finger|
| 4   | Hand  |
| 5   | Ear Lobe|
| 6   | Foot  |
| 7-255 | Reserved for future use |

### [References ↑](#contents)

1. https://www.bluetooth.com/specifications/gatt/generic-attributes-overview
1. https://github.com/nkolban/ESP32_BLE_Arduino/blob/master/examples/BLE_notify/BLE_notify.ino
1. https://github.com/nkolban/esp32-snippets/blob/master/Documentation/BLE%20C%2B%2B%20Guide.pdf

### [Potential Amendments ↑](#contents)

1. SpO2 measurement may be altered in future revision to support the following GATT Characteristics. 
   This may include moving SpO2 to its own service. See references:
   - https://www.bluetooth.com/specifications/gatt/viewer?attributeXmlFile=org.bluetooth.characteristic.plx_continuous_measurement.xml
   - https://www.bluetooth.com/specifications/gatt/viewer?attributeXmlFile=org.bluetooth.characteristic.plx_features.xml
   - https://www.bluetooth.com/specifications/gatt/viewer?attributeXmlFile=org.bluetooth.characteristic.plx_spot_check_measurement.xml


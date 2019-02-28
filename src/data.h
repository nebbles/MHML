/*
    Central Data Structure for firmware
    Author: Benedict Greenberg (https://github.com/nebbles)
    Date: 28 February 2019
    
    Defines central data structure for processed data that is handled by BLE module.
*/
#include <M5Stack.h>

#ifndef data_h
#define data_h

struct DataStruct
{
    // Body Sensor Location of PPG. 0=Other, 1=Chest, 2=Wrist, 3=Finger, 4=Hand, 5=Ear Lobe, 6=Foot.
    uint8_t ppgBSL = 0;
    // Value 0-255 in beats per minute
    uint8_t heartRate = 0;
    // Value 0-255 in ??units
    uint8_t heartRateInterval = 0;
    // Value 0-255 in ??units
    uint8_t interbeatInterval = 0;
    // Value 0-255 in ??units
    uint8_t spo2 = 0;

    // Body Sensor Location of PPG. 0=Other, 1=Chest, 2=Wrist, 3=Finger, 4=Hand, 5=Ear Lobe, 6=Foot.
    uint8_t gsrBSL;
    // Value 0-255 in ??units
    uint8_t scl = 0;
    // Value 0-255 in ??units
    uint8_t ns_scr = 0;
    // Value 0-255 in ??units
    uint8_t er_scr = 0;
};
struct DataStruct DATA; // global data object

#endif

/*
    Central Data Structure for firmware
    Author: Benedict Greenberg (https://github.com/nebbles)
    Date: 28 February 2019
    
    Defines central data structure for processed data that is handled by BLE 
    module.
*/
#include <M5Stack.h>

#ifndef data_h
#define data_h

struct DataStruct
{
    // Body Sensor Location of PPG. 0=Other, 1=Chest, 2=Wrist, 3=Finger, 4=Hand, 5=Ear Lobe, 6=Foot.
    uint8_t ppgBSL = 0;
    // 8 bit value in beats per minute
    uint8_t heartRate = 0;
    // 16 bit value in milliseconds
    uint16_t interbeatInterval = 0;
    // 32 bit float as percentage
    float spo2 = 0.0;

    // Body Sensor Location of PPG. 0=Other, 1=Chest, 2=Wrist, 3=Finger, 4=Hand, 5=Ear Lobe, 6=Foot.
    uint8_t gsrBSL = 0;
    // Value 0-255 in ??units
    uint16_t scl = 0;
    // // Value 0-255 in ??units
    // uint8_t ns_scr = 0;
    // // Value 0-255 in ??units
    // uint8_t er_scr = 0;
};
struct DataStruct DATA; // global data object

int hrDelta = 1;
int ibiDelta = 1;
float spo2Delta = 0.2;
int sclDelta = 45;
void simulateDataChanges()
{
    if (DATA.heartRate <= 60)
        hrDelta = -hrDelta;
    if (DATA.heartRate >= 70)
        hrDelta = -hrDelta;
    DATA.heartRate += hrDelta;
    DATA.heartRate = constrain(DATA.heartRate, 60, 70);

    if (DATA.interbeatInterval <= 70)
        ibiDelta = -ibiDelta;
    if (DATA.interbeatInterval >= 80)
        ibiDelta = -ibiDelta;
    DATA.interbeatInterval += ibiDelta;
    DATA.interbeatInterval = constrain(DATA.interbeatInterval, 70, 80);

    if (DATA.spo2 <= 97)
        spo2Delta = -spo2Delta;
    if (DATA.spo2 >= 99)
        spo2Delta = -spo2Delta;
    DATA.spo2 += spo2Delta;
    DATA.spo2 = constrain(DATA.spo2, 97, 99);

    if (DATA.scl <= 0)
        sclDelta = -sclDelta;
    if (DATA.scl >= 4095)
        sclDelta = -sclDelta;
    DATA.scl += sclDelta;
    DATA.scl = constrain(DATA.scl, 0, 4095);
}
#endif
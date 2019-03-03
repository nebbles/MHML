/********************************************************
*
* Project: MAXREFDES117#
* Filename: RD117_ARDUINO.ino
* Description: This module contains the Main application for the MAXREFDES117 example program.
*
* Revision History:
*\n 1-18-2016 Rev 01.00 GL Initial release.
*\n 12-22-2017 Rev 02.00 Significantlly modified by Robert Fraczkiewicz
*\n 08-22-2018 Rev 02.01 Added conditional compilation of the code related to ADALOGGER SD card operations
*
* --------------------------------------------------------------------
*
* This code follows the naming conventions:
*
* char              ch_pmod_value
* char (array)      s_pmod_s_string[16]
* float             f_pmod_value
* int32_t           n_pmod_value
* int32_t (array)   an_pmod_value[16]
* int16_t           w_pmod_value
* int16_t (array)   aw_pmod_value[16]
* uint16_t          uw_pmod_value
* uint16_t (array)  auw_pmod_value[16]
* uint8_t           uch_pmod_value
* uint8_t (array)   auch_pmod_buffer[16]
* uint32_t          un_pmod_value
* int32_t *         pn_pmod_value
*
* ------------------------------------------------------------------------- */

#ifndef ppg_h
#define ppg_h

#include <M5Stack.h>
#include <Wire.h>
#include <SPI.h>
#include "algorithm_by_RF.h"
#include "max30102.h"
#include "Deque.h"

Deque<int> ppgQueue;

// #define DEBUG // Uncomment for debug output to the Serial stream
#define TEST_MAXIM_ALGORITHM // Uncomment if you want to include results returned by the original MAXIM algorithm
// #define showRed

#ifdef TEST_MAXIM_ALGORITHM
  #include "algorithm.h" 
#endif

// Interrupt pin
const byte oxiInt = 5; // pin connected to MAX30102 INT

uint32_t elapsedTime, timeStart;
uint32_t aun_ir_buffer[BUFFER_SIZE]; //infrared LED sensor data
uint32_t aun_red_buffer[BUFFER_SIZE];  //red LED sensor data
float old_n_spo2;  // Previous SPO2 value
uint8_t uch_dummy,k;
int bound = 500;

// Variables from previous loop function
float n_spo2,ratio,correl;  //SPO2 value
int8_t ch_spo2_valid;  //indicator to show if the SPO2 calculation is valid
int32_t n_heart_rate; //heart rate value
int8_t  ch_hr_valid;  //indicator to show if the heart rate calculation is valid
int32_t i;
char hr_str[10];
long graphPos;
float raw_ir, raw_red;

void ppgInit()
{
pinMode(oxiInt, INPUT);  //pin G5 connects to the interrupt output pin of the MAX30102
maxim_max30102_reset(); //resets the MAX30102
delay(1000);

  maxim_max30102_read_reg(REG_INTR_STATUS_1,&uch_dummy);  //Reads/clears the interrupt status register
  maxim_max30102_init();  //initialize the MAX30102
  old_n_spo2=0.0;

  uch_dummy=Serial.read();
  
#ifdef TEST_MAXIM_ALGORITHM
  Serial.print(F("Time[s]\tSpO2\tHR\tSpO2_MX\tHR_MX\tClock\tRatio\tCorr"));
#else // TEST_MAXIM_ALGORITHM
  Serial.print(F("Time[s]\tSpO2\tHR\tClock\tRatio\tCorr"));
#endif // TEST_MAXIM_ALGORITHM

  Serial.println("");
  ppgQueue.setLimit(280);
  for(int i=0; i<280; i++) ppgQueue.pushTail(100);
}

void ppgInter()
{
    maxim_max30102_read_fifo((aun_red_buffer+i), (aun_ir_buffer+i));  //read from MAX30102 FIFO
    raw_ir = raw_ir_read(aun_ir_buffer, BUFFER_SIZE, aun_red_buffer, &n_spo2, &n_heart_rate);
    raw_red = raw_red_read(BUFFER_SIZE, aun_red_buffer, &n_spo2, &n_heart_rate);
    
    #ifdef showRed
    if(raw_red < bound && raw_red > -bound) // If IR reflectance values are consistent with HR variance
    {
        graphPos = map(raw_red, -bound, bound, 200, 0);
        graphPos = floor(graphPos);
        ppgQueue.popHead();
        ppgQueue.pushTail(graphPos);
    }
    #else // showRed
    if(raw_ir < bound && raw_ir > -bound) // If IR reflectance values are consistent with HR range
    {
        graphPos = map(raw_ir, -bound, bound, 200, 0);
        graphPos = floor(graphPos);
        ppgQueue.popHead();
        ppgQueue.pushTail(graphPos);
    }
    #endif //showRed
#ifdef DEBUG
    Serial.println("Interrupt Loop");
    Serial.print(aun_red_buffer[i], DEC);
    Serial.print(F("\n"));
    Serial.println((float)(raw_ir));
    Serial.println((float)(raw_red));


    Serial.print(i, DEC);
    Serial.print(F("\t"));
    Serial.print(aun_red_buffer[i], DEC);
    Serial.print(F("\t"));
    Serial.print(aun_ir_buffer[i], DEC);    
    Serial.println("");
#endif // DEBUG
}

void ppgCalc()
{
  //calculate heart rate and SpO2 after BUFFER_SIZE samples (ST seconds of samples) using Robert's method
  rf_heart_rate_and_oxygen_saturation(aun_ir_buffer, BUFFER_SIZE, aun_red_buffer, &n_spo2, &ch_spo2_valid, &n_heart_rate, &ch_hr_valid, &ratio, &correl);
  // millis_to_hours(elapsedTime,hr_str); // Time in hh:mm:ss format
  elapsedTime=millis()-timeStart;
  elapsedTime/=1000; // Time in seconds

#ifdef DEBUG
  Serial.println("--RF--");
  Serial.print(elapsedTime);
  Serial.print("\t");
  Serial.print(n_spo2);
  Serial.print("\t");
  Serial.print(n_heart_rate, DEC);
  Serial.print("\t");
  Serial.println(hr_str);
  Serial.println("------");
#endif // DEBUG

#ifdef TEST_MAXIM_ALGORITHM
  //calculate heart rate and SpO2 after BUFFER_SIZE samples (ST seconds of samples) using MAXIM's method
  float n_spo2_maxim;  //SPO2 value
  int8_t ch_spo2_valid_maxim;  //indicator to show if the SPO2 calculation is valid
  int32_t n_heart_rate_maxim; //heart rate value
  int8_t  ch_hr_valid_maxim;  //indicator to show if the heart rate calculation is valid
  maxim_heart_rate_and_oxygen_saturation(aun_ir_buffer, BUFFER_SIZE, aun_red_buffer, &n_spo2_maxim, &ch_spo2_valid_maxim, &n_heart_rate_maxim, &ch_hr_valid_maxim); 
#ifdef DEBUG
  Serial.println("--MX--");
  Serial.print(elapsedTime);
  Serial.print("\t");
  Serial.print(n_spo2_maxim);
  Serial.print("\t");
  Serial.print(n_heart_rate_maxim, DEC);
  Serial.print("\t");
  Serial.println(hr_str);
  Serial.println("------");
#endif // DEBUG
#endif // TEST_MAXIM_ALGORITHM

  //save samples and calculation result to SD card
#ifdef TEST_MAXIM_ALGORITHM
  if(ch_hr_valid && ch_spo2_valid || ch_hr_valid_maxim && ch_spo2_valid_maxim) {
#else   // TEST_MAXIM_ALGORITHM
  if(ch_hr_valid && ch_spo2_valid) { 
#endif // TEST_MAXIM_ALGORITHM
    Serial.print(elapsedTime);
    Serial.print("\t");
    Serial.print(n_spo2);
    Serial.print("\t");
    Serial.print(n_heart_rate, DEC);
    Serial.print("\t");
#ifdef TEST_MAXIM_ALGORITHM
    Serial.print(n_spo2_maxim);
    Serial.print("\t");
    Serial.print(n_heart_rate_maxim, DEC);
    Serial.print("\t");
#endif //TEST_MAXIM_ALGORITHM
    Serial.print(hr_str);
    Serial.print("\t");
    Serial.print(ratio);
    Serial.print("\t");
    Serial.print(correl);
    Serial.println("");
  }
}

#endif // ppg_h
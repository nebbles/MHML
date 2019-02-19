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

#include <M5Stack.h>
#include "ppg.h"

void setup() {

  M5.begin();
  Wire.begin();

#if defined(DEBUG) || !defined(USE_ADALOGGER)
  // initialize serial communication at 115200 bits per second:
  Serial.begin(115200);
#endif

  // Write text to show that PPG Script is Running
  M5.Lcd.setTextColor(YELLOW);
  M5.Lcd.setTextSize(2);
  M5.Lcd.setCursor(0, 0);
  M5.Lcd.print("PPG Script: Started");

  //Draw lines for graph
  M5.Lcd.drawLine(40, 20, 40, 220, WHITE);
  M5.Lcd.drawLine(40, 220, 320, 220, WHITE);

  while(Serial.available()==0)  //wait until user presses a key
  {
    Serial.println(F("Press any key to start conversion"));
    delay(1000);
  }

ppgInit();

  // Write text to show that Measurement has Started
  M5.Lcd.setTextColor(YELLOW, BLACK);
  M5.Lcd.setTextSize(2);
  M5.Lcd.setCursor(0, 0);
  M5.Lcd.print("PPG Script: Serial Running");

    
  timeStart=millis();
}

//Continuously taking samples from MAX30102.  Heart rate and SpO2 are calculated every ST seconds
void loop() 
{
ppgRun();

float raw_ir = raw_ir_read(aun_ir_buffer, BUFFER_SIZE, aun_red_buffer, &n_spo2, &n_heart_rate);
float raw_red = raw_red_read(BUFFER_SIZE, aun_red_buffer, &n_spo2, &n_heart_rate);
#ifdef showIR
if(raw_ir < 1000 && raw_ir > -1000) // If IR reflectance values are consistent with HR variance
{
    long graphPos = map(raw_ir, -1000, 1000, 200, 0);
    graphPos = floor(graphPos);
    ppgQueue.push(graphPos);
}
#endif // showIR

#ifdef showRed
if(raw_red < 1000 && raw_red > -1000) // If IR reflectance values are consistent with HR variance
{
    graphPos = map(raw_red, -1000, 1000, 200, 0);
    graphPos = floor(graphPos);
    ppgQueue.push(graphPos);
}
#endif // showRed

M5.Lcd.fillRect(0, 20, 39, 240, BLACK); // Moving value for IR reflectance
M5.Lcd.setCursor(0, graphPos);
M5.Lcd.print((int)(graphPos));
M5.Lcd.fillRect(41, 20, 280, 200, BLACK);                   // Clear and reset the screen
for(int i=0; i<280; i++)
{
int graphPos = ppgQueue.read(i);
if (graphPos > 20 && graphPos < 220) M5.Lcd.drawPixel(i + 41, graphPos, BLUE);  //Temp fix to prevent diagonal line from being drawn
}
elapsedTime=millis()-timeStart;
M5.Lcd.setTextColor(RED, BLACK);
M5.Lcd.setCursor(0, 30);
M5.Lcd.print((int)(elapsedTime/1000));
}

void millis_to_hours(uint32_t ms, char* hr_str)
{
  char istr[6];
  uint32_t secs,mins,hrs;
  secs=ms/1000; // time in seconds
  mins=secs/60; // time in minutes
  secs-=60*mins; // leftover seconds
  hrs=mins/60; // time in hours
  mins-=60*hrs; // leftover minutes
  itoa(hrs,hr_str,10);
  strcat(hr_str,":");
  itoa(mins,istr,10);
  strcat(hr_str,istr);
  strcat(hr_str,":");
  itoa(secs,istr,10);
  strcat(hr_str,istr);
}


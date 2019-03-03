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

//   while(Serial.available()==0)  //wait until user presses a key
//   {
//     Serial.println(F("Press any key to start conversion"));
//     delay(1000);
//   }

ppgInit();

  // Write text to show that Measurement has Started
  M5.Lcd.setTextColor(YELLOW, BLACK);
  M5.Lcd.setTextSize(2);
  M5.Lcd.setCursor(0, 0);
  M5.Lcd.print("PPG Script: Serial Running");
}

//Continuously taking samples from MAX30102.  Heart rate and SpO2 are calculated every ST seconds
void loop() 
{

//buffer length of BUFFER_SIZE stores ST seconds of samples running at FS sps
//read BUFFER_SIZE samples, and determine the signal range
for(i=0;i<BUFFER_SIZE;i++)
{
    while(digitalRead(oxiInt)==1);  //wait until the interrupt pin asserts
    ppgInter();

    #ifdef showRed
    if(raw_red < bound && raw_red > -bound) // If IR reflectance values are consistent with HR variance
    {
        graphPos = map(raw_red, -bound, bound, 200, 0);
        graphPos = floor(graphPos);
        ppgQueue.popHead();
        ppgQueue.pushTail(graphPos);
    }
    #else // showRed
    if(raw_ir < bound && raw_ir > -bound) // If IR reflectance values are consistent with HR variance
    {
        graphPos = map(raw_ir, -bound, bound, 200, 0);
        graphPos = floor(graphPos);
        ppgQueue.popHead();
        ppgQueue.pushTail(graphPos);
    }
    #endif //showRed

    M5.Lcd.fillRect(0, 20, 39, 200, BLACK); // Moving value for IR reflectance
    M5.Lcd.setCursor(0, graphPos);
    M5.Lcd.print((int)(graphPos));
    M5.Lcd.fillRect(41, 20, 280, 200, BLACK);                   // Clear and reset the screen
    for(int i=0; i<280; i++)
    {
    int graphPos = ppgQueue[i];
    if (graphPos > 20 && graphPos < 220) M5.Lcd.drawPixel(i + 41, graphPos, BLUE);  //Temp fix to prevent diagonal line from being drawn
    }
}

ppgCalc(); //this calculates the heart rate and prints via Serial

M5.Lcd.setCursor(0, 230);
M5.Lcd.setTextSize(1);
M5.Lcd.print("Heart Rate (BPM):");

M5.Lcd.setCursor(100, 230);
M5.Lcd.print((int)(n_heart_rate));

M5.Lcd.setCursor(140, 230);
M5.Lcd.setTextSize(1);
M5.Lcd.print("Blood Oxygen (SpO2):");

M5.Lcd.setCursor(260, 230);
M5.Lcd.print((float)(n_spo2));

}

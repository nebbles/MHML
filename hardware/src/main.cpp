/* 
  M5Stack Fire
    
  Sensors Code
  
  Reading of data from sensors and displaying it on M5Stack screen.

*/

#include <M5Stack.h>
#include "gsr.h"
#include "ppg.h"

// #define DEBUG // Uncomment whilst debugging for Serial debug stats.

void setup()
{
  M5.begin();
  Wire.begin();
  Serial.begin(115200);

  M5.Lcd.setTextColor(YELLOW);
  M5.Lcd.setTextSize(2);
  M5.Lcd.setCursor(0, 0);
  M5.Lcd.print("Sensors Script: ---");
  M5.Lcd.setCursor(0, 20);
  M5.Lcd.print("Press Any Button");

  while (true)
  {
  if (M5.BtnA.wasPressed() || M5.BtnB.wasPressed() || M5.BtnC.wasPressed()) break;
  M5.update();
  }

  ppgInit();
  gsrInit();

  // Reset Screen
  M5.Lcd.fillRect(0, 0, 320, 280, BLACK);

  // Write text to show that Measurement has Started
  M5.Lcd.setTextColor(YELLOW, BLACK);
  M5.Lcd.setTextSize(2);
  M5.Lcd.setCursor(0, 0);
  M5.Lcd.print("Sensors Script: Running");

  // PPG Graph
  M5.Lcd.drawLine(10, 170, 150, 170, WHITE); // Draw x-axis for Graph
  M5.Lcd.drawLine(10, 30, 10, 170, WHITE); // Draw y-axis for Graph
  
  M5.Lcd.setTextColor(WHITE, BLACK);
  M5.Lcd.setTextSize(1);
  M5.Lcd.setCursor(10, 175);
  M5.Lcd.print("PPG Graph");

  // GSR Graph
  M5.Lcd.drawLine(170, 170, 310, 170, WHITE); // Draw x-axis for Graph
  M5.Lcd.drawLine(170, 30, 170, 170, WHITE);   // Draw y-axis for Graph

  M5.Lcd.setTextColor(WHITE, BLACK);
  M5.Lcd.setTextSize(1);
  M5.Lcd.setCursor(170, 175);
  M5.Lcd.print("GSR Graph");
  

  timeStartGSR = millis();
  gsrRun();

}

void loop() //Continuously taking samples from MAX30102.  Heart rate and SpO2 are calculated every ST seconds
{

//buffer length of BUFFER_SIZE stores ST seconds of samples running at FS sps
//read BUFFER_SIZE samples, and determine the signal range
for(i=0;i<BUFFER_SIZE;i++)
{
    while(digitalRead(oxiInt)==1);  //wait until the interrupt pin asserts
    ppgInter();

    M5.Lcd.fillRect(11, 30, 140, 140, BLACK); // Clear and reset PPG Screen
    for(int i=0; i<280; i++)
    {
    int graphPos = ppgDeque[i];
    if (graphPos > 30 && graphPos < 170) M5.Lcd.drawPixel(i + 11, graphPos, BLUE);  //Temp fix to prevent diagonal line from being drawn
    }

    if (millis() - timeStartGSR > 200) // Record GSR at 5 Hz
  {
    #ifdef DEBUG
    Serial.println("[DEBUG]: GSR Run");
    Serial.println((int)(timeStartGSR));
    #endif //DEBUG

    gsrRun();

    M5.Lcd.fillRect(171, 30, 140, 140, BLACK); // Clear and reset GSR Graph

    for (int i = 0; i < 280; i++) // Draw Graph
    {
      int graphPos = gsrDeque[i];

      if (graphPos > 30 && graphPos < 170) // Temp fix to prevent diagonal line from being drawn
        M5.Lcd.drawPixel(i + 171, graphPos, BLUE);
    }
    timeStartGSR = millis();
  }
}

ppgCalc(); //this calculates the heart rate and prints via Serial

// PPG Information
M5.Lcd.fillRect(10, 185, 140, 10, BLACK);
M5.Lcd.setTextColor(WHITE, BLACK);
M5.Lcd.setTextSize(1);

M5.Lcd.setCursor(10, 185);
M5.Lcd.print("HR:  ");
M5.Lcd.setCursor(30, 185);
M5.Lcd.print((int)(n_heart_rate)); // Recorded Heart Rate

M5.Lcd.setCursor(80, 185);
M5.Lcd.print("SpO2:  ");
M5.Lcd.setCursor(110, 185);
M5.Lcd.print((int)(n_spo2)); // Recorded Heart Rate
 /* 
  Project firmware for M5Stack Fire
  Authors: Ben Greenberg & Scott Bunting
  February 2019

  The project is available on GitHub: https://github.com/nebbles/MHML
  Mobile Healthcare & Machine Learning,
  Imperial College London

  Utilises PPG and GSR sensors on body to measure key metrics. Information 
  is hosted and transmitted from a BLE server running on device. Data is
  also displayed on device LCD for convenience.
*/
#include <M5Stack.h>
#include "ble.h"

String FIRMWARE_REVISION = "0.2.1";

void setup()
{
  Serial.begin(115200);
  M5.begin();
  Wire.begin();
  M5.Speaker.mute();

  M5.Lcd.fillScreen(BLACK);
  M5.Lcd.setTextColor(GREEN, BLACK);
  M5.Lcd.setTextSize(2);
  M5.Lcd.setCursor(0, 0);
  M5.Lcd.print("MHML M5 v"+FIRMWARE_REVISION);

  bleInit(FIRMWARE_REVISION);

  DATA.ppgBSL = 3; // set PPG body sensor location
  DATA.gsrBSL = 3; // set GSR body sensor location
}

void loop()
{
  // Serial.println("looping");
  // M5.Lcd.drawRect(0, 0, 100, 30, BLACK);
  bleLCD();
  bleRun();
  
  if (deviceConnected) {
    incrementDataDummy();
  }

  Serial.print("BLE: Heart Rate: 0x");
  Serial.println(DATA.heartRate, HEX);
  delay(1000);
}

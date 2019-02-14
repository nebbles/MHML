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

void setup()
{
    Serial.begin(115200);
    M5.begin();
    Wire.begin();
    M5.Speaker.mute();

    bleInit();

    M5.Lcd.fillScreen(BLACK);
    M5.Lcd.setTextColor(GREEN, BLACK);
    M5.Lcd.setTextSize(2);
    M5.Lcd.setCursor(0, 0);
    M5.Lcd.print("MHML M5");
}

void loop()
{
    // M5.Lcd.drawRect(0, 0, 100, 30, BLACK);
    bleRun();
}

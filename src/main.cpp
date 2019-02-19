/* 
  M5Stack Fire
    
  MPU9250 Basic Example Code
  
  Demonstrate basic MPU-9250 functionality including parameterizing the register
  addresses, initializing the sensor, getting properly scaled accelerometer,
  gyroscope, and magnetometer data out. Added display functions to allow display
  to on breadboard monitor.
*/

#include <M5Stack.h>
#include "gsr.h"

void setup()
{
  M5.begin();
  Wire.begin();
  Serial.begin(115200);

  gsrInit();

  // Set text to show script is running
  M5.Lcd.setTextColor(YELLOW);
  M5.Lcd.setTextSize(2);
  M5.Lcd.setCursor(0, 0);
  M5.Lcd.print("GSR Script Running");

  M5.Lcd.drawLine(40, 20, 40, 220, WHITE);   // Draw y-axis for Graph
  M5.Lcd.drawLine(40, 220, 320, 220, WHITE); // Draw x-axis for Graph
}

void loop()
{
  timeStart = millis();
  gsrRun();

  long pos = map(gsr_average, 0, 4095, 200, 0); // Mapping GSR values to screen height
  int posInt = floor(pos);
  gsrQueue.pushTail(posInt);

  M5.Lcd.fillRect(41, 20, 280, 200, BLACK); // Clear and reset the screen
  for (int i = 0; i < 280; i++)
  {
    int graphPos = gsrQueue[i];
    // Temp fix to prevent diagonal line from being drawn
    if (graphPos > 20 && graphPos < 220)
      M5.Lcd.drawPixel(i + 41, graphPos, BLUE);
  }

  M5.Lcd.fillRect(0, 20, 38, 220, BLACK);
  M5.Lcd.setTextSize(1);
  M5.Lcd.setTextColor(RED, BLACK);
  M5.Lcd.setCursor(10, posInt);
  M5.Lcd.print((int)(gsr_average));

  elapsedTime = millis() - timeStart;
  timeDelay = 200 - elapsedTime;
  Serial.println((int)(timeDelay));
  delay(timeDelay);
}
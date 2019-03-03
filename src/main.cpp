/* 
  M5Stack Fire
    
  Sensors Code
  
  Reading of data from sensors and displaying it on M5Stack screen.

*/

#include <M5Stack.h>
#include "gsr.h"

// #define DEBUG // Uncomment whilst debugging for Serial debug stats.

void setup()
{
  M5.begin();
  Wire.begin();
  Serial.begin(115200);

  gsrInit();

  M5.Lcd.setTextColor(YELLOW); // Set text to show script is running
  M5.Lcd.setTextSize(2);
  M5.Lcd.setCursor(0, 0);
  M5.Lcd.print("GSR Script Running");

  M5.Lcd.drawLine(40, 20, 40, 220, WHITE);   // Draw y-axis for Graph
  M5.Lcd.drawLine(40, 220, 320, 220, WHITE); // Draw x-axis for Graph

  timeStart = millis();
  gsrRun();
}

void loop()
{

  if (millis() - timeStart > 200) // Record GSR at 5 Hz
  {
    gsrRun();
    timeStart = millis();

    M5.Lcd.fillRect(41, 20, 280, 200, BLACK); // Clear and reset the screen

    for (int i = 0; i < 280; i++) // Draw Graph
    {
      int graphPos = gsrQueue[i];

      if (graphPos > 20 && graphPos < 220) // Temp fix to prevent diagonal line from being drawn
        M5.Lcd.drawPixel(i + 41, graphPos, BLUE);
    }

    M5.Lcd.fillRect(0, 20, 38, 220, BLACK); // Write 0 --> 4095 GSR value
    M5.Lcd.setTextSize(1);
    M5.Lcd.setTextColor(RED, BLACK);
    M5.Lcd.setCursor(10, posInt);
    M5.Lcd.print((int)(gsr_average));
  }

  elapsedTime = millis() - timeStart;

  #ifdef DEBUG
  Serial.println("[DEBUG]: Loop Complete");
  Serial.println((int)(elapsedTime));
  #endif // DEBUG
}
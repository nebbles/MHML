/* 
  M5Stack Fire
    
  Sensors Code
  
  Reading of data from sensors and displaying it on M5Stack screen.

*/

#include <M5Stack.h>
#include "gsr.h"
#include "ppg.h"

#define DEBUG // Uncomment whilst debugging for Serial debug stats.

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
  }

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
}

ppgCalc(); //this calculates the heart rate and prints via Serial

// PPG Information
M5.Lcd.fillRect(10, 185, 140, 10, BLACK);
M5.Lcd.setTextColor(WHITE, BLACK);
M5.Lcd.setTextSize(1);

M5.Lcd.setCursor(10, 185);
M5.Lcd.print("HR:  ");
M5.Lcd.setCursor(30, 185);
M5.Lcd.print((int)(n_heart_rate));

M5.Lcd.setCursor(80, 185);
M5.Lcd.print("SpO2:  ");
M5.Lcd.setCursor(110, 185);
M5.Lcd.print((int)(n_spo2));
}

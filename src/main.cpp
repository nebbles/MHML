#include <M5Stack.h>
#include "ppg.h"

void setup() {

  M5.begin();
  Wire.begin();
  Serial.begin(115200);

  M5.Lcd.setTextColor(YELLOW);
  M5.Lcd.setTextSize(2);
  M5.Lcd.setCursor(0, 0);
  M5.Lcd.print("PPG Script: Started");
  M5.Lcd.setCursor(0, 20);
  M5.Lcd.print("Press Any Button");

  while (true)
  {
  if (M5.BtnA.wasPressed() || M5.BtnB.wasPressed() || M5.BtnC.wasPressed()) break;
  M5.update();
  }

  ppgInit();

  // Write text to show that Measurement has Started
  M5.Lcd.setTextColor(YELLOW, BLACK);
  M5.Lcd.setTextSize(2);
  M5.Lcd.setCursor(0, 0);
  M5.Lcd.print("PPG Script: Serial Running");

  M5.Lcd.drawLine(40, 20, 40, 220, WHITE);
  M5.Lcd.drawLine(40, 220, 320, 220, WHITE);
}

void loop() //Continuously taking samples from MAX30102.  Heart rate and SpO2 are calculated every ST seconds
{
//buffer length of BUFFER_SIZE stores ST seconds of samples running at FS sps
//read BUFFER_SIZE samples, and determine the signal range
for(i=0;i<BUFFER_SIZE;i++)
{
    while(digitalRead(oxiInt)==1);  //wait until the interrupt pin asserts
    ppgInter();

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


M5.Lcd.fillRect(0, 221, 320, 240, BLACK);  

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

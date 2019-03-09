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
#include "data.h"
#include "ble.h"
#include "gsr.h"
#include "ppg.h"

String FIRMWARE_REVISION = "0.2.3";

int button;
#define DEBUG_MAIN // Uncomment whilst debugging for Serial debug stats.

void setup()
{
  Serial.begin(115200);
  M5.begin();
  Wire.begin();
  M5.Speaker.mute();

  M5.Lcd.fillScreen(BLACK);
  M5.Lcd.setTextColor(WHITE, BLACK);
  M5.Lcd.setTextSize(4);
  M5.Lcd.setCursor(112, 50);
  M5.Lcd.print("MHML");

  M5.Lcd.setTextColor(GREEN, BLACK);
  M5.Lcd.setTextSize(2);
  M5.Lcd.setCursor(124, 90);
  M5.Lcd.print("v" + FIRMWARE_REVISION);

  M5.Lcd.setTextColor(GREEN);
  M5.Lcd.setTextSize(2);
  M5.Lcd.setCursor(22, 120);
  M5.Lcd.print("github.com/nebbles/MHML");

  M5.Lcd.setTextColor(YELLOW);
  M5.Lcd.setTextSize(2);
  M5.Lcd.setCursor(28, 170);
  M5.Lcd.print("Select Sensor Location");

  M5.Lcd.setTextColor(MAGENTA);
  M5.Lcd.setTextSize(2);
  M5.Lcd.setCursor(0, 220);
  M5.Lcd.print("   Finger  Wrist   Other  ");

  while (true)
  {
    if (M5.BtnA.wasPressed())
    {
      button = 1;
      break;
    }
    else if (M5.BtnB.wasPressed())
    {
      button = 2;
      break;
    }
    else if (M5.BtnC.wasPressed())
    {
      button = 3;
      break;
    }
    M5.update();
  }

  DATA.ppgBSL = button; // set PPG body sensor location
  DATA.gsrBSL = button; // set GSR body sensor location
  bleInit(FIRMWARE_REVISION);

  ppgInit();
  gsrInit();

  // Reset Screen
  M5.Lcd.fillRect(0, 0, 320, 280, BLACK);

  // Write text to show that Measurement has Started
  M5.Lcd.setTextColor(WHITE, BLACK);
  M5.Lcd.setTextSize(2);
  M5.Lcd.setCursor(0, 0);
  M5.Lcd.print("MHML M5 Sensors: Reading");

  // PPG Graph
  M5.Lcd.drawLine(10, 170, 150, 170, WHITE); // Draw x-axis for Graph
  M5.Lcd.drawLine(10, 30, 10, 170, WHITE);   // Draw y-axis for Graph

  M5.Lcd.setTextColor(WHITE, BLACK);
  M5.Lcd.setTextSize(1);
  M5.Lcd.setCursor(10, 175);
  M5.Lcd.print("PPG Graph");

  // GSR Graph
  M5.Lcd.drawLine(170, 170, 310, 170, WHITE); // Draw x-axis for Graph
  M5.Lcd.drawLine(170, 30, 170, 170, WHITE);  // Draw y-axis for Graph

  M5.Lcd.setTextColor(WHITE, BLACK);
  M5.Lcd.setTextSize(1);
  M5.Lcd.setCursor(170, 175);
  M5.Lcd.print("GSR Graph");

  timeStartGSR = millis();
  gsrRun();

#ifdef DEBUG_MAIN
  Serial.println("[DEBUG] Setup Loop Complete");
#endif //DEBUG_MAIN
}

/* 
 * Draw the raw PPG data to graph on LCD.  
 */
void drawGraphPPG()
{
  M5.Lcd.fillRect(11, 30, 140, 140, BLACK); // Clear and reset PPG Screen
  for (int i = 0; i < 280; i++)
  {
    int graphPos = ppgDeque[i];
    // Temp fix to prevent diagonal line from being drawn
    if (graphPos > 30 && graphPos < 170)
      M5.Lcd.drawPixel(i + 11, graphPos, RED);
  }
}

/* 
 * Draw the raw GSR data to graph on LCD.  
 */
void drawGraphGSR()
{
  M5.Lcd.fillRect(171, 30, 140, 140, BLACK); // Clear and reset GSR Graph
  for (int i = 0; i < 280; i++)              // Draw Graph
  {
    int graphPos = gsrDeque[i];
    // Temp fix to prevent diagonal line from being drawn
    if (graphPos > 30 && graphPos < 170)
      M5.Lcd.drawPixel(i + 171, graphPos, YELLOW);
  }
}

void loop()
{
  // bleLCD(); // debug BLE information to LCD
  bleRun(); // general BLE activity

  ppgCollectSamples(); // collects samples from buffer if available
  ppgCalc();           // applies algorithm for heart rate and spO2
  drawGraphPPG();

  if (millis() - timeStartGSR > 200) // Limits to 5 Hz
  {
    gsrRun();
    drawGraphGSR();
    timeStartGSR = millis(); // Limits to 5 Hz

#ifdef DEBUG_GSR
    Serial.print("[DEBUG] GSR was run: ");
    Serial.println((int)(timeStartGSR));
#endif // DEBUG_GSR
  }

  /* 
   * Update data structure with latest values
   */
  DATA.heartRate = n_heart_rate;
  DATA.spo2 = n_spo2;
  DATA.scl = gsr_average;

  /* 
   * LCD space for displaying sensor values
   */
  M5.Lcd.fillRect(0, 185, 320, 10, BLACK);
  M5.Lcd.setTextColor(WHITE, BLACK);
  M5.Lcd.setTextSize(1);

  M5.Lcd.setCursor(10, 185);
  M5.Lcd.print("HR:");
  M5.Lcd.setCursor(28, 185);
  M5.Lcd.print((int)(DATA.heartRate));

  M5.Lcd.setCursor(80, 185);
  M5.Lcd.print("SpO2:");
  M5.Lcd.setCursor(110, 185);
  M5.Lcd.print((int)(DATA.spo2));

  M5.Lcd.setCursor(170, 185);
  M5.Lcd.print("SCL:");
  M5.Lcd.setCursor(194, 185);
  M5.Lcd.print((int)(DATA.scl));

  /*
   * LCD space for BLE info text. Chars are dx=6, dy=10.
   * x: 0   -> 320 - 'char width'
   * y: 195 -> 240 - 'char height'
   */
  M5.Lcd.setTextColor(BLUE, BLACK);
  M5.Lcd.setCursor(10, 195);
  M5.Lcd.print("BLE:");
  M5.Lcd.setCursor(34, 195);
  if (deviceConnected)
  {
    M5.Lcd.setTextColor(GREEN, BLACK);
    M5.Lcd.print("Device connected.");
  }
  else
  {
    M5.Lcd.fillRect(34, 195, 320, 10, BLACK);
    M5.Lcd.setTextColor(RED, BLACK);
    M5.Lcd.print("No connection.");
  }
}
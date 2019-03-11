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

String FIRMWARE_REVISION = "0.3.0";
String DEVICE_NAME = "MHML M5 B";
enum class Modes
{
  FULL,
  BLE_ONLY
} MODE;

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
  M5.Lcd.setCursor(64, 90);
  M5.Lcd.print(DEVICE_NAME+" v" + FIRMWARE_REVISION);

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

  MODE = Modes::FULL;
  int button = 0;
  while (true)
  {
    if (M5.BtnA.isPressed() && M5.BtnC.isPressed())
    {
      // M5.powerOFF();

      MODE = Modes::BLE_ONLY;
      break;
    }
    else if (M5.BtnA.pressedFor(50))
    {
      button = 3; // BSL = Finger (see BLE spec)
      break;
    }
    else if (M5.BtnB.pressedFor(50))
    {
      button = 2; // BSL = Wrist (see BLE spec)
      break;
    }
    else if (M5.BtnC.pressedFor(50))
    {
      button = 0; // BSL = Other (see BLE spec)
      break;
    }
    M5.update();
  }

  DATA.ppgBSL = button; // set PPG body sensor location
  DATA.gsrBSL = button; // set GSR body sensor location

  bleInit(DEVICE_NAME, FIRMWARE_REVISION);

  // Reset Screen
  M5.Lcd.fillRect(0, 0, 320, 280, BLACK);

  // Write text to show that Measurement has Started
  M5.Lcd.setTextColor(WHITE, BLACK);
  M5.Lcd.setTextSize(2);
  M5.Lcd.setCursor(0, 0);
  M5.Lcd.print((String)DEVICE_NAME + " v" + FIRMWARE_REVISION);

  if (MODE == Modes::FULL)
  {
    ppgInit();
    gsrInit();

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
  }

#ifdef DEBUG_MAIN
  Serial.println("[DEBUG] Setup Loop Complete");
#endif //DEBUG_MAIN
}

/* 
 * Draw the raw PPG data to graph on LCD.  
 */
void drawGraphPPG()
{
<<<<<<< HEAD
  // bleLCD(); // debug BLE information to LCD
  bleRun(); // general BLE activity

  /* 
   * Collect samples from MAX30102 when there is data to process.  
   * Heart rate and SpO2 are calculated every ST seconds.
   */
  if (interruptCounter > 0)
  {
    ppgBufferProcess();
    portENTER_CRITICAL(&mux);
    interruptCounter--;
    portEXIT_CRITICAL(&mux);

#ifdef DEBUG
    Serial.print("[DEBUG] interruptCounter: ");
    Serial.println((int)(interruptCounter));
    Serial.print("[DEBUG] bufferIncremet: ");
    Serial.println((int)(bufferIncrement));
#endif // DEBUG
  }

  ppgCalc(); // calculates heart rate
  Serial.print("Heart Rate: ");
  Serial.println((int)(DATA.heartRate));
  /* 
   * Draw the raw PPG data to graph on LCD.  
   */
=======
>>>>>>> b3f1afb8ce1992e45cb4b8644ff587107e89b533
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

/* 
 * LCD space for displaying sensor values
 */
void drawLcdSensorValues()
{
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
}

/*
 * LCD space for BLE info text. Chars are dx=6, dy=10.
 * x: 0   -> 320 - 'char width'
 * y: 195 -> 240 - 'char height'
 */
void drawLcdBleStatus()
{
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

  M5.Lcd.setTextColor(WHITE);
  M5.Lcd.setCursor(10, 205);
  M5.Lcd.print("PPG BSL:");
  M5.Lcd.setCursor(58, 205);
  M5.Lcd.print(DATA.ppgBSL);
}

void loop()
{
  if (MODE == Modes::BLE_ONLY)
  {
    bleLCD(); // debug BLE information to LCD
    bleRun(); // general BLE activity
    if (deviceConnected)
    {
      simulateDataChanges();
      delay(800);
    }
    return;
  }

  bleRun(); // general BLE activity

  ppgCollectSamples(); // collects samples from buffer if available
  ppgCalc();           // applies algorithm for heart rate and spO2
  drawGraphPPG();

  if (millis() - timeStartGSR > 200) // limits to 5 Hz
  {
    gsrRun();
    drawGraphGSR();
    timeStartGSR = millis(); // resets timer

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

  drawLcdSensorValues();
  drawLcdBleStatus();
}
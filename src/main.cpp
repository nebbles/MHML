/* 
  M5Stack Fire
    
  MPU9250 Basic Example Code
  
  Demonstrate basic MPU-9250 functionality including parameterizing the register
  addresses, initializing the sensor, getting properly scaled accelerometer,
  gyroscope, and magnetometer data out. Added display functions to allow display
  to on breadboard monitor.
*/
#include <M5Stack.h>
#include "BluetoothSerial.h"

BluetoothSerial ESP_BT;

void setup()
{
    M5.begin();
    Wire.begin();
    Serial.begin(115200);

    ESP_BT.begin("ESP32_LED_Control"); //Name of your Bluetooth Signal
    Serial.println("Bluetooth Device is Ready to Pair");

    M5.Speaker.mute();
}

int counter = 0;
char newByte;

void loop()
{
    M5.Lcd.fillScreen(BLACK);
    M5.Lcd.setTextColor(GREEN, BLACK);
    M5.Lcd.setTextSize(2);
    M5.Lcd.setCursor(0, 0);
    M5.Lcd.print("MPU9250/AK8963");
    M5.Lcd.setCursor(0, 32);

    if (ESP_BT.available()) //Check if we receive anything from Bluetooth
    {
        newByte = ESP_BT.read();

        Serial.print("Received:");
        Serial.println(newByte);

        if (newByte == '\n')
        {
            Serial.println("NEW LINE");
        }

        M5.Lcd.print(newByte);
    }

    if (counter == 30)
    {
        Serial.println("M5 serial is alive...");
        counter = 0;
    }
    counter++;

    delay(100);
}
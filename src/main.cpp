/* 
  M5Stack Fire
    
  MPU9250 Basic Example Code
  
  Demonstrate basic MPU-9250 functionality including parameterizing the register
  addresses, initializing the sensor, getting properly scaled accelerometer,
  gyroscope, and magnetometer data out. Added display functions to allow display
  to on breadboard monitor.
*/
class Deque {
private:
    int theList[280] = {};
    int headIdx = 0;
    void incrementHead()
    {
        if (headIdx == 280)
            headIdx = 0;
        else
            headIdx++;
    }

public:

    void push(int newVal)
    {
        theList[headIdx] = newVal;
        incrementHead();
    }

    int read(int idx)
    {
        int readVal;
        int x = headIdx + idx;
        if (x > 280) x = x - 280;

        readVal = theList[x];
        return readVal;
    }

};

#include <M5Stack.h>

#define TFT_GREY 0x7BEF

const int GSR = 35;
int sensorValue = 0;
int gsr_average = 0;
Deque gsrQueue;

void setup()
{
    M5.begin();
    Wire.begin();
    Serial.begin(9600);

    for(int i=0; i<280; i++) gsrQueue.push(100);
}

void loop()
{
    long sum = 0;
    for (int i = 0; i < 10; i++) //Average the 10 measurements to remove the glitch
    {
        sensorValue = analogRead(GSR);
        sum += sensorValue;
        delay(5);
    }
    gsr_average = sum / 10;

    M5.Lcd.fillScreen(TFT_BLACK);                   // Clear and reset the screen
    M5.Lcd.drawLine(40, 0, 40, 200, YELLOW);        // Draw y-axis for Graph
    M5.Lcd.drawLine(40, 200, 320, 200, YELLOW);     // Draw x-axis for Graph

    long pos = map(gsr_average, 0, 4095, 200, 0);   // Mapping GSR values to screen height
    int posInt = floor(pos);
    gsrQueue.push(posInt);

    for(int i=0; i<280; i++)
    {
    int graphPos = gsrQueue.read(i);
    M5.Lcd.drawPixel(i + 40, graphPos, YELLOW);
    }

    M5.Lcd.setTextColor(YELLOW, BLACK);
    M5.Lcd.setCursor(10, posInt);
    M5.Lcd.print((int)(gsr_average));
}
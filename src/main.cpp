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

    // Set text to show script is running
    M5.Lcd.setTextColor(YELLOW);
    M5.Lcd.setTextSize(2);
    M5.Lcd.setCursor(0,0);
    M5.Lcd.print("GSR Script Running");

    M5.Lcd.drawLine(39, 20, 39, 221, WHITE);        // Draw y-axis for Graph
    M5.Lcd.drawLine(39, 221, 320, 221, WHITE);     // Draw x-axis for Graph

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



    long pos = map(gsr_average, 0, 4095, 200, 0);   // Mapping GSR values to screen height
    int posInt = floor(pos);
    gsrQueue.push(posInt);

    M5.Lcd.fillRect(40, 20, 280, 200, BLACK);                   // Clear and reset the screen
    for(int i=0; i<280; i++)
    {
    int graphPos = gsrQueue.read(i);
    M5.Lcd.drawPixel(i + 41, graphPos, BLUE);
    }

    M5.Lcd.fillRect(0, 20, 38, 220, BLACK);
    M5.Lcd.setTextSize(1);
    M5.Lcd.setTextColor(RED, BLACK);
    M5.Lcd.setCursor(10, posInt);
    M5.Lcd.print((int)(gsr_average));
}
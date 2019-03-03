#ifndef gsr_h
#define gsr_h

#include <M5Stack.h>
#include "Deque.h"

const int GSR = 35;
int sensorValue = 0;
int gsr_average = 0;
int elapsedTime, timeStart, timeDelay;
int posInt;
Deque<int> gsrQueue;

void gsrInit()
{
    gsrQueue.setLimit(280);
    for (int i = 0; i < 280; i++)
        gsrQueue.pushTail(100);
}

void gsrRun()
{
    long sum = 0;
    for (int i = 0; i < 10; i++) //Average the 10 measurements to remove the glitch
    {
        sensorValue = analogRead(GSR);
        sum += sensorValue;
        delay(5);
    }
    gsr_average = sum / 10;

    long pos = map(gsr_average, 0, 4095, 200, 0); // Mapping GSR values to screen height
    posInt = floor(pos);
    gsrQueue.popHead();
    gsrQueue.pushTail(posInt);
}

#endif
#ifndef gsr_h
#define gsr_h

#include <M5Stack.h>
#include "Deque.h"

// class Deque
// {
//   private:
//     int theList[280] = {};
//     int headIdx = 0;
//     void incrementHead()
//     {
//         if (headIdx == 280)
//             headIdx = 0;
//         else
//             headIdx++;
//     }

//   public:
//     void push(int newVal)
//     {
//         theList[headIdx] = newVal;
//         incrementHead();
//     }
//     int read(int idx)
//     {
//         int readVal;
//         int x = headIdx + idx;
//         if (x > 280)
//             x = x - 280;
//         readVal = theList[x];
//         return readVal;
//     }
// };

const int GSR = 35;
int sensorValue = 0;
int gsr_average = 0;
int elapsedTime, timeStart, timeDelay;
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
}

#endif
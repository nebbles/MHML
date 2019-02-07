#include <iostream>
#include "Deque.h"

void myHandler(const char *message)
{
    std::cout << message << std::endl;
}

int main()
{
    std::cout << "Hello" << std::endl;
    myHandler("Testing!");

    // int item = queue.read(0);
    // int x = 0;
    // // int item = (num + 1) % 3;
    // int k = 10;
    // int item = ((x - 1) + k) % k;

    Deque<int> queue;
    queue.setFaultHandler(myHandler);
    queue.setLimit(3);

    queue.pushTail(3);
    queue.pushTail(4);
    queue.pushTail(5);
    queue.pushTail(6);

    queue.setLimit(4);
    queue.pushTail(7);

    queue.popHead();
    queue.setLimit(3);
    queue.pushTail(8);

    queue.setLimit(4);
    queue.pushTail(9);

    for (int i = 0; i < queue.count(); i++)
    {
        std::cout << queue[i] << std::endl;
    }

    // std::cout << item << std::endl;
}

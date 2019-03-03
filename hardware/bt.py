import time
import serial

with serial.Serial('/dev/cu.ESP32_LED_Control-ESP32', 115200) as ser:
    print("Establishing connection to device...", flush=True)
    time.sleep(2)
    i = 0
    while i < 100:
        msg = "hello"
        print("Sending message over serial... {}".format(msg), flush=True)
        ser.write("Hello\n".encode())
        print("Sent.")
        time.sleep(1)
        i+= 0

# with serial.Serial('/dev/cu.usbmodem14201', 57600) as ser:
#     while True:
#         line = ser.readline()   # read a '\n' terminated line
#         print(line)

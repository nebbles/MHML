
print("This is the model module of the ml package being imported...")

a = 3

def main():
    print("Hello from the model module!")

def callme():
    print("I am a special function in the model module.")

if __name__ == "__main__":
    print("This is the ml.model module being run directly!")

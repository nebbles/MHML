import ml.model

print("This is the flask module of the api package being imported...")

def main():
    print("Hello from the flask module! I am now running a special function I need from the ml package...")
    ml.model.callme()

if __name__ == "__main__":
    print("This is the api.flask module being run directly!")

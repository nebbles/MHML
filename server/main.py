import api.flask
import ml.model

def main():
    print("Hello from main!")

if __name__ == "__main__":
    main()
    print("Main is now calling api.flask.main()")
    api.flask.main()
    print("Main is now calling ml.model.main()")
    ml.model.main()

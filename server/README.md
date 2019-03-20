<h2 align="center"><br>Server-Side Development</h2>
<br>

This directory contains the backend development, consisting of a RESTful API and a Machine Learning module.


### Requirements

Install all the [dependencies](https://github.com/nebbles/MHML/blob/backend/server/requirements.txt) using

```bash
virtualenv venv
source venv/bin/activate
pip install -r requirements.txt
```

### Note

- This code version is set to run on your local machine (see [flask.py](https://github.com/nebbles/MHML/blob/backend/server/main.py) line 12). However our system was deployed to a connected Raspberry Pi at mhml.greenberg.io (password can't be disclosed).

- To deploy your Flask app to your own Ubuntu server, follow this [tutorial](https://www.youtube.com/watch?v=kDRRtPO0YPA&t=4s).

- For security reasons, the access to the database requires a `mhml_key.json` file (see [firestore.py](https://github.com/nebbles/MHML/blob/backend/server/api/firestore.py) line 10) which wasn't added to this repository. If you wish to use firestore, you can set up your own database [here](https://firebase.google.com/docs/firestore/quickstart).


### Run

On your local machine, make sure the virtual environment is activated and run the following command:

```bash
python3 main.py
```

To start the Flask app from your Ubuntu server (assuming you have followed the [tutorial](https://www.youtube.com/watch?v=kDRRtPO0YPA&t=4s) above), change the host url in [flask.py](https://github.com/nebbles/MHML/blob/backend/server/main.py) at line 12 to your url, and run the following command:

```bash
gunicorn main:app
```

### ML directory

The `ml/` directory holds the machine learning modules that generates predictions for user's stress levels using a blend of self reported measures and sensor readings. This directory contains both a primitive model and user specific models that are generated after multiple application uses.

ml.py holds the ML class, it features two methods. The first one, predict retrieves the pickle from the model folder and applies it to the session data. Secondly, train generates a predictive model from user specific data and saves the model in a pickle.  

from api.flask import app
import ml.model

@app.route('/api')
def index():
    return "MHML server working", 200
@app.route('/api/teapot')
def teapot():
    return "I'm a teapot", 418

if __name__ == '__main__':
    app.run(host='localhost', port=5000)
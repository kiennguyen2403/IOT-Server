from flask import Flask 
from flask import request
from flask import jsonify
import json
from database import getAll, getID, insert
from device import physicalRead, physicalWrite
from flask_socketio import SocketIO,emit
from flask_cors import CORS

app = Flask(__name__)
app.config['SECRET_KEY'] = 'secret!'
CORS(app,resources={r"/*":{"origins":"*"}})
socketio = SocketIO(app,cors_allowed_origins="*")


@app.route('/')
def hello():
    return 'IoT project'

@app.route ('/leds', methods = ['GET'])
def getLeds():
    return getAll()

@app.route ('/led/<led_id>', methods = ['GET'])
def getLed(led_id):
    return getID(led_id)

@app.route('/off/<led_id>', methods = ['POST'])
def turnoff(led_id):
    physicalWrite(led_id+" off")
    resp = jsonify(success=True)
    return resp

@app.route('/on/<led_id>', methods = ['POST'])
def turnon(led_id):
    physicalWrite(led_id+" on")
    resp = jsonify(success=True)
    return resp

@socketio.on("connect")
def connected():
    # result = getAll()
    # print(result[len(result)-1][2])
    bed = getID(0)[2]
    living = getID(1)[2]
    print("client has connected")
    emit("connect",{"bed": bed,"living": living})


@socketio.on('operate')
def on(command):
    insert(command["data"],command["led"])
    physicalWrite(str(command["led"])+": "+command["data"])
    emit("data", "Change success", broadcast=True)

print("Server is running on port 5000")

# if __name__ == 'server':
socketio.run(app, debug=True,port=5000)
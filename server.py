from flask import Flask 
from flask import request
from flask import jsonify
import json
from database import getAll, getID
from device import physicalread, physicalWrite
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




@socketio.on('operate')
def on(command, led_id):
    physicalWrite(led_id+": "+command)
    emit("data",{'data':command, 'led': led_id,'id':request.sid},broadcast=True)


# if __name__ == 'server':
socketio.run(app, debug=True,port=5000)
from flask import Flask 
from flask import request
from flask import jsonify
import json
from database import getAll, getID, insert
from device import physicalRead, physicalWrite
from flask_socketio import SocketIO,emit
from flask_cors import CORS
import threading
import serial
import time

app = Flask(__name__)
app.config['SECRET_KEY'] = 'secret!'
CORS(app,resources={r"/*":{"origins":"*"}})
socketio = SocketIO(app,cors_allowed_origins="*")




# Create a function that reads data from the serial port in a loop
stageCheck = False
lastResponse = ""

def read_serial():
    global lastResponse
    while True:
        data = physicalRead()
        global stageCheck
        if data is not None and lastResponse != data.decode('utf-8').rstrip():
            lastResponse = data.decode('utf-8').rstrip()
            if "warning" in lastResponse:
                print("warning")
                stageCheck = True
            elif "bedroom1" in lastResponse:
                insert("off",0)
                print("trigger bedroom")
                stageCheck = True
            elif "bedroom0" in lastResponse:
                insert("on",0)
                print("trigger bedroom")
                stageCheck = True
            elif "livingroom1" in lastResponse:
                print("trigger livingroom")
                insert("off",1)
                stageCheck = True
            elif "livingroom0" in lastResponse:
                print("trigger livingroom")
                insert("on",1)
                stageCheck = True
            
            
        


# Create a thread that runs the read_serial function




@app.route('/')
def hello():
    return 'IoT project'

@app.route ('/leds', methods = ['GET'])
def getLeds():
    return getAll()

@app.route('/status/<led_id>', methods = ['POST'])
def turnon(led_id):
    data = request.get_json()
    insert(data["data"],led_id)
    resp = jsonify(success=True)
    return resp

@socketio.on("connect")
def connected():
    # result = getAll()
    if getID(0) is None:
        insert("off",0)
    if getID(1) is None:
        insert("off",1)
    bed = getID(0)[2]
    living = getID(1)[2]
    print("Client has connected")
    emit("connect",{"bed": bed,"living": living})

# @socketio.on("warning")
# def warning():
#     emit("warning",{"warning": "warning"},broadcast=True)



@socketio.on('operate')
def on(command):
    insert(command["data"],command["led"])
    physicalWrite(str(command["led"])+command["data"])
    emit("data", "Change success", broadcast=True)
    print("Change success")

     
    

@socketio.on('disconnect')
def disconnect():
    print("client has disconnected")
    emit("disconnect", f"user {request.sid} disconnected", broadcast=True)



print("Server is running on port 5000")
def run_server():
    socketio.run(app, debug=True,port=5000)

# if __name__ == 'server':
serial_thread = threading.Thread(target=read_serial)
serial_thread.start()
socketio.run(app, debug=True, port=5000)



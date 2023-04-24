from flask import Flask 
from flask import request
from flask import jsonify
import json
from database import getAll, getID, insert
from device import physicalRead, physicalWrite
from flask_socketio import SocketIO,emit
from flask_cors import CORS
import threading
import paho.mqtt.client as mqtt

app = Flask(__name__)
app.config['SECRET_KEY'] = 'secret!'
CORS(app,resources={r"/*":{"origins":"*"}})
socketio = SocketIO(app,cors_allowed_origins="*")

client = mqtt.Client()


def on_connect(client, userdata, flags, rc):
    print("Connected with result code "+str(rc))

def on_message(client, userdata, msg):
    print(msg.topic+" "+str(msg.payload))




client.on_connect = on_connect
client.on_message = on_message
client.connect("broker.hivemq.com", 1883, 60)
client.loop_start()





# Create a function that reads data from the serial port in a loop

lastResponse = ""

def read_serial():
    global lastResponse
    while True:
        data = physicalRead()
        if data is not None and data != lastResponse:
            lastResponse = data.decode('utf-8').rstrip()
            socketio.emit("data",{"data": lastResponse},broadcast=True)
        


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
    client.subscribe("iot/led")
    # result = getAll()
    if getID(0) is None:
        insert("off",0)
    if getID(1) is None:
        insert("off",1)
    bed = getID(0)[2]
    living = getID(1)[2]
    print("client has connected")
    emit("connect",{"bed": bed,"living": living})

# @socketio.on("warning")
# def warning():
#     emit("warning",{"warning": "warning"},broadcast=True)



@socketio.on('operate')
def on(command):
    client.publish("iot/led",command["data"])
    insert(command["data"],command["led"])
    physicalWrite(str(command["led"])+command["data"])
    emit("data", "Change success", broadcast=True)
    print(lastResponse)
    if "warning" in lastResponse:
        emit("warning",{"warning": "warning"},broadcast=True)
        print("warning")
    elif "bedroom1":
        insert("off",0)
        emit ("data",{"bed": "off"},broadcast=True)
    elif "bedroom0":
        insert("on",0)
        emit ("data",{"bed": "on"},broadcast=True)
    elif "livingroom1":
        insert("off",1)
        emit ("data",{"living": "off"},broadcast=True)
    elif "livingroom0":
        insert("on",1)
        emit ("data",{"living": "on"},broadcast=True)

@socketio.on('disconnect')
def disconnect():
    client.unsubscribe("iot/led")
    print("client has disconnected")



print("Server is running on port 5000")

# if __name__ == 'server':
serial_thread = threading.Thread(target=read_serial)
serial_thread.start()
socketio.run(app, debug=True,port=5000)
